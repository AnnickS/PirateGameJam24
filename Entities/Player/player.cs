using Godot;
using System;

public partial class player : CharacterBody2D
{
	[Export]
	private float health;
	[Export]
	public float Speed = 300.0f;
	AnimatedSprite2D animatedSprite;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("Animation");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
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
		UpdateAnimation(!direction.Equals(Vector2.Zero), direction.X < 0);
	}
	
	private void UpdateAnimation(bool moving, bool left)
	{
		if(moving)
		{
			animatedSprite.FlipH = left;
			animatedSprite.Play();
		} else
		{
			animatedSprite.Pause();
			//add idle animation
		}
	}
}
