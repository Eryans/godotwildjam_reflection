using Godot;
using System;

public partial class Health : Node
{
	[Export]
	public int MaxHealth = 5;

	[Signal]
	public delegate void OnDiesEventHandler();
	private int _health;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_health = MaxHealth;
	}

	public override void _Process(double delta)
	{
		if (_health <= 0)
		{
			EmitSignal(nameof(OnDies));
		}
	}
	public void LoseHealth(int damage = 1)
	{
		_health -= damage;
	}
}
