using Godot;
using System;

public partial class RoomTeleport : Node3D
{
	[Export]
	public Player Player;
	[Export]
	public RoomTeleport Target;

	private Area3D _area3D;
	public Marker3D Spawn { get; private set; }
	private bool _canInteract;

	public override void _Ready()
	{
		Spawn = GetNode<Marker3D>("Spawn");
		_area3D = GetNode<Area3D>("Area3D");
		_area3D.BodyEntered += OnAreaBodyEntered;
		_area3D.BodyExited += OnAreaBodyExited;

		// Vérifie que Target et Spawn sont bien assignés
		if (Target?.Spawn == null)
		{
			GD.PrintErr("Target ou Target.Spawn n'est pas assigné !");
			return;
		}
	}

	public override void _Process(double delta)
	{
		// Vérifie si l'utilisateur appuie sur "ui_accept" et qu'on peut interagir
		if (Input.IsActionJustPressed("ui_accept") && _canInteract && Target?.Spawn != null)
		{
			GD.Print("Téléportation activée");

			// Téléporte Player à la position exacte de Target.Spawn
			Player.Transform = Target.Spawn.GlobalTransform;
		}
	}

	public void SetTarget(RoomTeleport rt)
	{
		Target = rt;
	}

	private void OnAreaBodyEntered(Node3D body)
	{
		if (body == Player)
		{
			_canInteract = true;
		}
	}

	private void OnAreaBodyExited(Node3D body)
	{
		if (body == Player)
		{
			_canInteract = false;
		}
	}
}
