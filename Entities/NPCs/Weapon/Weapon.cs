using Godot;
using System;
using System.Linq;

[Tool]
public partial class Weapon : Node2D
{
	public override string[] _GetConfigurationWarnings()
	{
		string[] warnings = base._GetConfigurationWarnings();
		Godot.Collections.Array<Node> children = GetChildren();


		bool hasHitbox = false;
		for(int i = 0; i < children.Count; i++) 
		{
			if(children[i] is Area2D) {
				hasHitbox = true;
			}
		}

		if(!hasHitbox) {
			warnings = warnings.Concat(["This node requires an Area2D to function as a hit box!!"]).ToArray();
		}
		return warnings;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
