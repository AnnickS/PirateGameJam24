using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Weapon : Node2D
{
	public enum AttackTypes {
		Stab,
		Slash
	}

	[Export]
	public AttackTypes AttackType;

	[Export]
	public int weaponDamage = 10;

	[Export]
	public float weaponCooldown = 2.0f;

	[Export]
	public float weaponSpeed = 40.0f;

	private Action AttackMethod;
	public Timer attackAnimationTimer;
	public Timer attackCooldownTimer;
	public bool IsWeaponOnCooldown = false;
	public Area2D weaponRange;
	private Area2D weaponHitbox;
	private EntityBase Target;
	private bool IsAttacking = false;

	private Path2D Path;
	private PathFollow2D PathProgress;

	private List<Node2D> HitNodes;

	Sprite2D weaponSprite;
	private float WeaponRangeLength;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print($"Initialize weapon");
		weaponSprite = GetNode<Sprite2D>("WeaponHitbox/WeaponSprite");
		GD.Print($"Initialize weapon sprite ${weaponSprite}");
		weaponHitbox = GetNode<Area2D>("WeaponHitbox");
		GD.Print($"Initialize weapon hitbox ${weaponHitbox}");
		weaponRange = GetNode<Area2D>("WeaponRange");
		GD.Print($"Initialize weapon range ${weaponRange}");
		PathProgress = GetNode<PathFollow2D>("Path2D/PathFollow2D");
		GD.Print($"Initialize weapon pathProg${PathProgress}");
		Path = GetNode<Path2D>("Path2D");
		GD.Print($"Initialize weapon path${Path}");
		
		attackCooldownTimer = GetNode<Timer>("AttackCooldownTimer");
		GD.Print($"Initialize weapon cooldown timer${attackCooldownTimer}");
		attackCooldownTimer.WaitTime = weaponCooldown;
		attackCooldownTimer.Timeout += AttackCoolDownOver;

		WeaponRangeLength = (GetNode<CollisionShape2D>("WeaponRange/CollisionShape2D").Shape as CircleShape2D).Radius;
		GD.Print($"Initialize weapon range length${WeaponRangeLength}");

		weaponHitbox.Reparent(PathProgress);

		SetAttackType();

		weaponHitbox.BodyEntered += OnAttackHittingSomething;

		weaponSprite.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!IsAttacking) {
			return;
		}

		//GD.Print($"PROGRESS {PathProgress.Progress}, DELTA {(float)delta * weaponSpeed}");
		PathProgress.Progress += (float)delta * weaponSpeed;

		if(PathProgress.ProgressRatio >= .99f) {
			AttackFinish();
		}
	}

	private void SetAttackType() {
		if(AttackType == AttackTypes.Stab) {
			GD.Print("IT'S A STAB");
			AttackMethod = Stab;
		}
		else {
			AttackMethod = Slash;
		}
	}

	private void Stab() {
		Vector2 directionToTarget = (Target.GlobalPosition - GlobalPosition).Normalized();
		Path.Curve.ClearPoints();
		Path.Curve.AddPoint(Vector2.Zero);
		Path.Curve.AddPoint(directionToTarget * WeaponRangeLength);
	}

	private void Slash() {
		throw new NotImplementedException();
	}

	public void Attack(EntityBase target) {

		GD.Print($"GETTING CALLED {target}");
		Target = target;
		weaponSprite.Visible = true;
		HitNodes = [];
		IsAttacking = true;
		IsWeaponOnCooldown = true;

		AttackMethod();
		attackCooldownTimer.Start();
	}

	private void OnAttackHittingSomething(Node2D body) {
		if(!IsAttacking || !(body is EntityBase) || HitNodes.Contains(body)) {
			return;
		}

		
		HitNodes.Add(body);
		(body as EntityBase).Damage(weaponDamage);
	}

	private void AttackCoolDownOver() {
		IsWeaponOnCooldown = false;
	}

	private void AttackFinish() {
		IsAttacking = false;
		PathProgress.Progress = 0;
		weaponSprite.Visible = false;
	}
}
