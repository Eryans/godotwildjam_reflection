using Godot;
using System;

public partial class Control : Godot.Control
{
	private RichTextLabel richTextLabel;
	private int Score = 0;
	public override void _Ready()
	{
		richTextLabel = GetNode<RichTextLabel>("RichTextLabel");
		GlobalSignals.Instance.NPCDies += OnNpcDies;
		richTextLabel.Text = "Score : " + Score.ToString();
	}

	public override void _Process(double delta)
	{

	}

	private void OnNpcDies(FireBall fireBall)
	{
		Score += 1;
		GD.Print(Score);
		richTextLabel.Text = "Score : " + Score.ToString();
	}
}
