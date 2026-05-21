using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class ObstacleSpawner : Node2D
{
	[Export] private PlatformController _platform;
	private RandomNumberGenerator _rng;
	private Area2D _area;
	private EnemySpawner _enemySpawner;
	private int _whichSpawner;
	private bool _topObstacleSpawn = GameManager.Instance.TopObstacleSpawned;
	private bool _topEnemySpawn = GameManager.Instance.TopEnemySpawned;
	private bool _middleObstacleSpawn = GameManager.Instance.MiddleObstacleSpawned;
	private bool _middleEnemySpawn = GameManager.Instance.MiddleEnemySpawned;
	public override void _Ready()
    {
		_area = GetNode<Area2D>("Area2D");
		_rng = new RandomNumberGenerator();

		_whichSpawner = _platform.WhichSpawner;

		if (_whichSpawner == 0)
		{
			HandleSpawnLogic(_topObstacleSpawn, _topEnemySpawn, _whichSpawner);

		}
		else if (_whichSpawner == 1)
		{
			HandleSpawnLogic(_middleObstacleSpawn, _middleEnemySpawn, _whichSpawner);
		}
    }

	private void HandleSpawnLogic(bool obstacleSpawn, bool enemySpawn, int whichSpawner)
	{
		// If the previous platform spawned an obstacle or an enemy, leave at least one empty platform in between
		if (!obstacleSpawn && !enemySpawn)
		{
			// Try to get EnemySpawner; it does not exist on short platforms, and if that is the case, we can only spawn an obstacle.
			_enemySpawner = GetNodeOrNull<EnemySpawner>("../EnemySpawner");
			if (_enemySpawner == null)
			{
				RandomizeObstacleSpawn(whichSpawner);
			}
			else
			{
				RandomizeObstacleOrEnemy(whichSpawner);
			}
		}
		else
		{
			UpdateGameManager(whichSpawner);
		}
	}

	private void UpdateGameManager(int whichSpawner)
	{
		if (whichSpawner == 0)
		{
			GameManager.Instance.TopObstacleSpawned = false;
			GameManager.Instance.TopEnemySpawned = false;
		}
		else if (whichSpawner == 1)
		{
			GameManager.Instance.MiddleObstacleSpawned = false;
			GameManager.Instance.MiddleEnemySpawned = false;
		}
	}

	private void RandomizeObstacleSpawn(int whichSpawner)
	{
		int spawnObstacle = _rng.RandiRange(0, 5);
		if (spawnObstacle <= 3)
		{
			SpawnObstacle();
			if (whichSpawner == 0)
			{
				GameManager.Instance.TopObstacleSpawned = true;
			}
			else if (whichSpawner == 1)
			{
				GameManager.Instance.MiddleObstacleSpawned = true;
			}
		}
	}

	private void RandomizeObstacleOrEnemy(int whichSpawner)
	{
		int spawnEnemy = _rng.RandiRange(0, 1);
		if (spawnEnemy == 0)
		{
			RandomizeEnemySpawn(whichSpawner);
		}
		else
		{
			RandomizeObstacleSpawn(whichSpawner);
		}
	}

	private void RandomizeEnemySpawn(int whichSpawner)
	{
		int spawnEnemy = _rng.RandiRange(0, 5);
		if (spawnEnemy <= 3)
		{
			_enemySpawner.SpawnEnemy();
			if (whichSpawner == 0)
			{
				GameManager.Instance.TopEnemySpawned = true;
			}
			else if (whichSpawner == 1)
			{
				GameManager.Instance.MiddleEnemySpawned = true;
			}
			
		}
	}

	private void SpawnObstacle()
	{
		_area.Visible = true;
		_area.Monitoring = true;
	}
}
