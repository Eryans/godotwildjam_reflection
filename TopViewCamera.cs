using Godot;
using System;

public partial class TopViewCamera : SpringArm3D
{
	[Export]
	public Node3D Target;
	[Export]
	public float CameraLerpValue = 2.5f;

	private Camera3D camera3D;

	public override void _Ready()
	{
		camera3D = GetNode<Camera3D>("Camera3D");
	}
	public override void _Process(double delta)
	{
		Transform3D globalTransform = GlobalTransform;
		globalTransform.Origin = globalTransform.Origin.Lerp(Target.GlobalTransform.Origin, CameraLerpValue * (float)delta);
		// globalTransform.Origin = Target.GlobalTransform.Origin;
		GlobalTransform = globalTransform;
	}
	public Vector3 ShootRay()
	{
		Vector2 mousePos = camera3D.GetViewport().GetMousePosition();
		int rayLength = 1000;

		Vector3 from = camera3D.ProjectRayOrigin(mousePos);
		Vector3 to = from + camera3D.ProjectRayNormal(mousePos) * rayLength;

		var space = camera3D.GetWorld3D().DirectSpaceState;
		var rayQuery = new PhysicsRayQueryParameters3D
		{
			From = from,
			To = to,
		};

		var result = space.IntersectRay(rayQuery);

		if (result.ContainsKey("position"))
		{
			return (Vector3)result["position"];
		}

		return Vector3.Zero;
	}

}
