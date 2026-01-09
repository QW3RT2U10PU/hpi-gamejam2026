using Godot;
using System;

public partial class Level : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var plr = GetNode("%Player");
		var camera = new Camera2D();
		var bounds = GetUsedRect();
		Vector2 pos = MapToLocal(bounds.Position) - TileSet.TileSize / 2;
		Vector2 end = MapToLocal(bounds.End) - TileSet.TileSize / 2;
		camera.LimitTop = (int)Math.Ceiling(Math.Min(pos.Y, end.Y));
		camera.LimitBottom = (int)Math.Floor(Math.Max(pos.Y, end.Y));
		camera.LimitLeft = (int)Math.Ceiling(Math.Min(pos.X, end.X));
		camera.LimitRight = (int)Math.Floor(Math.Max(pos.X, end.X));
		plr.AddChild(camera);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
