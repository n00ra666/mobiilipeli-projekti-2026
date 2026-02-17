using Godot;
using System;

public partial class PlatformSpawner : Node2D
{
	[Export] private PackedScene _platformScene;
	[Export] private Vector2 _spawnCoordinate = Vector2.Zero;
	[Export] private float _spawnCoordsYMin, _spawnCoordsYMax = 0f;
	private RandomNumberGenerator _rng;
	private bool _isSpawning = true;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
    }

	public void OnTimerTimeout()
	{
		GD.Print("Timeout");
		if (!_isSpawning)
		{
			return;
		}

		var platform = (Node2D)_platformScene.Instantiate();
		platform.GlobalPosition = _spawnCoordinate + Vector2.Up * _rng.RandfRange(_spawnCoordsYMin, _spawnCoordsYMax);
		GetTree().Root.GetNode("TestLevel").AddChild(platform);
	}

	private void StopSpawning()
	{
		_isSpawning = false;
	}
}
