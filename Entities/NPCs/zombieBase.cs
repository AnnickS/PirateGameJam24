using Godot;
using System;

public partial class zombieBase : CharacterBody2D
{
	[Export]
	private int Health;
	[Export]
	public float Speed = 300.0f;
	[Export]
	private float Threshhold = 5f;
	private Vector2 RalleyPoint = Vector2.Zero;
	
	AnimatedSprite2D animatedSprite;

	public void SetThreshhold(float amount)
	{
		Threshhold = amount;
	}
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("Animation");
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionJustReleased("secondary_button"))
		{
			RalleyPoint = GetGlobalMousePosition();
		}

		//The threshhold can increase based on the number of zombies so they don't continually wig out
		if(!(Mathf.Abs(RalleyPoint.X-GlobalPosition.X) < Threshhold) ||
		!(Mathf.Abs(RalleyPoint.Y-GlobalPosition.Y) < Threshhold))
		{
			Vector2 direction = RalleyPoint-GlobalPosition;
			MoveCharacter(direction.Normalized());
		}
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
			animatedSprite.FlipH = !left;
			animatedSprite.Play("walking");
		} else
		{
			animatedSprite.Pause();
			//add idle animation
		}
	}
}
