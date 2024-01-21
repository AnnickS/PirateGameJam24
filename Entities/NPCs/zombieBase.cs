using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ZombieBase : EntityBase
{
	[Export]
	private float Threshhold;
	private Vector2 RallyPoint = Vector2.Zero;
	private bool IsForceMove = false;
	
	private Adversary target;
	private Weapon weapon;

	public void SetThreshhold(float amount)
	{
		Threshhold = amount;
	}
	
	protected override void Initialize()
	{
		_InitializeWeapon();
		deceleration = 15;
	}

	public override void _Input(InputEvent @event)
	{
		
		if(@event.IsActionPressed("secondary_button"))
		{
			RallyPoint = GetGlobalMousePosition();
			IsForceMove = Input.IsActionPressed("force_move");
		}
	}

	private void _OnBodyEnteringWeaponRange(Node2D body) {
		if(!(body is EntityBase) || !body.IsInGroup("Enemy") || weapon.IsWeaponOnCooldown) {
			return;
		}
		
		target = body as Adversary;
		Attack((Adversary)body);
	}

	protected override Vector2 GetNormalizedMovementDirection()
	{
		Vector2 vectorToRallyPoint = RallyPoint-GlobalPosition;

		if(ShouldNotMove(vectorToRallyPoint.Length())){
			return Vector2.Zero;
		}

		return vectorToRallyPoint.Normalized();
	}

	private bool ShouldNotMove(float distanceToRallyPoint ) {
		return 
			weapon.IsWeaponOnCooldown
		|| (HasValidTarget() && !IsForceMove) 
		|| distanceToRallyPoint <= Threshhold;
	}
	
	private void _InitializeWeapon() {
		weapon = GetNode<Weapon>("Weapon");
		weapon.weaponRange.BodyEntered += _OnBodyEnteringWeaponRange;
		weapon.attackCooldownTimer.Timeout += _EndAttack;
	}

	
	private void Attack(Adversary body) {
		CurrentState = AnimationState.Idle;
		weapon.Attack(body);
	}

	private void _EndAttack() {
		if(HasValidTarget()) {
			Attack(target);
		}
	}
	
	private bool HasValidTarget() {
		return IsInstanceValid(target) && weapon.weaponRange.OverlapsBody(target);
	}

	public override void ApplyEffect(Effect effect)
	{
		throw new NotImplementedException();
	}
}
