using Godot;
using System;

public partial class ProjectileLauncher : Marker3D
{
	[Export]
	public float ShootingRate = .5f;
	[Export]
	public int MaxProjectile = 10;
	private PackedScene _projectile;
	private Timer _shootingRateTimer = new();
	private bool _canShoot = true;
	public override void _Ready()
	{
		GlobalSignals.Instance.EmitPlayerProjectile(MaxProjectile, GlobalSignals.Instance.Projectiles.Count);
		_projectile = GD.Load<PackedScene>("res://game/scene/Entities/Fireball/fire_ball.tscn");
		_shootingRateTimer.Timeout += OnTimeOut;
		AddChild(_shootingRateTimer);
		_shootingRateTimer.Start(ShootingRate);
	}

	public void Shoot()
	{
		if (_canShoot)
		{
			_canShoot = false;
			if (GlobalSignals.Instance.Projectiles.Count < MaxProjectile)
			{
				CharacterBody3D instance = (CharacterBody3D)_projectile.Instantiate();
				instance.GlobalTransform = GlobalTransform;
				GetTree().Root.GetChild(1).AddChild(instance);
				instance.GlobalBasis = GlobalBasis;
				GlobalSignals.Instance.Projectiles.Add((FireBall)instance);
				GlobalSignals.Instance.EmitPlayerProjectile(MaxProjectile, GlobalSignals.Instance.Projectiles.Count);
			}
		}
	}

	private void OnTimeOut()
	{
		_canShoot = true;
		_shootingRateTimer.Start(ShootingRate);
	}
}
