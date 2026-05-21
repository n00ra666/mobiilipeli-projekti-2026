using Godot;
using System;

public partial class ScorePopup : Label
{
	private Tween _tween;

	public void ShowPopup(int score)
	{
		if (_tween != null)
		{
			_tween.Kill();
		}

		Text = $"+{score}";
		Visible = true;
		Position = new Vector2(-27, -90);
		Modulate = new Color(1, 1, 1, 1);

		_tween = CreateTween();

		_tween.TweenProperty(this, "position", new Vector2(-27, -110), 1f);

		_tween.Parallel().TweenProperty(this, "modulate:a", 0f, 1f);

		_tween.Finished += () =>
		{
			Visible = false;
		};
	}
}
