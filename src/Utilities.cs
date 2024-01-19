using Godot;
using System;

public static class Utility {
    public static void SetBusVolume(int newMusicLevel, string busToControl) {
			float linearVolume = (float)newMusicLevel / (float)100; 
            int musicBusIndex = AudioServer.GetBusIndex(busToControl);
			AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(linearVolume));
	}
}