using Godot;
using System;

public partial class TopViewCamera : Camera3D
{

	public Vector3 ShootRay()
	{
		Vector2 mousePos = GetViewport().GetMousePosition();
		int rayLength = 1000;

		Vector3 from = ProjectRayOrigin(mousePos);
		Vector3 to = from + ProjectRayNormal(mousePos) * rayLength;

		var space = GetWorld3D().DirectSpaceState;
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
