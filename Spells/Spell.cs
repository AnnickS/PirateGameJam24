using Godot;
using System.Collections.Generic;

public enum SpellState
{
	ChargingSFX,
	CastSFX,
	HitSFX
}

public abstract partial class Spell : Node
{
	protected List<Effect> SpellEffects;
	[Export]
	protected EntityBase Caster;
	protected List<EntityBase> AffectedEntities;
	[Export]
	protected AnimatedSprite2D SpellAnimation;
	[Export]
	protected AudioStreamPlayer SpellAudioPlayer;

	protected abstract void Initialize();
	protected abstract void Activate();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Initialize();
	}

	protected void PlayAudio()
	{
		SpellAudioPlayer.Play();
	}

	protected void PlayAnimation(SpellState State)
	{
		SpellAnimation.Play(State.ToString());
	}
}
