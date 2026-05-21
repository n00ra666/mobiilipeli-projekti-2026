using Godot;
using System;

public partial class PathFollower : PathFollow2D
{
	[Export] private float _speed = 0;
	private AnimatedSprite2D _sprite;
	private float _pathLength = -1;
	private float _moveSpeed = 0;
	private RandomNumberGenerator _rng;

	public int Direction
	{
		get;
		private set;
	} = 1;

	[Export]
	public bool CanMove
	{
		get;
		private set;
	} = true;

    public override void _Ready()
    {
		_sprite = GetNode<AnimatedSprite2D>("EnemyPlaceholder/AnimatedSprite2D");
		_rng = new RandomNumberGenerator();
		float randomProgress = _rng.RandfRange(0, 1);
        ProgressRatio = randomProgress;
    }


    public override void _Process(double delta)
	{
		if (!CanMove || !ValidatePath())
		{
			return;
		}

		float deltaRatio = Direction * _moveSpeed * (float)delta;
		ProgressRatio = Mathf.Clamp(ProgressRatio + deltaRatio, 0, 1);
		if (ProgressRatio >= 1 || ProgressRatio <= 0)
		{
			Direction *= -1;
		}

		_sprite.FlipH = Direction > 0;
	}

	private bool ValidatePath()
	{
		if (_pathLength >= 0)
		{
			return true;
		}

		Path2D path = GetParent<Path2D>();
		if (path == null || path.Curve == null)
		{
			return false;
		}

		_pathLength = path.Curve.GetBakedLength();
		_moveSpeed = _speed / _pathLength;


		return true;
	}

}
