using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public static PackedScene Scene {get;} = GD.Load<PackedScene>("uid://l5ejvbu0pwmb");

	[Export]
	public float Speed {get; set;} = 300.0f;
	
	[Export]
	public float AirDrag {get; set;} = 1.7f;
	
	[Export]
	public float DashSpeed {get; set;} = 5f;

	[Export]
	public float JumpStrength {get; set;} = 400.0f;

	private enum DashState {ACTIVE = 4, COOLDOWN = 1, NEEDS_FLOOR = 2}
	private DashState dashState = 0;

	public override void _PhysicsProcess(double delta)
	{
		GD.Print(dashState);
		Timer dashTimer = GetNode<Timer>("dashTimer");
		Timer dashCooldown = GetNode<Timer>("dashCooldown");
		Vector2 velocity = Velocity;

		// Add the gravity.
		Vector2 direction = Input.GetVector("move_left", "move_right", "jump", "move_down");
		velocity.X = direction.X * Speed;
		
		if(Input.IsActionJustPressed("dash") && dashState == 0) {
			dashTimer.Start();
			dashCooldown.Start();
			dashState = DashState.ACTIVE;
		}

		if(dashState == DashState.ACTIVE)
		{
			velocity.X *= DashSpeed;
		}
		
		if (IsOnFloor())
		{
			if (dashState != DashState.ACTIVE) dashState &= ~DashState.NEEDS_FLOOR;
		}
		else if (dashState != DashState.ACTIVE)
		{
			velocity += GetGravity() * (float)delta;
			velocity.X /= AirDrag;
		}
		else
		{
			velocity.Y = 0;
		}
		
		if (direction.Y < 0 && IsOnFloor())
		{
			velocity.Y = -JumpStrength;
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
