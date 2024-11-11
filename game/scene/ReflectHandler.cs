using Godot;
using System;

public partial class ReflectHandler : Marker3D
{
	[Export]
	public Player Player { get; set; }
	public enum ReflectedAxesEnum
	{
		X,
		Z
	}
	[Export]
	public ReflectedAxesEnum ReflectedAxes { get; set; }

	private CharacterBody3D _reflection;
	private Area3D _area3D;
	public override void _Ready()
	{
		_area3D = GetNode<Area3D>("Area3D");
		_area3D.BodyEntered += OnAreaEnteredByBody;
		_area3D.BodyExited += OnAreaLeavedByBody;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsInstanceValid(Player) && IsInstanceValid(_reflection))
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

	private void OnAreaEnteredByBody(Node3D body)
	{
		if (body is Player && !IsInstanceValid(_reflection))
		{
			_reflection = new CharacterBody3D();
			_reflection.AddChild(Player.GetNode<Node3D>("character").Duplicate());
			_reflection.AddChild(Player.GetNode<Node3D>("CollisionShape3D").Duplicate());
			SetupReflectTransform();
			GetTree().Root.AddChild(_reflection);
		}
	}

	private void OnAreaLeavedByBody(Node3D body)
	{
		if (IsInstanceValid(_reflection))
		{
			_reflection.QueueFree();
			_reflection = null;
		}
	}
}
