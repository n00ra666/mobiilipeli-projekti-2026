using Godot;
using System;

public partial class Obstacle : Area2D
{
    [Export] private AudioStreamPlayer2D _scoreAudio;
    [Export] private AudioStreamPlayer2D _bubbleAudio;
    [Export] private Sprite2D _sprite;
    private int _value = 2500;
    private void OnArea2DBodyEntered(Node body)
    {
        if (body is CharacterController characterController)
        {
            if (characterController.IsDashing)
            {
                ScoreManager.Instance.AddScore(_value);
                Clear();
            }
            else if (characterController.HasBubble)
            {
                _bubbleAudio.Play();
                characterController.HasBubble = false;
                GameManager.Instance.BubbleUsed = true;
            }
            else
            {
                GameManager.Instance.SubtractLife();
                //body.QueueFree();
            }
        }
    }

    private void Clear()
    {
        _sprite.Visible = false;
        _scoreAudio.Finished += OnAudioClipFinished;
        _scoreAudio.Play();
    }

    private void OnAudioClipFinished()
    {
        if (_scoreAudio != null)
        {
            _scoreAudio.Finished -= OnAudioClipFinished;
        }
        QueueFree();
    }
}
