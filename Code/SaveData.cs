using Godot;
using System;
using System.Linq;

public partial class SaveData : Node
{
	#region Singleton

	public static SaveData Instance
	{
		get;
		private set;
	}

	public SaveData()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree();
			return;
		}
	}
	#endregion

	private const string SavePath = "user://savegame.json";

	public int HighScore = 0;
	public int Money = 0;
	public bool JumpUpgradeBought = false;
	public bool ExtraLifeBought = false;
	public bool BubbleBought = false;
	public float MasterVolume = 0.5f;
	public float MusicVolume = 0.5f;
	public float EffectsVolume = 0.5f;

	public override void _Ready()
	{
		LoadData();
		//DeleteSaveData();
	}

	public bool CheckMoney(int amount)
	{
		if (amount <= Money)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void AddMoney(int amount)
	{
		Money += amount;
		SaveDataToFile();
	}

	public void DecreaseMoney(int amount)
	{
		if (amount > Money)
		{
			return;
		}
		Money -= amount;
		SaveDataToFile();
	}

	public bool CheckHighScore(int score)
	{
		if (score > HighScore)
		{
			HighScore = score;
			SaveDataToFile();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void UpgradeJumps()
	{
		JumpUpgradeBought = true;
		SaveDataToFile();
	}

	public void UpgradeLives()
	{
		ExtraLifeBought = true;
		SaveDataToFile();
	}

	public void UpgradeBubble()
	{
		BubbleBought = true;
		SaveDataToFile();
	}

	public void UpdateAudioSlider(int busIndex, float linearVolume)
	{
		if (busIndex == AudioServer.GetBusIndex(SettingsConfig.MASTER_BUS))
		{
			MasterVolume = linearVolume;
		}
		else if (busIndex == AudioServer.GetBusIndex(SettingsConfig.MUSIC_BUS))
		{
			MusicVolume = linearVolume;
		}
		else if (busIndex == AudioServer.GetBusIndex(SettingsConfig.EFFECTS_BUS))
		{
			EffectsVolume = linearVolume;
		}

		SaveDataToFile();
	}

	private void SaveDataToFile()
	{
		var data = new Godot.Collections.Dictionary
		{
			{ "highscore", HighScore },
			{ "money", Money },
			{ "jumpUpgrade", JumpUpgradeBought },
			{ "extraLife", ExtraLifeBought },
			{ "bubble", BubbleBought },
			{ "master", MasterVolume },
			{ "music", MusicVolume },
			{ "effects", EffectsVolume }
		};

		string json = Json.Stringify(data);

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);

		file.StoreString(json);
	}

	private void LoadData()
	{
		if (!FileAccess.FileExists(SavePath))
		{
			return;
		}

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);

		string json = file.GetAsText();

		var data = Json.ParseString(json).AsGodotDictionary();

		if (data.ContainsKey("highscore"))
		{
			HighScore = (int)data["highscore"];
		}
		if (data.ContainsKey("money"))
		{
			Money = (int)data["money"];
		}
		if (data.ContainsKey("jumpUpgrade"))
		{
			JumpUpgradeBought = (bool)data["jumpUpgrade"];
		}
		if (data.ContainsKey("extraLife"))
		{
			ExtraLifeBought = (bool)data["extraLife"];
		}
		if (data.ContainsKey("bubble"))
		{
			BubbleBought = (bool)data["bubble"];
		}
		if (data.ContainsKey("master"))
		{
			MasterVolume = (float)(double)data["master"];
		}
		if (data.ContainsKey("music"))
		{
			MusicVolume = (float)(double)data["music"];
		}
		if (data.ContainsKey("effects"))
		{
			EffectsVolume = (float)(double)data["effects"];
		}
	}

	private void DeleteSaveData()
	{
		if (FileAccess.FileExists(SavePath))
		{
			DirAccess.RemoveAbsolute(SavePath);
		}
		HighScore = 0;
		Money = 0;
		JumpUpgradeBought = false;
		ExtraLifeBought = false;
		BubbleBought = false;
		MasterVolume = 0.5f;
		MusicVolume = 0.5f;
		EffectsVolume = 0.5f;
	}
}
