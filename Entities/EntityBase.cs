using Godot;
using System;
using System.Collections.Generic;

public enum Stat
{
	MaxHealth,
	CurrentHealth,
	MaxMana,
	CurrentMana,
	Defense,
	Attack,
	Shield,
	Speed
}

public enum AnimationState
{
	Moving,
	Attacking,
	Casting,
	Damage,
	Death,
	Idle
}

public abstract partial class EntityBase : CharacterBody2D
{
	//Defined here just so I remember how to define dictionarys inline
	protected Dictionary<Stat, int> BaseStats = new Dictionary<Stat, int>()
	{
		{ Stat.CurrentHealth, 100 },
		{ Stat.MaxHealth, 300 },
		{ Stat.Defense, 20 },
		{ Stat.Shield, 150 },
		{ Stat.Speed, 300 }
	};
	protected Dictionary<Stat, int> ModifierStats = new Dictionary<Stat, int>();

	[Export]
	public int Shield;

	protected AnimatedSprite2D AnimatedSprite;
	protected AnimationState CurrentState;
	protected bool left;

	protected abstract void Initialize();
	protected abstract Vector2 GetNormalizedMovementDirection();
	protected virtual int GetMovementSpeed() {
		return BaseStats[Stat.Speed];
	}

	protected int deceleration = 300;

	public virtual void Damage(int damage)
	{
		BaseStats[Stat.CurrentHealth] -= damage;
		
		if(BaseStats[Stat.CurrentHealth] <= 0) {
			Die();
		}
	}

	//Apply Effect could be implemented here since every creature will
	//have the same stats to effect
	public abstract void ApplyEffect(Effect effect);
	protected virtual void Die() {
		QueueFree();
	}

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("Animation");
		Initialize();
	}

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Move();
		UpdateAnimation();
	}

	protected void Move() {
		Vector2 direction = GetNormalizedMovementDirection();
		Vector2 velocity = Velocity;
		
		if (direction != Vector2.Zero)
		{
			velocity = direction * GetMovementSpeed();
			CurrentState = AnimationState.Moving;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, deceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, deceleration);
		}
		
		//for future reference, this should probably be moved since it's not really a part of "moving"
		SetAnimationState(velocity, direction);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void SetAnimationState(Vector2 velocity, Vector2 direction) {
		CurrentState = velocity == Vector2.Zero ? AnimationState.Idle : AnimationState.Moving;

		//not using velocity so if character is knocked back, they still face the same way
		if(direction.X > 0.1) {
			left = false;
		}
		else if(direction.X < -0.1) {
			left = true;
		}
	}

	protected void UpdateAnimation()
	{
		AnimatedSprite.FlipH = !left;
		AnimatedSprite.Play(CurrentState.ToString());
	}
}
