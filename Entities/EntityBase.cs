using Godot;
using System;

public enum State
	{
		Moving,
		Damage,
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

	protected abstract void Move();
	protected abstract void Damage();
	protected abstract void UpdateAnimation();

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("Animation");
	}
}
