using Godot;
using System;
using System.Collections.Generic;

public partial class Player : EntityBase
{
	private List<Spell> AbilityList;

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
			velocity.X = direction.X * BaseStats[Stat.Speed];
			velocity.Y = direction.Y * BaseStats[Stat.Speed];
			CurrentState = AnimationState.Moving;
			left = direction.X < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, BaseStats[Stat.Speed]);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, BaseStats[Stat.Speed]);
			CurrentState = AnimationState.Idle;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void Damage(int damage)
	{
		throw new NotImplementedException();
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}

	protected override void Die()
	{
		throw new NotImplementedException();
	}
}
