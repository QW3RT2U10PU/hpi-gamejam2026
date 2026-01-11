using Godot;
using System;
using System.Globalization;

public partial class InteractionIndicator : Sprite2D
{
	private static readonly Rect2 unpressedRegion = new(64, 32, new(16, 16));
	private static readonly Rect2 pressedRegion = new(64, 144, new(16, 16));

	public bool Enabled {get
		{
			return Visible;
		}
		set
		{
			Visible = value;
			Timer timer = GetNode<Timer>("BlinkTimer");
			if (value) {
				((AtlasTexture) Texture).Region = unpressedRegion;
				timer.Start();
			}
			else timer.Stop();
		}
	}

	private void togglePressed()
	{
		if (((AtlasTexture) Texture).Region.Equals(unpressedRegion))
		{
			((AtlasTexture) Texture).Region = pressedRegion;
		}
		else ((AtlasTexture) Texture).Region = unpressedRegion;
	}
}
