using Godot;

[GlobalClass]
public partial class Projectile : SceneSwitcher
{
	[Export] public float Speed {get; set;} = 100;
	public Vector2 Velocity {get; set;} = Vector2.Zero;

	[Export] public double Lifetime {get; set;} = 5;

    public override void _Ready()
    {
        base._Ready();
		GetTree().CreateTimer(Lifetime).Timeout += QueueFree;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Velocity * (float) delta;
	}
}
