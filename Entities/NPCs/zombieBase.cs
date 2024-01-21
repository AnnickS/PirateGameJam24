using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ZombieBase : EntityBase
{
	[Export]
	private float Threshhold;
	private Vector2 RallyPoint = Vector2.Zero;
	private bool IsForceMove = false;
	
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
		deceleration = 15;
	}

	public override void _Input(InputEvent @event)
	{
		
		if(@event.IsActionPressed("secondary_button"))
		{
			RallyPoint = GetGlobalMousePosition();
			IsForceMove = Input.IsActionPressed("force_move");
		}
	}

	private void _OnBodyEnteringWeaponRange(Node2D body) {
		if(!body.IsInGroup("Enemy") || IsWeaponOnCooldown) {
			return;
		}
		
		Attack((Adversary)body);
	}

	protected override Vector2 GetNormalizedMovementDirection()
	{
		Vector2 vectorToRallyPoint = RallyPoint-GlobalPosition;

		if(ShouldNotMove(vectorToRallyPoint.Length())){
			return Vector2.Zero;
		}

		return vectorToRallyPoint.Normalized();
	}

	private bool ShouldNotMove(float distanceToRallyPoint ) {
		return 
			CurrentState.Equals(AnimationState.Attacking)
		|| (HasValidTarget() && !IsForceMove) 
		|| distanceToRallyPoint <= Threshhold;
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
	
	private void Attack(Adversary body) {
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
		if(HasValidTarget()) {
			Attack(target);
		}
	}
	
	private bool HasValidTarget() {
		return IsInstanceValid(target) && weaponRange.OverlapsBody(target);
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
}
