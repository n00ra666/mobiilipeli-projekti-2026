using Godot;
using System;
using System.Collections.Generic;

public partial class PlatformSpawner : Node2D
{
	[Export] private Godot.Collections.Array<PackedScene> Platforms;
	[Export] private Vector2 _spawnCoordinate = Vector2.Zero;
	[Export] private float _spawnCoordsYMin, _spawnCoordsYMax = 0f;
	[Export] private float _minGap, _maxGap;
	[Export] private int _whichSpawner;
	private RandomNumberGenerator _rng;
	private ObstacleSpawner _obstacleSpawner;
	private int _previousPlatformIndex;
	private float _worldEndX;
	

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
		var startPlatform = GetNode<Node2D>("../StartPlatform");
		float width = GetPlatformWidth(startPlatform);

		_worldEndX = startPlatform.GlobalPosition.X + width / 2;
    }

    public override void _Process(double delta)
    {
		float speed = GameManager.Instance.CurrentSpeed;

		_worldEndX += (float)(delta * speed);
		if (ShouldSpawnNext() && GameManager.Instance.IsScrolling)
		{
			SpawnNextPlatform();
		}
    }

	private bool ShouldSpawnNext()
	{
		// Get the coordinate of the right side of the viewport
		float screenRight = GetViewportRect().Size.X;
		// Define a buffer range
		float spawnBuffer = 300f;
		// Wait for the condition to become true before spawning next platform
		return _worldEndX < screenRight + spawnBuffer;
	}

	private float GetPlatformWidth(Node2D platform)
	{
		return ((PlatformController)platform).Width;
	}

	private float GetNextGap()
	{
		return (float)GD.RandRange(_minGap, _maxGap);
	}

	private void SpawnNextPlatform()
	{
		var randomPlatform = _rng.RandiRange(0, Platforms.Count - 1);
		var platform = (PlatformController)Platforms[randomPlatform].Instantiate();

		float width = GetPlatformWidth(platform);
		float gap = GetNextGap();

		float spawnX = _worldEndX + gap + width / 2;
		float spawnY = _rng.RandfRange(_spawnCoordsYMin, _spawnCoordsYMax);
		platform.GlobalPosition = new Vector2(spawnX, spawnY);
		platform.WhichSpawner = _whichSpawner;

		GetTree().Root.GetNode("TestLevel").AddChild(platform);

		_obstacleSpawner = platform.GetNode<ObstacleSpawner>("ObstacleSpawner");

		_worldEndX = spawnX + width / 2;
	}
}
