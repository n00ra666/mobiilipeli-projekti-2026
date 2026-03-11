using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
	[Signal] public delegate void DashStartedEventHandler();
	[Signal] public delegate void DashEndedEventHandler();
	[Export] private Timer _dashTimer;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -850.0f;
	private float _horizontalMovement = 0;
	private bool _isJumping = false;
	private bool _isDashing = false;
	private bool _timerStarted = false;
	private int _extraJumpsLeft = 1;

	public bool IsDashing
	{
		get => _isDashing;
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputConfig.InputJump) && _extraJumpsLeft > 0)
		{
			_isJumping = true;
		}

		if (@event.IsActionPressed(InputConfig.InputDash) && _isDashing == false)
		{
			_isDashing = true;
		}
	}

    public override void _Ready()
    {
        DashStarted += GameManager.Instance.OnDashStarted;
		DashEnded += GameManager.Instance.OnDashEnded;
    }


    public override void _Process(double delta)
    {
        //_horizontalMovement = Input.GetAxis(InputConfig.InputLeft, InputConfig.InputRight);
    }


	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (_isJumping)
		{
			velocity.Y = JumpVelocity;
			_extraJumpsLeft--;
			if (_extraJumpsLeft < 1)
			{
				_isJumping = false;
			}
		}

		if (_isDashing)
		{
			if (!_timerStarted)
			{
				_timerStarted = true;
				EmitSignal(SignalName.DashStarted);
				_dashTimer.Start();
			}
		}

		if (IsOnFloor())
		{
			_extraJumpsLeft = 1;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		if (!Mathf.IsZeroApprox(_horizontalMovement))
		{
			velocity.X = _horizontalMovement * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnTimerTimeout()
	{
		_dashTimer.Stop();
		EmitSignal(SignalName.DashEnded);
		_isDashing = false;
		_timerStarted = false;
	}
}
