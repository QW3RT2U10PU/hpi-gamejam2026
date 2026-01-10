using Godot;
using System;

//NPCs assume being a child of a level, allowing to call the dialogue method of the level
[GlobalClass]
public partial class Npc : Interactable
{
	[Export]
	public Dialogue NpcDialogue {get; private set;}

    public override void Interact(Node body)
    {
		GetParent<Level>().StartDialogue(NpcDialogue);
    }
}
