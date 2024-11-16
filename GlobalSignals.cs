using Godot;
using System;

public partial class GlobalSignals : Node
{
	public static GlobalSignals Instance { get; private set; }

	[Signal]
	public delegate void ReflectBodyTriggerEventHandler(Area3D area3D);
	[Signal]
	public delegate void NPCHitByProjectileEventHandler(string npcName, FireBall projectile);
	[Signal]
	public delegate void NPCDiesEventHandler(FireBall projectile);
	[Signal]
	public delegate void NPCAttackEventHandler(int attack);
	[Signal]
	public delegate void PlayerIsDeadEventHandler();

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitReflectBodyTrigger(Area3D area3D)
	{
		EmitSignal(nameof(ReflectBodyTrigger), area3D);
	}

	public void EmitNPCHitByProjectile(string npcName, FireBall projectile)
	{
		EmitSignal(nameof(NPCHitByProjectile), npcName, projectile);
	}

	public void EmitNPCDies(FireBall projectcile)
	{
		EmitSignal(nameof(NPCDies), projectcile);
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
