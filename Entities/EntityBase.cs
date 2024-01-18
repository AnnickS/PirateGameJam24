using Godot;
using System;

public enum State
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
	[Export]
	protected int Shield;
	[Export]
	protected int Health;
	[Export]
	protected float Speed = 300.0f;
	protected AnimatedSprite2D AnimatedSprite;
	protected State CurrentState;
	protected bool left;

	protected abstract void Initialize();
	protected abstract void Move(double delta);
	public abstract void Damage(int damage);

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("Animation");
		Initialize();
	}

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Move(delta);
		UpdateAnimation();
	}

	protected void UpdateAnimation()
	{
		AnimatedSprite.FlipH = !left;

		switch(CurrentState)
		{
			case State.Moving:
				AnimatedSprite.Play("walking");
				break;
			case State.Attacking:
				//Add attacking animation
				break;
			case State.Casting:
				//Add casting animation
				break;
			case State.Damage:
				//Add damage animation
				break;
			case State.Death:
				//Add death animation
				break;
			case State.Idle:
				//Add Idle Animation here
				AnimatedSprite.Pause();
				break;
			default:
				break;
		}
	}
}
