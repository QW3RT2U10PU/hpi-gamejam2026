using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public static PackedScene Scene {get;} = GD.Load<PackedScene>("uid://l5ejvbu0pwmb");

	[Export]
	public float Speed {get; set;} = 300.0f;

	[Export]
	public float JumpStrength {get; set;} = 400.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");
		velocity.X = direction.X * Speed;
		if (direction.Y < 0 && IsOnFloor())
		{
			velocity.Y = -JumpStrength;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
