using Godot;
using System;

public partial class Adversary : EntityBase
{
	private PathNode NextPathNode;

	protected override void Initialize()
	{
		NextPathNode = (GetTree().GetFirstNodeInGroup("Squad1Path") as NpcPath).StartNode;
	}
	
	protected override Vector2 GetNormalizedMovementDirection()
	{
		if(NextPathNode == null) {
			NextPathNode = (GetTree().GetFirstNodeInGroup("Squad1Path") as NpcPath).StartNode;
		}
		if((NextPathNode.GlobalPosition - GlobalPosition).Length() < 5) {
			NextPathNode = NextPathNode.NextNode;
		}
		
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
