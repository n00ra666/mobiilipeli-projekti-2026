using Godot;
using System;

public partial class PlatformController : Node2D
{
	[Export] private float _scrollSpeed = -50.0f;
	[Export] private float _offScreenEndCoordinate = -500f;

    public override void _Process(double delta)
	{
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
