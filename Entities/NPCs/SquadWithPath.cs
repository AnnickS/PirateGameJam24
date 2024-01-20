using Godot;
using System;

public partial class SquadWithPath : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Adversary>("Adversary").NextPathNode = GetNode<NpcPath>("NpcPath").StartNode;
	}
}
