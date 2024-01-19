using Godot;
using System;

public partial class VolumeControl : GridContainer
{
	[Export]
	public SaveValueKeys BusToControl = SaveValueKeys.Master;
	private int _musicVolume = 0;
	private int _musicBusIndex = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._InitializeMusicControls();
		this._AddVolumeAdjustHandlers();
	}
	
	private void _InitializeMusicControls() {
		this._musicBusIndex = AudioServer.GetBusIndex(BusToControl.ToString());
		float currentVolume = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(this._musicBusIndex));
		this._musicVolume = (int)Math.Round(100f * currentVolume, 2);
		this._SetMusicLevel(this._musicVolume);
	}
	
	private void _SetMusicLevel(int newMusicLevel) {
		this._musicVolume = newMusicLevel;
		Label musicVolumeLbl = GetNode<Label>("Volume");
		musicVolumeLbl.Text = newMusicLevel.ToString() + "%";

		Utilities.SetBusVolume(newMusicLevel, BusToControl.ToString());
		Utilities.UpdateValueForSave(BusToControl, newMusicLevel);
	}
	
	private void _AddVolumeAdjustHandlers() {
		Button lowerMusicBtn = GetNode<Button>("LowerMusicBtn");
		Button raiseMusicBtn = GetNode<Button>("RaiseMusicBtn");
		
		lowerMusicBtn.Pressed += () => {
			int newMusicLevel = this._musicVolume - 10;
			
			if(newMusicLevel <= 0) {
				newMusicLevel = 0;
			}
			
			this._SetMusicLevel(newMusicLevel);
		};
		
		raiseMusicBtn.Pressed += () => {
			int newMusicLevel = this._musicVolume + 10;
			
			if(newMusicLevel >= 100) {
				newMusicLevel = 100;
			}
			
			this._SetMusicLevel(newMusicLevel);
		};
	}
}
