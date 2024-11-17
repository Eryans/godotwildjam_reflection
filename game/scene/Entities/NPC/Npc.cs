using Godot;
using Godot.Collections;
using System;

public partial class Npc : CharacterBody3D
{
	[Export]
	public float Speed = 2.5f;
	[Export]
	public float AttackRate = 2.5f;
	[Export]
	public int Attack = 1;
	private int _health = 5;
	private Array<Node3D> targets = new();
	private NavigationAgent3D _navAgent;
	private Node3D _currentTarget;
	private bool _canAttack = true;
	private Timer _canAttackTimer = new();
	private Area3D _attackZone;
	private bool _isInAttackRange = false;
	public override void _Ready()
	{
		_attackZone = GetNode<Area3D>("DamageZone");
		_attackZone.BodyEntered += OnBodyEnterDamageZone;
		_attackZone.BodyExited += OnBodyExitDamageZone;

		GlobalSignals.Instance.NPCHitByProjectile += LoseHealth;
		_canAttackTimer.Timeout += OnAttackTimeout;
		AddChild(_canAttackTimer);
		_canAttackTimer.Start(AttackRate);
		CallDeferred("SetupNavAgent");
	}
	public override void _ExitTree()
	{
		GlobalSignals.Instance.NPCHitByProjectile -= LoseHealth;
	}
	public override void _Process(double delta)
	{
		if (_isInAttackRange && _canAttack)
		{
			_canAttack = false;
			_canAttackTimer.Start(AttackRate);
			GlobalSignals.Instance.EmitNpcAttack(Attack);
		}
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

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();
			if (body is RigidBody3D rb)
			{
				Vector3 pushDirection = rb.GlobalTransform.Origin - GlobalTransform.Origin;
				rb.ApplyCentralForce(pushDirection.Normalized() * 50);
			}
		}

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
		if (IsInstanceValid(_navAgent) && IsInstanceValid(target))
		{
			_navAgent.TargetPosition = target.GlobalTransform.Origin;
		}
	}

	private void LoseHealth(string npcName, int receivedDamage)
	{
		if (Name == npcName)
		{
			_health -= receivedDamage;
			GD.Print("lost ", receivedDamage, " HP");
			if (_health <= 0)
			{
				GlobalSignals.Instance.NPCHitByProjectile -= LoseHealth;
				GlobalSignals.Instance.EmitNPCDies(receivedDamage);
				GlobalSignals.Instance.Npcs.RemoveAll((mob) => mob.Name == npcName);
				QueueFree();
			}
		}
	}
	private void OnBodyEnterDamageZone(Node3D body)
	{
		if (body is Player)
		{
			_isInAttackRange = true;
		}
	}
	private void OnBodyExitDamageZone(Node3D body)
	{
		if (body is Player)
		{
			_isInAttackRange = false;
		}
	}
	private void OnAttackTimeout()
	{
		_canAttack = true;
	}
}
