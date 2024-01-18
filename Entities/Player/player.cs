using Godot;
using System;

public partial class Player : EntityBase
{
	    protected override void Initialize()
    {
		return;
    }
	
	protected override void Move(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

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

    public override void Damage(int damage)
    {
        throw new NotImplementedException();
    }
}
