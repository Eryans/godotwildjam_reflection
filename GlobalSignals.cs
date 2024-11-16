using Godot;
using System;

public partial class GlobalSignals : Node
{
	public static GlobalSignals Instance { get; private set; }

	[Signal]
	public delegate void ReflectBodyTriggerEventHandler(Area3D area3D);
	[Signal]
	public delegate void NPCHitByProjectileEventHandler(string npcName);

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitReflectBodyTrigger(Area3D area3D)
	{
		EmitSignal(nameof(ReflectBodyTrigger), area3D);
	}

	public void EmitNPCHitByProjectile(string npcName)
	{
		EmitSignal(nameof(NPCHitByProjectile), npcName);
	}
}
