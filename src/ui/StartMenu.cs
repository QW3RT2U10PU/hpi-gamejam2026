using Godot;
using System;

public partial class StartMenu : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartGame()
	{
		// res://src/levels/Level1Room1.tscn
		GetTree().ChangeSceneToFile("uid://bfpu7cdwgvy54");
	}
	public void QuitGame()
	{
		GetTree().Quit();
	}
}
