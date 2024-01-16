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
