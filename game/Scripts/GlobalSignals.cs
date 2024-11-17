using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalSignals : Node
{
	public static GlobalSignals Instance { get; private set; }

	[Signal]
	public delegate void ReflectBodyTriggerEventHandler(Area3D area3D);
	[Signal]
	public delegate void NPCHitByProjectileEventHandler(string npcName, int damage);
	[Signal]
	public delegate void NPCDiesEventHandler(int damage);
	[Signal]
	public delegate void NPCAttackEventHandler(int attack);
	[Signal]
	public delegate void PlayerIsDeadEventHandler();
	public List<Npc> Npcs = new();
	public List<FireBall> Projectiles = new();

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitReflectBodyTrigger(Area3D area3D)
	{
		EmitSignal(nameof(ReflectBodyTrigger), area3D);
	}

	public void EmitNPCHitByProjectile(string npcName, int damage)
	{
		EmitSignal(nameof(NPCHitByProjectile), npcName, damage);
	}

	public void EmitNPCDies(int damage)
	{
		EmitSignal(nameof(NPCDies), damage);
	}

	public void EmitNpcAttack(int attack)
	{
		EmitSignal(nameof(NPCAttack), attack);
	}

	public void EmitPlayerDead()
	{
		EmitSignal(nameof(PlayerIsDead));
	}
}
