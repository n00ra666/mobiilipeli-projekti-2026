using Godot;
using System;

public partial class Shop : Control
{
	[Export] private Button _returnButton, _jumpUpgradeButton, _extraLifeButton, _bubbleButton;
	[Export] int _jumpUpgradeCost, _extraLifeCost, _bubbleCost = 0;
	private string _mainMenuPath = "res://Scenes/mainmenu.tscn";

    public override void _Ready()
	{
		_returnButton.Pressed += OnReturnPressed;
		_jumpUpgradeButton.Pressed += OnJumpUpgradePressed;
		_extraLifeButton.Pressed += OnExtraLifePressed;
		_bubbleButton.Pressed += OnBubblePressed;

		UpdateButtons();
	}

	private void OnReturnPressed()
	{
		GetTree().ChangeSceneToFile(_mainMenuPath);
	}

	private void OnJumpUpgradePressed()
	{
		SaveData.Instance.DecreaseMoney(_jumpUpgradeCost);
		SaveData.Instance.UpgradeJumps();
		UpdateButtons();
	}

	private void OnExtraLifePressed()
	{
		SaveData.Instance.DecreaseMoney(_extraLifeCost);
		SaveData.Instance.UpgradeLives();
		UpdateButtons();
	}

	private void OnBubblePressed()
	{
		SaveData.Instance.DecreaseMoney(_bubbleCost);
		SaveData.Instance.UpgradeBubble();
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		if (SaveData.Instance.JumpUpgradeBought || SaveData.Instance.Money < _jumpUpgradeCost)
		{
			_jumpUpgradeButton.Disabled = true;
		}
		if (SaveData.Instance.ExtraLifeBought || SaveData.Instance.Money < _extraLifeCost)
		{
			_extraLifeButton.Disabled = true;
		}
		if (SaveData.Instance.BubbleBought || SaveData.Instance.Money < _bubbleCost)
		{
			_bubbleButton.Disabled = true;
		}
	}



}
