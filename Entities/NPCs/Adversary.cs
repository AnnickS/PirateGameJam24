using Godot;
using System;

public partial class Adversary : EntityBase
{
	public PathNode NextPathNode;

	private EntityBase target = null;

	private int WeaponDamage = 15;

	protected override void Initialize()
	{
		GD.Print($"STUPID DUMB {target}");
		BaseStats[Stat.Speed] = 350;
		InitializeVision();
		InitializeAttack();
	}
	
	protected override Vector2 GetNormalizedMovementDirection()
	{
		GD.Print($"is isntance {IsInstanceValid(target)}, target {target}");
		if(IsInstanceValid(target)) {
			return (target.GlobalPosition - GlobalPosition).Normalized();
		}

		if((NextPathNode.GlobalPosition - GlobalPosition).Length() < 5) {
			NextPathNode = NextPathNode.NextNode;
		}

		return (NextPathNode.GlobalPosition - GlobalPosition).Normalized();
	}

	protected override int GetMovementSpeed() {
		if(!IsInstanceValid(target)) {
			return BaseStats[Stat.Speed] / 3;
		}
		return BaseStats[Stat.Speed];
	}

	private void InitializeVision() {
		Area2D visionRange = GetNode<Area2D>("VisionRange");
		visionRange.BodyEntered += OnBodyEnteringVisionRange;
	}

	private void InitializeAttack() {
		Area2D visionRange = GetNode<Area2D>("AttackRange");
		visionRange.BodyEntered += OnBodyEnteringWeaponRange;
	}

	private void OnBodyEnteringVisionRange(Node2D body) {
		GD.Print($"{body}, {body is EntityBase}, {ShouldSwitchTargets(body as EntityBase)}");
		if(body is EntityBase && ShouldSwitchTargets(body as EntityBase)) {
			target = body as EntityBase;
		}
	}

	private void OnBodyEnteringWeaponRange(Node2D body) {
		if(body == target) {
			Attack(body as EntityBase);
		}
	}

	private void Attack(EntityBase entity) {
		entity.Damage(WeaponDamage);
	}

	private bool ShouldSwitchTargets(EntityBase entity) {
		return (target == null || entity is Player) && !entity.IsInGroup("Enemy");
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
}
