using Godot;
using System;

public partial class EnemyScore : Area2D
{
    [Export] private Enemy _enemy;
    [Export] private AudioStreamPlayer2D _scoreAudio;
    [Export] private AudioStreamPlayer2D _bubbleAudio;
    private int _value = 5000;
    private bool _addScore = true;

    public override void _EnterTree()
    {
        _enemy.BubbleHitEnemy += OnBubbleHitEnemy;
    }

    public override void _ExitTree()
    {
        _enemy.BubbleHitEnemy -= OnBubbleHitEnemy;
    }


    private void OnBubbleHitEnemy()
    {
        _addScore = false;
        _bubbleAudio.Play();
    }

    private void OnArea2DBodyEntered(Node body)
    {
        if (body is CharacterController characterController)
        {
            if (_addScore && GameManager.Instance.IsScrolling)
            {
                ScoreManager.Instance.AddScore(_value);
                _scoreAudio.Play();
            }
        }
    }
}
