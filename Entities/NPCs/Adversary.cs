using Godot;
using System;

public partial class Adversary : EntityBase
{
	protected override void Initialize()
    {
        throw new NotImplementedException();
    }

	public override void _PhysicsProcess(double delta)
	{
	}
	
	public override void Damage(int damage)
    {
        Health -= damage;
		
		if(Health <= 0) {
			Die();
		}
    }
	
	private void Die() {
		QueueFree();
	}

    protected override void Move(double delta)
    {
        throw new NotImplementedException();
    }
}
