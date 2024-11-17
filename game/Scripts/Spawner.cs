using System.Collections.Generic;
using Godot;

public partial class Spawner : Path3D
{
	[Export]
	public float SpawnRateTimerValue = 5.0f;
	[Export]
	public int MaxNpc = 50;
	private PackedScene _enemyNpc;
	private Timer _spawnRateTimer = new();
	public override void _Ready()
	{
		_spawnRateTimer.Timeout += OnTimeOut;
		AddChild(_spawnRateTimer);
		_spawnRateTimer.Start(SpawnRateTimerValue);
		_enemyNpc = GD.Load<PackedScene>("res://game/scene/Entities/NPC/npc.tscn");
	}

	public override void _Process(double delta)
	{
	}

	private void OnTimeOut()
	{
		if (GlobalSignals.Instance.Npcs.Count < MaxNpc)
		{
			Npc mob = (Npc)_enemyNpc.Instantiate();
			GlobalSignals.Instance.Npcs.Add(mob);
			PathFollow3D mobSpawnLocation = GetNode<PathFollow3D>("PathFollow");
			GetTree().Root.GetChild(1).AddChild(mob);
			mobSpawnLocation.ProgressRatio = GD.Randf();
			mob.GlobalPosition = mobSpawnLocation.GlobalPosition;
			_spawnRateTimer.Start(SpawnRateTimerValue);
		}
	}
}
