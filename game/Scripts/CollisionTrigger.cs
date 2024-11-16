using Godot;
using System;

public partial class CollisionTrigger : Area3D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is RigidBody3D rb)
		{
			GlobalSignals.Instance.EmitReflectBodyTrigger(this);
		}
	}
}
