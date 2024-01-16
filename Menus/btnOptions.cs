using Godot;
using System;

public partial class btnOptions : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Action changeSceneToOptions = () => {
			GetTree().ChangeSceneToFile("res://Menus/OptionsMenu.tscn");
		};
		
		this.Pressed += changeSceneToOptions;
	}
}
