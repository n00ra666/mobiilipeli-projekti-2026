using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] private Button _startButton, _settingsButton, _shopButton;
	[Export] private Label _highScoreLabel, _moneyLabel;
	private string _gameScenePath = "res://Scenes/test_level.tscn";
	private string _shopScenePath = "res://Scenes/shop.tscn";
	private string _settingsScenePath = "res://Scenes/settings.tscn";

	public override void _Ready()
	{
		_startButton.Pressed += OnStartButtonPressed;
		_shopButton.Pressed += OnShopButtonPressed;
		_settingsButton.Pressed += OnSettingsButtonPressed;
		_highScoreLabel.Text = "High Score: " + SaveData.Instance.HighScore;
		_moneyLabel.Text = "Money: " + SaveData.Instance.Money;
	}

    public override void _ExitTree()
    {
        _startButton.Pressed -= OnStartButtonPressed;
		_shopButton.Pressed -= OnShopButtonPressed;
		_settingsButton.Pressed -= OnSettingsButtonPressed;
    }


	private void OnStartButtonPressed()
	{
		GameManager.Instance.StartNewGame();
		GetTree().ChangeSceneToFile(_gameScenePath);
	}

	private void OnShopButtonPressed()
	{
		GetTree().ChangeSceneToFile(_shopScenePath);
	}

	private void OnSettingsButtonPressed()
	{
		GetTree().ChangeSceneToFile(_settingsScenePath);
	}
}
