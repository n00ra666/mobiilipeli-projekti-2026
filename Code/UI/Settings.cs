using Godot;
using System;

public partial class Settings : Control
{

	[Export] private Slider _masterSlider;
	[Export] private Slider _musicSlider;
	[Export] private Slider	_effectsSlider;
	[Export] private Button _returnButton;

	private string _mainMenuScenePath = "res://Scenes/mainmenu.tscn";

	private int _masterBusIndex = -1;
	private int _musicBusIndex = -1;
	private int _effectsBusIndex = -1;

    public override void _EnterTree()
	{
		_masterSlider.ValueChanged += OnMasterValueChanged;
		_musicSlider.ValueChanged += OnMusicValueChanged;
		_effectsSlider.ValueChanged += OnEffectsValueChanged;

		_returnButton.Pressed += OnReturnButtonPressed;
	}
	
    public override void _ExitTree()
	{
		_masterSlider.ValueChanged -= OnMasterValueChanged;
		_musicSlider.ValueChanged -= OnMusicValueChanged;
		_effectsSlider.ValueChanged -= OnEffectsValueChanged;

		_returnButton.Pressed -= OnReturnButtonPressed;
	}

    public override void _Ready()
	{
		InitializeAudio();
	}

	private void OnMasterValueChanged(double value)
	{
		SetVolumeToBus(_masterBusIndex, (float)value, _masterSlider);
	}

	private void OnMusicValueChanged(double value)
	{
		SetVolumeToBus(_musicBusIndex, (float)value, _musicSlider);
	}

	private void OnEffectsValueChanged(double value)
	{
		SetVolumeToBus(_effectsBusIndex, (float)value, _effectsSlider);
	}

	private void OnReturnButtonPressed()
	{
		GetTree().ChangeSceneToFile(_mainMenuScenePath);
	}

	private void SetVolumeToBus(int busIndex, float linearVolume, Slider slider)
	{
		float dbVolume = Mathf.LinearToDb(linearVolume);

		AudioServer.SetBusVolumeDb(busIndex, dbVolume);
		SaveData.Instance.UpdateAudioSlider(busIndex, linearVolume);
	}

	private void InitializeAudio()
	{
		_masterBusIndex = AudioServer.GetBusIndex(SettingsConfig.MASTER_BUS);
		_musicBusIndex = AudioServer.GetBusIndex(SettingsConfig.MUSIC_BUS);
		_effectsBusIndex = AudioServer.GetBusIndex(SettingsConfig.EFFECTS_BUS);
		_masterSlider.Value = SaveData.Instance.MasterVolume;
		_musicSlider.Value = SaveData.Instance.MusicVolume;
		_effectsSlider.Value = SaveData.Instance.EffectsVolume;

		AudioServer.SetBusVolumeDb(_masterBusIndex, Mathf.LinearToDb(SaveData.Instance.MasterVolume));
		AudioServer.SetBusVolumeDb(_musicBusIndex, Mathf.LinearToDb(SaveData.Instance.MusicVolume));
		AudioServer.SetBusVolumeDb(_effectsBusIndex, Mathf.LinearToDb(SaveData.Instance.EffectsVolume));
	}
}
