using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class ObstacleSpawner : Node2D
{
	private RandomNumberGenerator _rng;
	private Area2D _area;
	private bool _previousPlatformSpawnedObstacle = false;
	public override void _Ready()
    {
		_area = GetNode<Area2D>("Area2D");
		_rng = new RandomNumberGenerator();
		if (!_previousPlatformSpawnedObstacle)
		{
			RandomizeObstacleSpawn();
		}
    }

	private void RandomizeObstacleSpawn()
	{
		int spawnObstacle = _rng.RandiRange(0, 5);
		if (spawnObstacle <= 3)
		{
			SpawnObstacle();
			_previousPlatformSpawnedObstacle = true;
		}
	}

	private void SpawnObstacle()
	{
		_area.Visible = true;
		_area.Monitoring = true;
	}

	private void OnArea2DBodyEntered(Node body)
	{
		if (body is CharacterController)
		{
			GameManager.Instance.SubtractLife();
			body.QueueFree();
		}
	}

}
