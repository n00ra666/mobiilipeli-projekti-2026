using Godot;
using System;

public partial class PlatformController : StaticBody2D
{
	[Export] private float _offScreenEndCoordinate = -500f;
	[Export] public float Width;
	private int _whichSpawner;
	private float _scrollSpeed;

	public int WhichSpawner
	{
		get => _whichSpawner;
		set
		{
			_whichSpawner = value;
		}
	}

    public override void _Process(double delta)
	{
		if (GameManager.Instance.IsScrolling == false)
		{
			StopMovement();
			return;
		}
		
		_scrollSpeed = GameManager.Instance.CurrentSpeed;
		var position = Position;
		position.X += (float)delta * _scrollSpeed;

		if (position.X < _offScreenEndCoordinate)
		{
			QueueFree();
		}

		Position = position;
	}

	private void StopMovement()
	{
		_scrollSpeed = 0f;
	}
}
