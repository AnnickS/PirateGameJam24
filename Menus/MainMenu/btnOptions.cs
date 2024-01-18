using Godot;
using System;

public partial class btnOptions : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Action changeSceneToOptions = () => {
			Node optionsScene = ResourceLoader.Load<PackedScene>("res://Menus/OptionsMenu/OptionsMenu.tscn").Instantiate();
			GetTree().Root.AddChild(optionsScene);
		};
		
		this.Pressed += changeSceneToOptions;
	}
}
