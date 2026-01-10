using Godot;
using System;

public partial class Pausemenu : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			GetTree().Paused = !GetTree().Paused;
			if (GetTree().Paused)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}
	}

	public void ContinueGame()
	{
		Hide();
		GetTree().Paused = false;
	}
	public void GoToMainMenu()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("uid://difj88kcjy8k3");
	}
	public void QuitGame()
	{
		GetTree().Paused = false;
		GetTree().Quit();
	}
}
