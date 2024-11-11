using Godot;
using System;

public partial class Camera : SpringArm3D
{
	[Export]
	public Node3D Target;
	[Export]
	public Vector3 Offset;
	[Export]
	public bool ApplyBasisToTarget = true;
	[Export]
	public float LookAroundSpeed = 0.005f;

	private Transform3D targetTransform;
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Process(double delta)
	{

		if (IsInstanceValid(Target))
		{
			Transform3D localTransform = Transform;
			localTransform.Origin = Target.Transform.Origin + Offset;
			Transform = localTransform;
			if (ApplyBasisToTarget)
			{
				Target.Rotation = new Vector3(Target.Rotation.X, Rotation.Y, Target.Rotation.Z);
			}
		}
	}
	// accumulators

	// Thx Godot doc
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// modify accumulated mouse rotation
			_rotationX += -mouseMotion.Relative.X * LookAroundSpeed;
			_rotationY += -mouseMotion.Relative.Y * LookAroundSpeed;

			// reset rotation
			Transform3D transform = Transform;
			transform.Basis = Basis.Identity;
			Transform = transform;

			RotateObjectLocal(Vector3.Up, _rotationX); // first rotate about Y
			RotateObjectLocal(Vector3.Right, _rotationY); // then rotate about X
		}
	}
}
