using Godot;
using System;

public partial class Control : Godot.Control
{
	private RichTextLabel _richTextLabel;
	private RichTextLabel _gameOverText;
	private RichTextLabel _fireballAmount;
	private RichTextLabel _canDash;
	private int Score = 0;
	public override void _Ready()
	{
		_richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
		_gameOverText = GetNode<RichTextLabel>("GameOverText");
		_canDash = GetNode<Panel>("Panel").GetNode<RichTextLabel>("CanDash");
		_fireballAmount = GetNode<Panel>("Panel").GetNode<RichTextLabel>("FireballAmount");

		GlobalSignals.Instance.PlayerIsDead += OnPlayerDead;
		GlobalSignals.Instance.NPCDies += OnNpcDies;
		GlobalSignals.Instance.PlayerCanDash += OnPlayerCanDash;
		GlobalSignals.Instance.PlayerProjectile += OnPlayerProjectileChange;

		_gameOverText.Visible = false;
		_richTextLabel.Text = "Score : " + Score.ToString();
	}
	public override void _ExitTree()
	{
		GlobalSignals.Instance.PlayerIsDead -= OnPlayerDead;
		GlobalSignals.Instance.NPCDies -= OnNpcDies;
		GlobalSignals.Instance.PlayerCanDash -= OnPlayerCanDash;
		GlobalSignals.Instance.PlayerProjectile -= OnPlayerProjectileChange;
	}
	private void OnNpcDies(int damage)
	{
		Score += damage;
		GD.Print(Score);
		_richTextLabel.Text = "Score : " + Score.ToString();
	}

	private void OnPlayerDead()
	{
		_gameOverText.Visible = true;
	}

	private void OnPlayerCanDash(bool canDash)
	{
		_canDash.Text = canDash ? "Dash !" : "No Dash :(";
	}

	private void OnPlayerProjectileChange(int max, int curr)
	{
		_fireballAmount.Text = "'Ammo' : " + (max - curr) + "/" + max;
	}
}
