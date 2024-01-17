using Godot;
using System;

public partial class zombieBase : CharacterBody2D
{
	[Export]
	private int Health;
	[Export]
	public float Speed = 300.0f;
	[Export]
	private float Threshhold;
	private Vector2 RalleyPoint = Vector2.Zero;
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

	AnimatedSprite2D animatedSprite;

	public void SetThreshhold(float amount)
	{
		Threshhold = amount;
	}
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("Animation");
		_InitializeWeapon();
	}

	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("secondary_button"))
		{
			RalleyPoint = GetGlobalMousePosition();
		}
	}

	private void _OnBodyEnteringWeaponRange(Node2D body) {
		if(!body.IsInGroup("Enemy") || IsWeaponOnCooldown) {
			return;
		}
		
		_Attack((Adversary)body);
	}

	public override void _PhysicsProcess(double delta)
	{
		//The threshhold can increase based on the number of zombies so they don't continually wig out
		Vector2 direction = RalleyPoint-GlobalPosition;
		MoveCharacter(direction.Length(), direction.Normalized(), delta);
	}
	
	private void MoveCharacter(float length, Vector2 direction, double delta)
	{
		Vector2 velocity = Velocity;

		duration = length < Threshhold ? duration + (float)delta : 0f;

		velocity.X = length < Threshhold ? Mathf.MoveToward(Velocity.X, 0, duration/0.02f) : direction.X * Speed;
		velocity.Y = length < Threshhold ? Mathf.MoveToward(Velocity.Y, 0, duration/0.02f) : direction.Y * Speed;

		Velocity = velocity;
		MoveAndSlide();
		UpdateAnimation(!velocity.Equals(Vector2.Zero), direction.X < 0);
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
		body.TakeDamage(weaponDamage);
		weaponSprite.Visible = true;
		
		IsWeaponOnCooldown = true;

		attackCooldownTimer.Start();
		attackAnimationTimer.Start();
	}

	private void _EndAttack() {
		IsWeaponOnCooldown = false;
		if(IsInstanceValid(target) && weaponRange.OverlapsBody(target)) {
			_Attack(target);
		}
	}
	
	private void UpdateAnimation(bool moving, bool left)
	{
		if(moving)
		{
			animatedSprite.FlipH = !left;
			animatedSprite.Play("walking");
		} else
		{
			animatedSprite.Pause();
			duration = 0.0f;
			slowX = false;
			slowY = false;
			//add idle animation
		}
	}
}
