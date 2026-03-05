using Godot;

public partial class PlatformSpeedManager : Node
{
	private float _currentSpeed = -400f;
	public float CurrentSpeed
	{
		get => _currentSpeed;
		set
		{
			_currentSpeed = value;
		}
	}
}
