using GameJam;
using Godot;
using System;
using System.Threading.Tasks.Dataflow;

[GlobalClass]
public abstract partial class Interactable : Area2D
{
	/// <summary>
	/// last interactable that was entered, or null after it was left
	/// </summary>
	public static Interactable FocusedInteractable {get; private set;} = null;
	/// <summary>
	/// body that entered FocusedInteractable
	/// </summary>
	public static Node FocusedInteractableBody {get; private set;} = null;

	public enum InteractionType
	{
		/// <summary>
		/// always triggers when entering
		/// </summary>
		ALWAYS,
		/// <summary>
		/// triggers once when entering, and interactable via key
		/// </summary>
		ONCE_AND_INTERACTABLE,
		/// <summary>
		/// triggers once when entering
		/// </summary>
		ONCE,
		/// <summary>
		/// interactable via key
		/// </summary>
		INTERACTABLE,
		/// <summary>
		/// is ignored
		/// </summary>
		NEVER
	}

	[Export]
	public InteractionType Interaction {get; set;} = InteractionType.ONCE_AND_INTERACTABLE;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += TryInteract;
		BodyExited += DisableInteractability;
		CollisionLayer = 2;
		CollisionMask = 1;
	}

	public void TryInteract(Node body)
	{
		switch (Interaction) {
			case InteractionType.ALWAYS:
				FocusedInteractable = this;
				FocusedInteractableBody = body;
				CallDeferred(MethodName.Interact, body);
				break;
			case InteractionType.ONCE_AND_INTERACTABLE:
				FocusedInteractable = this;
				FocusedInteractableBody = body;
				Interaction = InteractionType.INTERACTABLE;
				CallDeferred(MethodName.Interact, body);
				break;
			case InteractionType.ONCE:
				Interaction = InteractionType.NEVER;
				CallDeferred(MethodName.Interact, body);
				break;
			case InteractionType.INTERACTABLE:
				FocusedInteractable = this;
				FocusedInteractableBody = body;
				break;
		}
		if (FocusedInteractable != null && FocusedInteractableBody is Player p) p.NotifyInteractable(true);
	}

	public void DisableInteractability(Node body)
	{
		if (FocusedInteractable == this) {
			FocusedInteractable = null;
			if (FocusedInteractableBody is Player p) p.NotifyInteractable(false);
		}
	}

	/// <summary>
	/// abstract method for interaction
	/// </summary>
	/// <param name="body">physics body that entered the area, usually ignored</param>
	public abstract void Interact(Node body);
}
