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
	
	protected override Vector2 GetNormalizedMovementDirection()
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		return direction;
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
}
