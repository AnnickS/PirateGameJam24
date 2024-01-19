using Godot;
using System;

public partial class SaveOptions : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Action changeSceneToMainMenu = () => {
			GetNode("/root/OptionsMenu").QueueFree();
			Utilities.Save();
		};
		
		this.Pressed += changeSceneToMainMenu;
	}
}
