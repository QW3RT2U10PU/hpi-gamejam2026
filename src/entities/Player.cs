using System;

namespace GameJam;

using Godot;

public partial class Player : CharacterBody2D
{
	public static PackedScene Scene { get; } = GD.Load<PackedScene>("uid://l5ejvbu0pwmb");

	[Export]
	public float Speed {get; set;} = 300.0f;

	[Export]
	public float Acceleration {get; set;} = 1000f;
	
	
	[Export]
	public float DashSpeed {get; set;} = 3f;

	[Export]
	public float JumpStrength {get; set;} = 400f;

	[Export] public float AirDrag { get; set; } = 1.7f;

	[Export] public double CoyoteTime {get; set;} = 0.08;
	private double remainingCoyoteTime = 0;

	private enum DashState {ACTIVE = 4, COOLDOWN = 1, NEEDS_FLOOR = 2}
	private DashState dashState = 0;

	public override void _PhysicsProcess(double delta)
	{
		Timer dashTimer = GetNode<Timer>("dashTimer");
		Timer dashCooldown = GetNode<Timer>("dashCooldown");

		// Add the gravity.
		Vector2 direction = Vector2.Zero;
		if (Input.IsActionPressed("move_right")) direction.X++;
		if (Input.IsActionPressed("move_left")) direction.X--;
		if (Input.IsActionPressed("move_down")) direction.Y++;
		if (Input.IsActionPressed("jump")) direction.Y--;
		
		Vector2 velocity = Velocity;
		velocity.X = direction.X * Speed;
		float currentAcceleration = Acceleration;

		if (Input.IsActionJustPressed("dash") && dashState == 0)
		{
			dashTimer.Start();
			dashCooldown.Start();
			dashState = DashState.ACTIVE;
		}
		
		if (IsOnFloor())
		{
			remainingCoyoteTime = CoyoteTime;
			if (dashState != DashState.ACTIVE) dashState &= ~DashState.NEEDS_FLOOR;
		}
		else {
			remainingCoyoteTime -= delta;
			currentAcceleration /= AirDrag;
			if (dashState != DashState.ACTIVE && remainingCoyoteTime <= 0)
			{
				velocity += GetGravity() * (float)delta;
			}
			else
			{
				velocity.Y = 0;
			}
		}
		
		if (direction.Y < 0 && remainingCoyoteTime > 0)
		{
			velocity.Y = -JumpStrength;
			remainingCoyoteTime = 0;
		}


		if (dashState == DashState.ACTIVE)
		{
			velocity.X *= DashSpeed;
		}
		else
		{
			float xVelocityDelta = velocity.X - Velocity.X;
			xVelocityDelta = Math.Sign(xVelocityDelta) * Math.Min(Math.Abs(xVelocityDelta), currentAcceleration * (float) delta);
			velocity.X = Math.Clamp(Velocity.X + xVelocityDelta, -Speed, Speed);
		}


		Velocity = velocity;
		MoveAndSlide();
	}

	public void DashExpired()
	{
		dashState = DashState.COOLDOWN | DashState.NEEDS_FLOOR;
	}

	public void DashCooldownExpired()
	{
		dashState &= ~DashState.COOLDOWN;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			GetViewport().SetInputAsHandled();
			Interactable.FocusedInteractable?.Interact(Interactable.FocusedInteractableBody);
		}
	}
}
