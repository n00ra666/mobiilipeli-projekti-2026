using Godot;
using System;

public partial class Enemy : Area2D
{
    [Signal] public delegate void BubbleHitEnemyEventHandler();
    [Export] private AnimatedSprite2D _sprite;
    public override void _Process(double delta)
    {
        UpdateAnimations();
    }

    private void OnArea2DBodyEntered(Node body)
    {
        if (body is CharacterController characterController)
        {
            
            if (characterController.HasBubble)
            {
                EmitSignal(SignalName.BubbleHitEnemy);
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

    private void UpdateAnimations()
    {
        _sprite.Play("default");
    }
}
