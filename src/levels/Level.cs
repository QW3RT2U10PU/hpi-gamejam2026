using Godot;
using System;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var plr = GetNode("%Player");
		var camera = new Camera2D();
		var map = GetNode<TileMapLayer>("%Map");
		var bounds = map.GetUsedRect();
		Vector2 pos = ToLocal(map.ToGlobal(map.MapToLocal(bounds.Position) - map.TileSet.TileSize / 2));
		Vector2 end = ToLocal(map.ToGlobal(map.MapToLocal(bounds.End) - map.TileSet.TileSize / 2));
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

	public void StartDialogue(Dialogue dialogue)
	{
		GetNode<DialogueContainer>("%DialogueContainer").CurrentDialogue = dialogue;
	}
}
