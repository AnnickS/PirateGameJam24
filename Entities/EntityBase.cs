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
	protected int GetMovementSpeed() {
		return BaseStats[Stat.Speed];
	}

	protected int deceleration = 300;

	public abstract void Damage(int damage);

	//Apply Effect could be implemented here since every creature will
	//have the same stats to effect
	public abstract void ApplyEffect(Effect effect);
	protected abstract void Die();

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
		
		CurrentState = velocity == Vector2.Zero ? AnimationState.Idle : AnimationState.Moving;

		Velocity = velocity;
		
		if(direction.X > 0.1) {
			left = false;
		}
		else if(direction.X < -0.1) {
			left = true;
		}
		
		MoveAndSlide();
	}

	protected void UpdateAnimation()
	{
		AnimatedSprite.FlipH = !left;
		AnimatedSprite.Play(CurrentState.ToString());
	}
}
