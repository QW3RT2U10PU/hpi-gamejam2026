using Godot;

[GlobalClass]
public partial class SceneSwitcher : Interactable
{
	[Export(PropertyHint.File, "*.scn, *.tscn")] public string Loads {get; set;} = null;

    public override void Interact(Node body)
    {
        GetTree().ChangeSceneToFile(Loads);
    }
}
