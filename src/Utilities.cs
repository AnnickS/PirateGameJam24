using Godot;
using System;

public partial class Utilities:Node {
	private static string SaveFilePath = "user://SaveFile.dat";
	private static Godot.Collections.Dictionary<string, Variant> SaveFileData;
	
	public override void _Ready() {
		Load();
		SetPreferences();
	}
	
	public static void SetBusVolume(int newMusicLevel, string busToControl) {
		float linearVolume = (float)newMusicLevel / (float)100; 
		int musicBusIndex = AudioServer.GetBusIndex(busToControl);
		AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(linearVolume));
	}
	
	public static void UpdateValueForSave(SaveValueKeys key, Variant value) {
		string keyString = key.ToString();
		SaveFileData[keyString] = value;
	}
	
	public static Variant GetSaveValue(SaveValueKeys key) {
		return SaveFileData[key.ToString()];
	}
	
	public static void Save() {
		using var saveFile = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
		var jsonString = Json.Stringify(SaveFileData);
		saveFile.StoreLine(jsonString);
		saveFile.Close();
	}
	
	private static void Load() {
		using var saveFile = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
		
		if(saveFile == null) {
			FirstTimeCreateSaveFile();
			return;
		}
		
		var saveFileJsonString = saveFile.GetLine();
		var jsonObject = new Json();
		var parseResult = jsonObject.Parse(saveFileJsonString);
		
		if(parseResult != Error.Ok) {
			GD.Print($"Load Game JSON Parse Error: {jsonObject.GetErrorMessage()} in {saveFileJsonString} at line {jsonObject.GetErrorLine()}");
		}
		
		SaveFileData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)jsonObject.Data);
	}
	
	private static void SetPreferences() {
		SetBusVolume((int)GetSaveValue(SaveValueKeys.Master), SaveValueKeys.Master.ToString());
		SetBusVolume((int)GetSaveValue(SaveValueKeys.Music), SaveValueKeys.Music.ToString());
	}
	
	private static void FirstTimeCreateSaveFile() {
		SaveFileData = new Godot.Collections.Dictionary<string, Variant>();
		UpdateValueForSave(SaveValueKeys.Master, 70);
		UpdateValueForSave(SaveValueKeys.Music, 100);
		
		Save();
	}
}

public enum SaveValueKeys {
	Music,
	Master,
}


