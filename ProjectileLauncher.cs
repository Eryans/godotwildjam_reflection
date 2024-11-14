using Godot;
using System;

public partial class ProjectileLauncher : Marker3D
{
	private PackedScene _projectile;
	public override void _Ready()
	{
		_projectile = GD.Load<PackedScene>("res://game/scene/Entities/fire_ball.tscn");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("l_mouse"))
		{
			CharacterBody3D instance = (CharacterBody3D)_projectile.Instantiate();
			instance.GlobalTransform = GlobalTransform;
			GetTree().Root.AddChild(instance);
			instance.GlobalBasis = GlobalBasis;
		}
	}
}
