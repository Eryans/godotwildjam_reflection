using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	TopViewCamera Camera;
	public const float JumpVelocity = 4.5f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();
			if (body is RigidBody3D rb)
			{
				Vector3 pushDirection = rb.Transform.Origin - Transform.Origin;
				rb.ApplyCentralForce(new Vector3(pushDirection.X, rb.Transform.Origin.Y, pushDirection.Z).Normalized() * 50);
			}
		}

		if (IsInstanceValid(Camera))
		{

			var lookAtDirection = Camera.ShootRay() - GlobalTransform.Origin;
			var targetRotation = Mathf.Atan2(lookAtDirection.X, lookAtDirection.Z);
			Vector3 gRotation = GlobalRotation;
			gRotation.Y = Mathf.LerpAngle(Rotation.Y, targetRotation, 1);
			GlobalRotation = gRotation;
		}


	}
}
