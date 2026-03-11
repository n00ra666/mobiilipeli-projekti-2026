using Godot;
using System;

public partial class PlatformController : StaticBody2D
{
	[Export] private float _scrollSpeed = -50.0f;
	[Export] private float _offScreenEndCoordinate = -500f;

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

	public void OnArea2DAreaEntered(Area2D area)
	{
		GD.Print("Area entered");
	}

}
