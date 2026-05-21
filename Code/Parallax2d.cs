using Godot;
using System;

public partial class Parallax2d : Parallax2D
{
	[Export] private float _scrollingSpeedDecrease = 100;
	public override void _Process(double delta)
	{
		if (GameManager.Instance.IsScrolling)
		{
			Autoscroll = new Vector2(GameManager.Instance.CurrentSpeed + _scrollingSpeedDecrease, 0f);
		}
		else
		{
			Autoscroll = new Vector2(0, 0);
		}
	}
}
