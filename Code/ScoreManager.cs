using Godot;
using System;

public partial class ScoreManager : Node
{
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

	private bool _gameReset = false;
	private float _distance = 0;
	private int _score = 0;
	public int Score
	{
		get => _score;
	}

    public override void _Process(double delta)
    {
		if (GameManager.Instance.IsScrolling)
		{
			_distance -= GameManager.Instance.CurrentSpeed * (float)delta;
			_score = (int)_distance;
		}
    }

	public void AddScore(int amount)
	{
		if (amount > 0)
		{
			_score += amount;
		}
	}

	public void Reset()
	{
		_distance = 0;
		_score = 0;
	}

}
