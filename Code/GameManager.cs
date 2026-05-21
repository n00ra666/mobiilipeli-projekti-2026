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
	
	[Signal] public delegate void PlayerDiedEventHandler();
	[Export] public Curve DifficultyCurve;
	private bool _isScrolling = true;
	private float _currentSpeed = -400f;
	private float _baseSpeed = -400f;
	private float _minSpeed = -400f;
	private float _maxSpeed = -1000f;
	private float _elapsedTime;
	private float _maxDifficultyTime = 120f;
	private float _curveValue;
	private float _speedProgress;
	private bool _isDashing = false;
	private float _dashMultiplier = 1.5f;
	private int _lives = 2;
	public bool _extraLifeBought = false;
	private bool _isGameOver = false;
	private bool _topObstacleSpawned = false;
	private bool _middleObstacleSpawned = false;
	private bool _topEnemySpawned = false;
	private bool _middleEnemySpawned = false;
	private bool _bubbleUsed = false;
	private bool _isInvulnerable = false;

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

	public bool TopObstacleSpawned
	{
		get => _topObstacleSpawned;
		set
		{
			_topObstacleSpawned = value;
		}
	}

	public bool MiddleObstacleSpawned
	{
		get => _middleObstacleSpawned;
		set
		{
			_middleObstacleSpawned = value;
		}
	}
	public bool TopEnemySpawned
	{
		get => _topEnemySpawned;
		set
		{
			_topEnemySpawned = value;
		}
	}

	public bool MiddleEnemySpawned
	{
		get => _middleEnemySpawned;
		set
		{
			_middleEnemySpawned = value;
		}
	}
	public bool BubbleUsed
	{
		get => _bubbleUsed;
		set
		{
			_bubbleUsed = value;
		}
	}

    public override void _Ready()
    {
		if (SaveData.Instance.ExtraLifeBought)
		{
			_lives = 3;
		}
    }


	public async Task SubtractLife()
	{
		if (!_isInvulnerable)
		{
			_lives--;
			_isScrolling = false;
			_isInvulnerable = true;
			EmitSignal(SignalName.PlayerDied);
			if (_lives > 0)
			{
				await RestartRound();
			}
			else if (_lives == 0)
			{
				GameOver();
			}
		}
	}

	private async Task RestartRound()
	{
		await ToSignal(GetTree().CreateTimer(2.5f), SceneTreeTimer.SignalName.Timeout);
		GetTree().ReloadCurrentScene();
		_isScrolling = true;
		_isDashing = false;
		_elapsedTime = 0;
		_currentSpeed = _baseSpeed;
		_isInvulnerable = false;
	}

	private void GameOver()
	{
		_isGameOver = true;
	}

	public void StartNewGame()
	{
		if (SaveData.Instance.ExtraLifeBought)
		{
			_lives = 3;
		}
		else
		{
			_lives = 2;
		}
		_isScrolling = true;
		_isGameOver = false;
		_isDashing = false;
		_elapsedTime = 0;
		_bubbleUsed = false;
		_currentSpeed = _baseSpeed;
		_isInvulnerable = false;
	}

	public void RestartGame()
	{
		if (SaveData.Instance.ExtraLifeBought)
		{
			_lives = 3;
		}
		else
		{
			_lives = 2;
		}
		_isScrolling = true;
		_isGameOver = false;
		_isDashing = false;
		_elapsedTime = 0;
		_bubbleUsed = false;
		_currentSpeed = _baseSpeed;
		_isInvulnerable = false;
		GetTree().ReloadCurrentScene();
	}

	public void OnDashStarted()
	{
		_isDashing = true;
	}

	public void OnDashEnded()
	{
		_isDashing = false;
	}

    public override void _Process(double delta)
    {
        _elapsedTime += (float)delta;
		_speedProgress = Mathf.Clamp(_elapsedTime / _maxDifficultyTime, 0, 1);
		_curveValue = DifficultyCurve.Sample(_speedProgress);
		CurrentSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, _curveValue);
		if (_isDashing)
		{
			CurrentSpeed = CurrentSpeed * _dashMultiplier;
		}
    }
}
