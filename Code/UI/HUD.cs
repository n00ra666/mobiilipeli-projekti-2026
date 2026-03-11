using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Export] private Label _livesLabel;
    [Export] private Label _scoreLabel;
	[Export] private PanelContainer _gameOverPanel;
	[Export] private Button _restartButton;

    public override void _EnterTree()
    {
        _restartButton.Pressed += OnRestartPressed;
    }

    public override void _Process(double delta)
    {
        _livesLabel.Text = $"LIVES: {GameManager.Instance.Lives.ToString()}";
        _scoreLabel.Text = $"SCORE: {ScoreManager.Instance.Score.ToString()}";

		if (GameManager.Instance.IsGameOver)
		{
			_gameOverPanel.Visible = true;
		}
    }

	private void OnRestartPressed()
	{
		_gameOverPanel.Visible = false;
        ScoreManager.Instance.Reset();
		GameManager.Instance.RestartGame();
	}

    public override void _ExitTree()
    {
        _restartButton.Pressed -= OnRestartPressed;
    }


}
