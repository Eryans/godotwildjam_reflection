using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ReflectHandler : Marker3D
{

	public enum ReflectedAxesEnum
	{
		X,
		Z
	}
	[Export]
	public ReflectedAxesEnum ReflectedAxes { get; set; }

	private CharacterBody3D _reflection;
	private Area3D _area3D;
	private Player _player { get; set; }
	private List<ReflectPair> _props = new();
	public override void _Ready()
	{
		if (!IsInstanceValid(_player))
		{
			_player = GetTree().Root.GetChild(0).GetNode<Player>("Player");
		}
		_area3D = GetNode<Area3D>("Area3D");
		_area3D.BodyEntered += OnAreaEnteredByBody;
		_area3D.BodyExited += OnAreaLeavedByBody;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsInstanceValid(_player) && IsInstanceValid(_reflection))
		{
			HandleReflectVelocity();
			HandleBasis();
		}
		foreach (var rb in _props)
		{
		}
	}
	private void SetupReflectTransform(Node3D originalProp, Node3D reflection)
	{
		var transform = originalProp.Transform;
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			transform.Origin.X = 2 * Transform.Origin.X - originalProp.Transform.Origin.X;
		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			transform.Origin.Z = 2 * Transform.Origin.Z - originalProp.Transform.Origin.Z;
		}
		reflection.Transform = transform;
	}
	private void HandleBasis()
	{
		Node3D reflectionMesh = _reflection.GetNode<Node3D>("character");
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			reflectionMesh.Scale = new Vector3(-1, 1, 1);
		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			reflectionMesh.Scale = new Vector3(1, 1, -1);

		}
		_reflection.Rotation = -_player.Rotation;
	}

	private void HandleReflectVelocity()
	{
		Vector3 playerVelocity = _player.Velocity;
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
			GD.Print("Player enter reflect zone");
			_reflection = new CharacterBody3D();
			_reflection.AddChild(_player.GetNode<Node3D>("character").Duplicate());
			_reflection.AddChild(_player.GetNode<Node3D>("CollisionShape3D").Duplicate());
			SetupReflectTransform(_player, _reflection);
			GetTree().Root.AddChild(_reflection);
		}
		if (body is RigidBody3D prop)
		{
			if (_props.FirstOrDefault(p => p.Id == prop.Name) == null)
			{

				RigidBody3D propReflect = (RigidBody3D)prop.Duplicate();
				SetupReflectTransform(prop, propReflect);
				GetTree().Root.AddChild(propReflect);
				_props.Add(new ReflectPair(prop.Name, prop, propReflect));
			}
		}
	}

	private void OnAreaLeavedByBody(Node3D body)
	{
		if (body is Player)
		{
			if (IsInstanceValid(_reflection))
			{
				_reflection.QueueFree();
				_reflection = null;
			}
		}
	}
}

public class ReflectPair
{
	public string Id { get; set; }
	public RigidBody3D Original { get; set; }
	public RigidBody3D Reflect { get; set; }

	public ReflectPair(string id, RigidBody3D original, RigidBody3D reflect)
	{
		Id = id;
		Original = original;
		Reflect = reflect;
	}
}