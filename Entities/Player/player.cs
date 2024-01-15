using Godot;
using System;

public partial class player : CharacterBody2D
{
	[Export]
	public const float Speed = 300.0f;

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		MoveCharacter(direction);
	}
	
	private void MoveCharacter(Vector2 direction)
	{
		Vector2 velocity = Velocity;
		
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
