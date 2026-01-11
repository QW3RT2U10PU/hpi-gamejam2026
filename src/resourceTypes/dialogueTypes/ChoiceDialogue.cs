using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ChoiceDialogue : Dialogue
{
    [Export] public Godot.Collections.Dictionary<string, Dialogue> Options = [];

    private Dialogue _choice;

    public override IEnumerator<(string, ICollection<string>)> GetEnumerator()
    {
        DialogueContainer.ActiveDialogueContainer.HasChosen += Choose;
        yield return (null, Options.Keys);
        foreach (var val in _choice) yield return val;
        _choice = null;
    }

    public void Choose(object sender, string c)
    {
        _choice = Options[c];
        GD.Print(_choice);
        DialogueContainer.ActiveDialogueContainer.HasChosen -= Choose;
    }
}