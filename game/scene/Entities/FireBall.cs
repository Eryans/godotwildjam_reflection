using Godot;

public partial class FireBall : CharacterBody3D
{
	private Vector3 _direction;
	[Export]
	public float Speed = 15f;

	[Export]
	public int maxBounceBeforeDeath = 3;
	public override void _Ready()
	{
		_direction = Transform.Basis.Z;
		Velocity = _direction * Speed;
	}

	public override void _Process(double delta)
	{
		if (maxBounceBeforeDeath <= 0)
		{
			QueueFree();
		}
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();

			if (body is not Player)
			{
				_direction = Velocity.Bounce(collision.GetNormal()).Normalized();
				maxBounceBeforeDeath -= 1;
				Velocity = _direction * Speed;
			}
			if (body is CharacterBody3D cb && cb.IsInGroup("npc"))
			{
				string npcName = cb.Name;
				GlobalSignals.Instance.EmitNPCHitByProjectile(npcName);
				QueueFree();
			}
			if (body is RigidBody3D rb)
			{
				Vector3 pushDirection = (rb.Transform.Origin - Transform.Origin).Normalized();
				rb.ApplyCentralForce(pushDirection * 50);
			}
		}
	}
}
