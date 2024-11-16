using Godot;
using System;

public partial class Control : Godot.Control
{
	private RichTextLabel _richTextLabel;
	private RichTextLabel _gameOverText;
	private int Score = 0;
	public override void _Ready()
	{
		_richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
		_gameOverText = GetNode<RichTextLabel>("GameOverText");

		GlobalSignals.Instance.PlayerIsDead += OnPlayerDead;
		GlobalSignals.Instance.NPCDies += OnNpcDies;

		_gameOverText.Visible = false;
		_richTextLabel.Text = "Score : " + Score.ToString();
	}
	public override void _ExitTree()
	{
		GlobalSignals.Instance.PlayerIsDead -= OnPlayerDead;
		GlobalSignals.Instance.NPCDies -= OnNpcDies;
	}
	private void OnNpcDies(FireBall fireBall)
	{
		Score += 1;
		GD.Print(Score);
		_richTextLabel.Text = "Score : " + Score.ToString();
	}

	private void OnPlayerDead()
	{
		_gameOverText.Visible = true;
	}
}
