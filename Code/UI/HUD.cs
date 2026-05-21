using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Export] private Label _livesLabel, _scoreLabel, _highScoreLabel, _moneyLabel;
	[Export] private PanelContainer _gameOverPanel;
	[Export] private Button _restartButton, _mainMenuButton, _jumpButton, _dashButton;
    private bool _gameOver = false;

    private string _mainMenuPath = "res://Scenes/mainmenu.tscn";

    public override void _Ready()
    {
        _restartButton.Pressed += OnRestartPressed;
        _mainMenuButton.Pressed += OnMainMenuPressed;
        _jumpButton.ButtonDown += OnJumpDown;
        _jumpButton.ButtonUp += OnJumpUp;
        _dashButton.ButtonDown += OnDashDown;
        _dashButton.ButtonUp += OnDashUp;

        FadeOutButtons();
    }

    public override void _Process(double delta)
    {
        _livesLabel.Text = $"LIVES: {GameManager.Instance.Lives}";
        _scoreLabel.Text = $"SCORE: {ScoreManager.Instance.Score}";

		if (GameManager.Instance.IsGameOver && _gameOver == false)
		{
            _gameOver = true;
            GameOver();
		}
    }

    private void GameOver()
    {
        var newHighScore = SaveData.Instance.CheckHighScore(ScoreManager.Instance.Score);
        if (newHighScore)
        {
            _highScoreLabel.Text = "New High Score: " + SaveData.Instance.HighScore;
        }
        else
        {
            _highScoreLabel.Text = "Score: " + ScoreManager.Instance.Score;
        }
        var moneyEarned = ScoreManager.Instance.CalculateMoneyEarned();
        _moneyLabel.Text = "Money earned: " + moneyEarned;
        _gameOverPanel.Visible = true;
    }

	private void OnRestartPressed()
	{
		_gameOverPanel.Visible = false;
        ScoreManager.Instance.Reset();
		GameManager.Instance.RestartGame();
	}

    private void OnMainMenuPressed()
    {
        _gameOverPanel.Visible = false;
        ScoreManager.Instance.Reset();
        GetTree().ChangeSceneToFile(_mainMenuPath);
    }

    private void OnJumpDown()
    {
        var action = new InputEventAction
        {
            Action = InputConfig.InputJump,
            Pressed = true
        };

        Input.ParseInputEvent(action);
    }

    private void OnJumpUp()
    {
        var action = new InputEventAction
        {
            Action = InputConfig.InputJump,
            Pressed = false
        };

        Input.ParseInputEvent(action);
    }

    private void OnDashDown()
    {
        var action = new InputEventAction
        {
            Action = InputConfig.InputDash,
            Pressed = true
        };

        Input.ParseInputEvent(action);
    }

    private void OnDashUp()
    {
        var action = new InputEventAction
        {
            Action = InputConfig.InputDash,
            Pressed = false
        };

        Input.ParseInputEvent(action);
    }

    private void FadeOutButtons()
    {
        _jumpButton.Modulate = new Color(1, 1, 1, 1);
        _dashButton.Modulate = new Color(1, 1, 1, 1);
        Tween tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(_jumpButton, "modulate:a", 0.0f, 2.5f);
        tween.TweenProperty(_dashButton, "modulate:a", 0.0f, 2.5f);
    }

    public override void _ExitTree()
    {
        _restartButton.Pressed -= OnRestartPressed;
    }


}
