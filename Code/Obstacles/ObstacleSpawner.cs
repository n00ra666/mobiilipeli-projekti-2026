using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class ObstacleSpawner : Node2D
{
	private RandomNumberGenerator _rng;
	private bool _previousPlatformSpawnedObstacle = false;
	public override void _Ready()
    {
		_rng = new RandomNumberGenerator();
		if (!_previousPlatformSpawnedObstacle)
		{
			RandomizeObstacleSpawn();
		}
    }

	private void RandomizeObstacleSpawn()
	{
		int spawnObstacle = _rng.RandiRange(0, 5);
		if (spawnObstacle <= 1)
		{
			SpawnObstacle();
			_previousPlatformSpawnedObstacle = true;
		}
	}

	private void SpawnObstacle()
	{
		Visible = true;

	}

	private void OnArea2DBodyEntered(Node body)
	{
		if (body is CharacterController)
		{
			GD.Print("Player detected");
			body.QueueFree();
		}
	}

}
