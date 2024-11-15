using Godot;
using System;

public partial class Controller : Node
{
	[Export]
	public Player Player;
	[Export]
	public Camera3D Camera;
	private float _speed = 5.0f;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		Vector3 velocity = Player.Velocity;

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = /*Player.Transform.Basis */ new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Player.Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(Player.Velocity.Z, 0, _speed);
		}

		Transform3D cameraGlobalTransform = Camera.GlobalTransform;
		cameraGlobalTransform.Origin.Z = Player.GlobalTransform.Origin.Z;
		Camera.GlobalTransform = cameraGlobalTransform;
		Player.Velocity = velocity;
	}
}
