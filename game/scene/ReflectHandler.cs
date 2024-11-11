using Godot;
using System;

public partial class ReflectHandler : Marker3D
{
	[Export]
	public CharacterBody3D Player { get; set; }
	public enum ReflectedAxesEnum
	{
		X,
		Z
	}
	[Export]
	public ReflectedAxesEnum ReflectedAxes { get; set; }
	private CharacterBody3D _reflection;
	private const float _speed = 5.0f;


	public override void _Ready()
	{
		if (Player != null)
		{
			_reflection = new CharacterBody3D();
			_reflection.CallDeferred("add_child", Player.GetNode<Node3D>("character").Duplicate());
			_reflection.CallDeferred("add_child", Player.GetNode<Node3D>("CollisionShape3D").Duplicate());
			SetupReflectTransform();
			GetTree().Root.CallDeferred("add_child", _reflection);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player != null)
		{
			HandleReflectVelocity();
		}
	}
	private void SetupReflectTransform()
	{
		var transform = Player.Transform;
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			transform.Origin.X = Transform.Origin.X - Player.Transform.Origin.X;
		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			transform.Origin.Z = Transform.Origin.Z - Player.Transform.Origin.Z;
		}
		_reflection.Transform = transform;
	}
	private void HandleReflectVelocity()
	{
		var playerVelocity = Player.Velocity;
		Vector3 inversedAxes = new(playerVelocity.X, playerVelocity.Y, playerVelocity.Z);
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			inversedAxes.X = -playerVelocity.X;

		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			inversedAxes.Z = -playerVelocity.Z;

		}
		_reflection.Velocity = inversedAxes;
		_reflection.MoveAndSlide();
	}

}
