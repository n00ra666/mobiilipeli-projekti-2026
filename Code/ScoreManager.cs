using Godot;
using System;

public partial class ScoreManager : Node
{
	#region Singleton
	public static ScoreManager Instance
	{
		get;
		private set;
	}

	public ScoreManager()
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

	private ScorePopup _scorePopup;
	private bool _gameReset = false;
	private float _distance = 0;
	private int _bonusScore = 0;
	private int _moneyEarned = 0;
	public int Score => (int)_distance + _bonusScore;

    public override void _EnterTree()
    {
        GameManager.Instance.PlayerDied += OnPlayerDeath;
    }

	public override void _ExitTree()
    {
        GameManager.Instance.PlayerDied -= OnPlayerDeath;
    }

    public override void _Process(double delta)
    {
		if (GameManager.Instance.IsScrolling)
		{
			_distance -= GameManager.Instance.CurrentSpeed * (float)delta;
		}
    }

	public void AddScore(int amount)
	{
		if (amount > 0)
		{
			_bonusScore += amount;
			if (_scorePopup == null)
			{
				_scorePopup = GetNode<ScorePopup>("../TestLevel/Player/ScorePopup");
			}
			_scorePopup.ShowPopup(amount);
		}
	}

	public void Reset()
	{
		_distance = 0;
		_bonusScore = 0;
	}

	public int CalculateMoneyEarned()
	{
		_moneyEarned = (int)(Score / 1000);
		SaveData.Instance.AddMoney(_moneyEarned);
		return _moneyEarned;
	}

	private void OnPlayerDeath()
	{
		_scorePopup = null;
	}

}
