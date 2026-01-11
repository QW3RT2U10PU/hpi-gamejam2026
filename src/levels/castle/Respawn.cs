using GameJam;
using Godot;
using System;

public partial class Respawn : Area2D
{
	public void ResetPlayer(Node2D player)
	{
    	if (player is Player)
			{
				player.Position = new Vector2(176,8);
			}
	}
}
