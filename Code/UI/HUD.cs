using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Export] private Label _livesLabel;
	[Export] private PanelContainer _gameOverPanel;
	[Export] private Button _restartButton;

    public override void _EnterTree()
    {
        _restartButton.Pressed += OnRestartPressed;
    }

    public override void _Process(double delta)
    {
        _livesLabel.Text = $"LIVES: {GameManager.Instance.Lives.ToString()}";

		if (GameManager.Instance.IsGameOver)
		{
			_gameOverPanel.Visible = true;
		}
    }

	private void OnRestartPressed()
	{
		_gameOverPanel.Visible = false;
		GameManager.Instance.RestartGame();
	}

    public override void _ExitTree()
    {
        _restartButton.Pressed -= OnRestartPressed;
    }


}
