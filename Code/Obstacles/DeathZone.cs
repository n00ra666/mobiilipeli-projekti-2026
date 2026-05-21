using Godot;
using System;

public partial class DeathZone : Node2D
{
	private Area2D _area;

	public override void _Ready()
	{
		_area = GetNode<Area2D>("Area2D");
	}

	private void OnArea2DBodyEntered(Node body)
	{
		if (body is CharacterController characterController)
		{
			GameManager.Instance.SubtractLife();
			//body.QueueFree();
		}
	}
}
