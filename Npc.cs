using Godot;
using Godot.Collections;
using System;

public partial class Npc : CharacterBody3D
{
	[Export]
	public float Speed = 2.5f;
	private int _health = 5;
	private Array<Node3D> targets = new();
	private NavigationAgent3D _navAgent;
	private Node3D _currentTarget;
	public override void _Ready()
	{
		GlobalSignals.Instance.NPCHitByProjectile += LoseHealth;
		CallDeferred("SetupNavAgent");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
		if (IsInstanceValid(_currentTarget))
		{
			LookAt(_currentTarget.GlobalTransform.Origin);
			Vector3 rotation = Rotation;
			rotation.X = 0;
			rotation.Z = 0;
			Rotation = rotation;
			Vector3 currentLocation = GlobalTransform.Origin;
			Vector3 nextLocation = _navAgent.GetNextPathPosition();
			Vector3 newVelocity = (nextLocation - currentLocation).Normalized() * 3;
			Velocity = newVelocity;
			MoveAndSlide();
		}
		_currentTarget = GetNearestTarget();
		UpdateTargetLocation(_currentTarget);
	}

	private Node3D GetNearestTarget()
	{
		if (targets.Count > 0)
		{

			Node3D nearest = null;
			foreach (Node3D target in targets)
			{
				nearest ??= target;
				if (target.GlobalPosition.DistanceTo(GlobalPosition) < nearest.GlobalPosition.DistanceTo(GlobalPosition))
				{
					nearest = target;
				}
			}
			return nearest;
		}
		return _currentTarget;
	}

	private void SetupNavAgent()
	{
		_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		var mainGameNodes = GetTree().Root.GetChild(1).GetChildren();

		foreach (Node child in mainGameNodes)
		{
			if (child.IsInGroup("player"))
			{
				targets.Add((Node3D)child);
			}
		}
		_currentTarget = GetNearestTarget();
		UpdateTargetLocation(_currentTarget);
	}
	public void UpdateTargetLocation(Node3D target)
	{
		if (IsInstanceValid(_navAgent))
		{
			_navAgent.TargetPosition = target.GlobalTransform.Origin;
		}
	}

	private void LoseHealth(string npcName)
	{
		if (Name == npcName)
		{
			_health -= 1;
			if (_health <= 0)
			{
				GlobalSignals.Instance.NPCHitByProjectile -= LoseHealth;
				QueueFree();
			}
		}
	}
}
