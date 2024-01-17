using Godot;
using System;

public partial class Adversary : CharacterBody2D
{
	public const float Speed = 300.0f;
	
	[Export]
	private int Health = 100;

	public override void _PhysicsProcess(double delta)
	{
	}
	
	public void TakeDamage(int damage) {
		Health -= damage;
		
		if(Health <= 0) {
			Die();
		}
	}
	
	private void Die() {
		QueueFree();
	}
}
