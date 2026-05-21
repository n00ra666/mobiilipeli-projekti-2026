using Godot;
using System;

public partial class FollowCamera : Camera2D
{
	[Export] Node2D _target = null;
	private AnimatedSprite2D _playerSprite;

	private float _speed = 10.0f;

    public override void _Ready()
    {
        GameManager.Instance.PlayerDied += OnPlayerDeath;
    }

    public override void _Process(double delta)
    {
		if (_target == null)
		{
			return;
		}
        else if (ProcessCallback == Camera2DProcessCallback.Idle)
		{
			UpdatePosition((float)delta);
		}
    }

/*     public override void _PhysicsProcess(double delta)
	{
		if (_target == null)
		{
			return;
		}
		else if (ProcessCallback == Camera2DProcessCallback.Physics)
		{
			UpdatePosition((float)delta);
		}
	} */

	private void UpdatePosition(float deltaTime)
	{
		if (_target == null)
		{
			return;
		}
		Vector2 targetPosition = _target.GlobalPosition;
		Vector2 currentPosition = GlobalPosition;
		Vector2 newPosition = currentPosition.Lerp(targetPosition, _speed * deltaTime);
		GlobalPosition = newPosition;
	}

	private void OnPlayerDeath()
	{
		_target = null;
	}
}
