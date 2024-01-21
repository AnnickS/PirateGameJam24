using Godot;
using System;
using System.Linq;

public partial class Weapon : Node2D
{
	public Timer attackAnimationTimer;
	public Timer attackCooldownTimer;
	private bool IsWeaponOnCooldown = false;
	private int weaponDamage = 10;
	private Area2D weaponRange;
	private Adversary target;
	Sprite2D weaponSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Area2D GetWeaponHitbox() {
		Godot.Collections.Array<Node> children = GetChildren();

		for(int i = 0; i < children.Count; i++) 
		{
			if(children[i] is Area2D) {
				return children[i] as Area2D;
			}
		}

		return null;
	}

	private Sprite2D GetWeaponSprite() {
		Godot.Collections.Array<Node> children = GetChildren();

		for(int i = 0; i < children.Count; i++) 
		{
			if(children[i] is Sprite2D) {
				return children[i] as Sprite2D;
			}
		}

		return null;
	}
}
