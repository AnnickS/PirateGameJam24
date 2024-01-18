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
	
    protected override void Move(double delta)
    {
        throw new NotImplementedException();
    }

	public override void Damage(int damage)
    {
		BaseStats[Stat.CurrentHealth] -= damage;
		
		if(BaseStats[Stat.CurrentHealth] <= 0) {
			Die();
		}
    }

	public override void ApplyEffect(Effect effect)
    {
        throw new NotImplementedException();
    }
	
	protected override void Die() {
		QueueFree();
	}
}
