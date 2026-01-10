using Godot;

[GlobalClass]
public partial class SceneSwitcher : Interactable
{
	[Export] public PackedScene Loads {get; set;} = null;

    public override void Interact(Node body)
    {
        GetTree().ChangeSceneToPacked(Loads);
    }
}
