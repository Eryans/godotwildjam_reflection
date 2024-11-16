using Godot;
using System;

public partial class Controller : Node
{
	[Export]
	public Player Player;
	private float _speed = 8.0f;

	private bool _gameover = false;
	public override void _Ready()
	{
		GlobalSignals.Instance.PlayerIsDead += OnPlayerDead;
	}

	public override void _Process(double delta)
	{
		if (IsInstanceValid(Player))
		{
			if (!_gameover)
			{

				if (Input.IsActionJustPressed("l_mouse"))
				{
					Player.GetNode<ProjectileLauncher>("ProjectileLauncher").Shoot();
					Player.GetNode<ProjectileLauncher>("ProjectileLauncher2").Shoot();
				}
				Vector3 velocity = Player.Velocity;

				if (!Player.IsOnFloor())
				{
					velocity += Player.GetGravity() * (float)delta;
				}
				Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
				Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
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
				Player.Velocity = velocity;
			}
			else
			{

				Player.Velocity = Vector3.Zero;
			}

		}
		if (_gameover && Input.IsActionJustPressed("enter"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	private void OnPlayerDead()
	{
		_gameover = true;
	}
}
