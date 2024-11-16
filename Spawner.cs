using Godot;

public partial class Spawner : Path3D
{
	[Export]
	public float SpawnRateTimerValue = 2.0f;
	private PackedScene _enemyNpc;
	private Timer _spawnRateTimer = new();
	public override void _Ready()
	{
		_spawnRateTimer.Timeout += OnTimeOut;
		AddChild(_spawnRateTimer);
		_spawnRateTimer.Start(SpawnRateTimerValue);
		_enemyNpc = GD.Load<PackedScene>("res://game/scene/Entities/npc.tscn");
	}

	public override void _Process(double delta)
	{
	}

	private void OnTimeOut()
	{
		GD.Print("spawniing");
		Npc mob = (Npc)_enemyNpc.Instantiate();
		PathFollow3D mobSpawnLocation = GetNode<PathFollow3D>("PathFollow");
		GetTree().Root.GetChild(1).AddChild(mob);
		mobSpawnLocation.ProgressRatio = GD.Randf();
		mob.GlobalPosition = mobSpawnLocation.GlobalPosition;
		_spawnRateTimer.Start(SpawnRateTimerValue);
	}
}
