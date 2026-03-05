using Godot;
using System;
using System.Collections.Generic;

public partial class PlatformSpawner : Node2D
{
	[Export] private Godot.Collections.Array<PackedScene> Platforms;
	[Export] private Vector2 _spawnCoordinate = Vector2.Zero;
	[Export] private float _spawnCoordsYMin, _spawnCoordsYMax = 0f;
	[Export] private float _longPlatformWaitTime = 7f;
	[Export] private float _mediumPlatformWaitTime = 3f;
	[Export] private float _shortPlatformWaitTime = 1.5f;
	[Export] private Timer _timer;
	private RandomNumberGenerator _rng;
	private bool _isSpawning = true;
	private int _previousPlatformIndex;
	

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
		//_timer.WaitTime = _rng.RandfRange(2.0f, 4.0f);
    }

	public void OnTimerTimeout()
	{
		if (!_isSpawning)
		{
			return;
		}

		var randomPlatform = _rng.RandiRange(0, Platforms.Count - 1);
		_previousPlatformIndex = randomPlatform;
		var platform = (StaticBody2D)Platforms[randomPlatform].Instantiate();
		platform.GlobalPosition = _spawnCoordinate + Vector2.Up * _rng.RandfRange(_spawnCoordsYMin, _spawnCoordsYMax);
		GetTree().Root.GetNode("TestLevel").AddChild(platform);

		if (_previousPlatformIndex == 0)
		{
			_timer.WaitTime = _longPlatformWaitTime;
		}
		else if (_previousPlatformIndex == 1)
		{
			_timer.WaitTime = _mediumPlatformWaitTime;
		}
		else if (_previousPlatformIndex == 2)
		{
			_timer.WaitTime = _shortPlatformWaitTime;
		}
		//_timer.WaitTime = _rng.RandfRange(2.0f, 4.0f);
	}

	private void StopSpawning()
	{
		_isSpawning = false;
	}
}
