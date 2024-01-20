using Godot;
using System;

public partial class Adversary : EntityBase
{
	public PathNode NextPathNode;

	protected override void Initialize()
	{
		return;
	}
	
	protected override Vector2 GetNormalizedMovementDirection()
	{
		if((NextPathNode.GlobalPosition - GlobalPosition).Length() < 5) {
			NextPathNode = NextPathNode.NextNode;
		}
		GD.Print((NextPathNode.GlobalPosition - GlobalPosition).Normalized());
		return (NextPathNode.GlobalPosition - GlobalPosition).Normalized();
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
	
	protected override void Die() {
		QueueFree();
	}
}
