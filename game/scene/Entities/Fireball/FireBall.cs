using Godot;

public partial class FireBall : CharacterBody3D
{
	[Export]
	public float Speed = 15f;

	[Export]
	public int maxBounceBeforeDeath = 10;
	public int Damage { get { return _damage; } private set { _damage = value; } }
	private Vector3 _direction;
	private int _damage = 1;


	public override void _Ready()
	{
		_direction = Transform.Basis.Z;
		Velocity = _direction * Speed;
	}

	public override void _Process(double delta)
	{
		if (maxBounceBeforeDeath <= 0)
		{
			GlobalSignals.Instance.Projectiles.RemoveAll((projectile) => projectile.Name == Name);
			GlobalSignals.Instance.EmitPlayerProjectile(10, GlobalSignals.Instance.Projectiles.Count);

			QueueFree();
		}
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();
			_direction = Velocity.Bounce(collision.GetNormal()).Normalized();
			if (body is not FireBall)
			{
				maxBounceBeforeDeath -= 1;
			}
			else
			{
				Speed++;
			}
			Velocity = _direction * Speed;
			if (body is CharacterBody3D cb && cb.IsInGroup("npc"))
			{
				string npcName = cb.Name;
				GlobalSignals.Instance.EmitNPCHitByProjectile(npcName, Damage);
				Speed++;
			}
			if (body is RigidBody3D rb)
			{
				Vector3 pushDirection = (rb.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
				rb.ApplyCentralForce(pushDirection * 500);
			}
			_damage++;
		}
	}

}
