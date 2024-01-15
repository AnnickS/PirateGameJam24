using Godot;
using System;

public partial class MusicControls : GridContainer
{
	private float _musicLevel = 0f;
	private int _musicBusIndex = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._musicBusIndex = AudioServer.GetBusIndex("MusicBus");
		this._musicLevel = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(this._musicBusIndex));
		
		Button lowerMusicBtn = GetNode<Button>("LowerMusicBtn");
		Button raiseMusicBtn = GetNode<Button>("RaiseMusicBtn");
		
		lowerMusicBtn.Pressed += () => {
			float newMusicLevel = this._musicLevel - 0.1f;
			
			if(newMusicLevel <= 0f) {
				newMusicLevel = 0f;
			}
			
			this._SetMusicLevel(newMusicLevel);
		};
		
		raiseMusicBtn.Pressed += () => {
			float newMusicLevel = this._musicLevel + 0.1f;
			
			if(newMusicLevel >= 1f) {
				newMusicLevel = 1f;
			}
			
			this._SetMusicLevel(newMusicLevel);
		};
	}
	
	private void _SetMusicLevel(float newMusicLevel) {
			this._musicLevel = newMusicLevel;
			AudioServer.SetBusVolumeDb(this._musicBusIndex, Mathf.LinearToDb(newMusicLevel));
	}
}
