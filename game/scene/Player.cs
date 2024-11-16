using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	TopViewCamera Camera;
	public const float JumpVelocity = 4.5f;
	private Health _health = new();
	public override void _Ready()
	{
		AddChild(_health);
		_health.OnDies += PlayerDies;
		GlobalSignals.Instance.NPCAttack += OnBeingAttacked;
	}
	public override void _PhysicsProcess(double delta)
	{

		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision3D collision = GetSlideCollision(i);
			var body = collision.GetCollider();
			if (body is RigidBody3D rb)
			{
				Vector3 pushDirection = rb.GlobalTransform.Origin - GlobalTransform.Origin;
				rb.ApplyCentralForce(pushDirection.Normalized() * 50);
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

	private void OnBeingAttacked(int damage)
	{
		GD.Print("Player is being attacked !");
		GD.Print("Player lose in HP: ", damage);
		_health.LoseHealth(damage);
	}

	private void PlayerDies()
	{
		GD.Print("You are dead, not big soorprize");
	}
}
