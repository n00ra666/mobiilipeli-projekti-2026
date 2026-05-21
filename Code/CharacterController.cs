using Godot;
using System;

public partial class CharacterController : CharacterBody2D
{
	[Signal] public delegate void DashStartedEventHandler();
	[Signal] public delegate void DashEndedEventHandler();
	[Export] private Timer _dashTimer;
	[Export] private AnimatedSprite2D _sprite;
	[Export] private Sprite2D _bubbleSprite;
	[Export] private AudioStreamPlayer2D _dashAudio;
	[Export] private AudioStreamPlayer2D _deathAudio;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -850.0f;
	private float _horizontalMovement = 0;
	private bool _isJumping = false;
	private bool _isDashing = false;
	private bool _timerStarted = false;
	private int _extraJumpsLeft = 1;
	private bool _jumpUpgraded = false;
	private bool _hasBubble = false;
	private bool _playerDied = false;

	public bool IsDashing
	{
		get => _isDashing;
	}

	public bool HasBubble
	{
		get => _hasBubble;
		set
		{
			_hasBubble = value;
		}
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
		GameManager.Instance.PlayerDied += OnPlayerDeath;
        DashStarted += GameManager.Instance.OnDashStarted;
		DashEnded += GameManager.Instance.OnDashEnded;
		if (SaveData.Instance.JumpUpgradeBought == true)
		{
			_jumpUpgraded = true;
			_extraJumpsLeft = 2;
		}
		if (SaveData.Instance.BubbleBought)
		{
			_hasBubble = true;
			if (GameManager.Instance.BubbleUsed)
			{
				_hasBubble = false;
			}
		}

		if (_hasBubble)
		{
			_bubbleSprite.Visible = true;
		}
    }


    public override void _Process(double delta)
    {
        UpdateAnimations();
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
			_isJumping = false;
			velocity.Y = JumpVelocity;
			_extraJumpsLeft--;
		}

		if (_isDashing)
		{
			if (!_timerStarted && !_dashAudio.Playing)
			{
				_timerStarted = true;
				_dashAudio.Play();
				EmitSignal(SignalName.DashStarted);
				_dashTimer.Start();
			}
		}

		if (IsOnFloor())
		{
			if (_jumpUpgraded)
			{
				_extraJumpsLeft = 2;
			}
			else
			{
				_extraJumpsLeft = 1;
			}
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

	private void UpdateAnimations()
	{
		if (_sprite != null)
		{
			if (_playerDied)
			{
				return;
			}
			if (_isDashing)
			{
				_sprite.Play("dash");
			}
			else
			{
				_sprite.Play("run");
			}
		}
		if (!_hasBubble)
		{
			_bubbleSprite.Visible = false;
		}
	}

	private void OnTimerTimeout()
	{
		_dashTimer.Stop();
		EmitSignal(SignalName.DashEnded);
		_isDashing = false;
		_timerStarted = false;
	}

	private void OnPlayerDeath()
	{
		if (_playerDied)
		{
			return;
		}
		
		_deathAudio.Play();
		_playerDied = true;
		if (_sprite != null)
		{
			_sprite.Play("death");
			_sprite.AnimationFinished += OnDeathAnimationFinished;
		}
	}

	private void OnDeathAnimationFinished()
	{
		if (_sprite != null)
		{
			_sprite.AnimationFinished -= OnDeathAnimationFinished;
		}
		QueueFree();
	}


}
