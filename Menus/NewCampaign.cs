using Godot;
using System;

public partial class NewCampaign : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Action changeSceneToNewCampaign = () => {
			GetTree().ChangeSceneToFile("res://Levels/BasicLevel.tscn");
		};
		
		this.Pressed += changeSceneToNewCampaign;
	}
}
