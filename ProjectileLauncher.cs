using Godot;
using System;

public partial class ProjectileLauncher : Marker3D
{
	[Export]
	public float ShootingRate = .5f;
	private PackedScene _projectile;
	private Timer _shootingRateTimer = new();
	private bool _canShoot = true;
	public override void _Ready()
	{
		_projectile = GD.Load<PackedScene>("res://game/scene/Entities/fire_ball.tscn");
		_shootingRateTimer.Timeout += OnTimeOut;
		AddChild(_shootingRateTimer);
		_shootingRateTimer.Start(ShootingRate);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("l_mouse") && _canShoot)
		{
			_canShoot = false;
			CharacterBody3D instance = (CharacterBody3D)_projectile.Instantiate();
			instance.GlobalTransform = GlobalTransform;
			GetTree().Root.AddChild(instance);
			instance.GlobalBasis = GlobalBasis;
		}
	}

	private void OnTimeOut()
	{
		_canShoot = true;
		_shootingRateTimer.Start(ShootingRate);
	}
}
