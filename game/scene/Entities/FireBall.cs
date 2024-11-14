using Godot;

public partial class FireBall : CharacterBody3D
{
	private Vector3 _direction;
	[Export]
	public float Speed = 15f;

	public override void _Ready()
	{
		_direction = -Transform.Basis.Z;
		Velocity = _direction * Speed; // Initialisation de la vélocité ici
	}

	public override void _Process(double delta)
	{
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();

			if (body is not Player)
			{
				// On rebondit en utilisant la normale de la collision
				_direction = Velocity.Bounce(collision.GetNormal()).Normalized();
				Velocity = _direction * Speed; // On réactualise la vélocité après le rebond
			}

			if (body is RigidBody3D rb)
			{
				// Appliquer une force pour repousser le corps rigide
				Vector3 pushDirection = (rb.Transform.Origin - Transform.Origin).Normalized();
				rb.ApplyCentralForce(pushDirection * 50);
			}
		}
	}
}
