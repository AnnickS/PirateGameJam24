using Godot;
using System;

public partial class Player : EntityBase
{
	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Move();
		UpdateAnimation();
	}

	protected override void Move()
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		GD.Print("Velocity: ", direction);
		Vector2 velocity = Velocity;
		
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			CurrentState = State.Moving;
			left = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			CurrentState = State.Idle;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    protected override void Damage()
    {
        throw new NotImplementedException();
    }

    protected override void UpdateAnimation()
	{
		switch(CurrentState)
		{
			case State.Moving:
				AnimatedSprite.Play("walking");
				AnimatedSprite.FlipH = !left;
				break;
			case State.Damage:
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
