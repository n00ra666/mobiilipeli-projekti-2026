using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export] private Area2D _enemy;
	[Export] private Area2D _scoreArea;
	public void SpawnEnemy()
	{
		_enemy.Visible = true;
		_enemy.Monitoring = true;
		_scoreArea.Visible = true;
		_scoreArea.Monitoring = true;
	}
}
