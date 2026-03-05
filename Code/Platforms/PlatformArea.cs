using Godot;
using System;

public partial class PlatformArea : Area2D
{
    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("Area entered");
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach(var body in GetOverlappingBodies())
        {
            GD.Print("Currently overlapping: " + body.Name);
        }
    }
}
