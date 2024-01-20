using Godot;
using System;

public partial class Adversary : EntityBase
{
	protected override void Initialize()
	{
		GD.Print("ERROR, INITIALZIZE NOT SET YET FOR ADVERSARY");
	}

	public override void _PhysicsProcess(double delta)
	{
	}
	
	protected override Vector2 GetNormalizedMovementDirection()
	{
		return Vector2.Zero;
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
	
	protected override void Die() {
		QueueFree();
	}
}
