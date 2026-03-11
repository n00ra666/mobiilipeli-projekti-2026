using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node
{
	#region Singleton
	
	public static GameManager Instance
	{
		get;
		private set;
	}

	public GameManager()
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
	private bool _isScrolling = true;
	private float _currentSpeed = -400f;
	private float _baseSpeed = -400f;
	private float _dashMultiplier = 1.5f;
	private int _lives = 2;
	private bool _isGameOver = false;

	public bool IsScrolling
	{
		get => _isScrolling;
		set
		{
			_isScrolling = value;
		}
	}

	public float CurrentSpeed
	{
		get => _currentSpeed;
		set
		{
			_currentSpeed = value;
		}
	}
	public int Lives
	{
		get => _lives;
	}

	public bool IsGameOver
	{
		get => _isGameOver;
	}

	public async Task SubtractLife()
	{
		_lives--;
		_isScrolling = false;
		if (_lives > 0)
		{
			await RestartRound();
		}
		else if (_lives == 0)
		{
			GameOver();
		}
	}

	private async Task RestartRound()
	{
		await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
		GetTree().ReloadCurrentScene();
		_isScrolling = true;
	}

	private void GameOver()
	{
		_isGameOver = true;
	}

	public void RestartGame()
	{
		_lives = 2;
		_isScrolling = true;
		_isGameOver = false;
		GetTree().ReloadCurrentScene();
	}

	public void OnDashStarted()
	{
		_currentSpeed = _baseSpeed * _dashMultiplier;
	}

	public void OnDashEnded()
	{
		_currentSpeed = _baseSpeed;
	}
}
