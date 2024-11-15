using Godot;

public partial class ReflectHandler : Marker3D
{

	public enum ReflectedAxesEnum
	{
		X,
		Z
	}
	[Export]
	public ReflectedAxesEnum ReflectedAxes { get; set; }

	private CharacterBody3D _reflection;
	private Player _player;
	public override void _Ready()
	{
		if (!IsInstanceValid(_player))
		{
			_player = GetTree().Root.GetChild(1).GetNode<Player>("Player");
			_reflection = (CharacterBody3D)_player.Duplicate();
			SetupReflectTransform(_player, _reflection);
			GetTree().Root.CallDeferred("add_child", _reflection);
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsInstanceValid(_player) && IsInstanceValid(_reflection))
		{
			// HandleReflectVelocity();
			SetupReflectTransform(_player, _reflection);
			HandleBasis();
		}

	}

	private void SetupReflectTransform(Node3D originalProp, Node3D reflection)
	{
		var transform = originalProp.Transform;
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			transform.Origin.X = 2 * Transform.Origin.X - originalProp.Transform.Origin.X;
		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)

		{
			transform.Origin.Z = 2 * Transform.Origin.Z - originalProp.Transform.Origin.Z;
		}
		reflection.Transform = transform;
	}
	private void HandleBasis()
	{
		Node3D reflectionMesh = _reflection.GetNode<Node3D>("character");
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			reflectionMesh.Scale = new Vector3(-1, 1, 1);
		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			reflectionMesh.Scale = new Vector3(1, 1, -1);

		}
		_reflection.Rotation = -_player.Rotation;
	}

	private void HandleReflectVelocity()
	{
		Vector3 playerVelocity = _player.Velocity;
		Vector3 inversedAxes = new(playerVelocity.X, playerVelocity.Y, playerVelocity.Z);
		if (ReflectedAxes == ReflectedAxesEnum.X)
		{
			inversedAxes.X = -playerVelocity.X;

		}
		else if (ReflectedAxes == ReflectedAxesEnum.Z)
		{
			inversedAxes.Z = -playerVelocity.Z;

		}
		_reflection.Velocity = inversedAxes;
		_reflection.MoveAndSlide();
	}
}
