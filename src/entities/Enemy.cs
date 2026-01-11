using GameJam;
using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Enemy : Npc
{
	/// <summary>
	/// seconds
	/// </summary>
	[Export] public int AttackTime {get; private set;} = 10;

	/// <summary>
	/// attacks per second
	/// </summary>
	[Export] public double AttackSpeed {get; private set;} = 5;

	[Export] public Godot.Collections.Array<Dialogue> DialoguesBetweenAttacks {get; private set;} = [];

	/// <summary>
	/// Must be a projectile
	/// </summary>
	[Export] public PackedScene SpawnsProjectile {get; set;} = null;

	[Export] public float SpawnRadius {get; set;} = 500;

    public override Dialogue NpcDialogue {get
		{
			if (dialogues == null) return base.NpcDialogue;
			return dialogues.Current;
		}
		protected set
		{
			base.NpcDialogue = value;
		}
	}

	[Export(PropertyHint.File, "*.scn, *.tscn")] public string PostDefeatScene {get; set;} = null;


	private double queuedAttacks = 0;
	private IEnumerator<Dialogue> dialogues = null;
	private bool attacking = false;

    public override void Interact(Node body)
    {
        base.Interact(body);
		dialogues ??= DialoguesBetweenAttacks.GetEnumerator();
		if (dialogues.MoveNext())
		{
			GD.Print("Attack");
			attacking = true;
			queuedAttacks = 0;
			GetTree().CreateTimer(AttackTime).Timeout += EndAttack;
		}
		else
		{
			GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, PostDefeatScene);
		}
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (attacking) {
			Interaction = InteractionType.NEVER;
			if (Interactable.FocusedInteractable == this && FocusedInteractableBody is Player plr) plr.NotifyInteractable(false);
			queuedAttacks += delta * AttackSpeed;
			for (; queuedAttacks >= 1; queuedAttacks--)
			{
				GD.Print("Projectile");
				float rotation = Random.Shared.NextSingle() * (float) Math.PI * 2;
				Vector2 direction = Vector2.Right.Rotated(rotation);
				Projectile p = SpawnsProjectile.Instantiate<Projectile>();
				p.Position = direction * SpawnRadius;
				p.Velocity = -direction * p.Speed;
				p.Rotation = rotation;
				AddChild(p);
			}
		}
    }

	private void EndAttack()
	{
		Interaction = InteractionType.INTERACTABLE;
		attacking = false;
	}
}
