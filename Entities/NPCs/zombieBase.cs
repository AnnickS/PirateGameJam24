using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ZombieBase : EntityBase
{
	[Export]
	private float Threshhold;
	private Vector2 RalleyPoint = Vector2.Zero;
	private bool IsForceMove = false;
	private float duration = 0f;
	private bool slowX = false;
	private bool slowY = false;
	
	private Timer attackAnimationTimer;
	private Timer attackCooldownTimer;
	private bool IsWeaponOnCooldown = false;
	private int weaponDamage = 10;
	private Adversary target;
	private Area2D weaponRange;
	AnimatedSprite2D weaponSprite;

	public void SetThreshhold(float amount)
	{
		Threshhold = amount;
	}
	
	protected override void Initialize()
	{
		_InitializeWeapon();
	}

	public override void _Input(InputEvent @event)
	{
		
		if(@event.IsActionPressed("secondary_button"))
		{
			RalleyPoint = GetGlobalMousePosition();
			IsForceMove = Input.IsActionPressed("force_move");
		}
	}

	private void _OnBodyEnteringWeaponRange(Node2D body) {
		if(!body.IsInGroup("Enemy") || IsWeaponOnCooldown) {
			return;
		}
		
		_Attack((Adversary)body);
	}
	
	protected override void Move(double delta)
	{
		if(CurrentState.Equals(AnimationState.Attacking)) {
			return;
		}
		
		if(_HasValidTarget() && !IsForceMove){
			return;
		}

		Vector2 direction = (RalleyPoint-GlobalPosition).Normalized();
		float length = (RalleyPoint-GlobalPosition).Length();
		Vector2 velocity = Velocity;

		duration = length < Threshhold ? duration + (float)delta : 0f;

		velocity.X = length < Threshhold ? Mathf.MoveToward(Velocity.X, 0, duration/0.01f) : direction.X * BaseStats[Stat.Speed];
		velocity.Y = length < Threshhold ? Mathf.MoveToward(Velocity.Y, 0, duration/0.01f) : direction.Y * BaseStats[Stat.Speed];
		left = direction.X < 0;

		Velocity = velocity;
		MoveAndSlide();

		if(Velocity.Equals(Vector2.Zero))
		{
			CurrentState = AnimationState.Idle;
			duration = 0.0f;
			slowX = false;
			slowY = false;
		} else
		{
			CurrentState = AnimationState.Moving;
		}
	}
	
	private void _InitializeWeapon() {
		weaponSprite = GetNode<AnimatedSprite2D>("Weapon");
		weaponSprite.Visible = false;
		
		weaponRange = GetNode<Area2D>("WeaponRange");
		weaponRange.BodyEntered += _OnBodyEnteringWeaponRange;
 
		attackAnimationTimer = GetNode<Timer>("AttackAnimationTimer");
		attackCooldownTimer = GetNode<Timer>("AttackCooldownTimer");
		attackAnimationTimer.Timeout += () => {
			weaponSprite.Visible = false;
		};
		attackCooldownTimer.Timeout += _EndAttack;
	}
	
	private void _Attack(Adversary body) {
		target = body;
		body.Damage(weaponDamage);
		weaponSprite.Visible = true;
		CurrentState = AnimationState.Idle;
		
		IsWeaponOnCooldown = true;

		attackCooldownTimer.Start();
		attackAnimationTimer.Start();
	}

	private void _EndAttack() {
		IsWeaponOnCooldown = false;
		if(_HasValidTarget()) {
			_Attack(target);
		}
	}
	
	private bool _HasValidTarget() {
		return IsInstanceValid(target) && weaponRange.OverlapsBody(target);
	}

	public override void Damage(int damage)
	{
		throw new NotImplementedException();
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}

	protected override void Die()
	{
		throw new NotImplementedException();
	}
}
