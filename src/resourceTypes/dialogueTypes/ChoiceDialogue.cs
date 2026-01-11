using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ChoiceDialogue : Dialogue
{
    [Export] public Godot.Collections.Dictionary<string, Dialogue> Options = [];

    private Dialogue _choice;

    public override IEnumerator<(string, ICollection<string>, Dialogue)> GetEnumerator()
    {
        DialogueContainer.ActiveDialogueContainer.HasChosen += Choose;
        yield return (null, Options.Keys, this);
        foreach (var val in _choice) yield return (val.Item1, val.Item2, this);
        _choice = null;
    }

    public void Choose(object sender, string c)
    {
        _choice = Options[c];
        GD.Print(_choice);
        DialogueContainer.ActiveDialogueContainer.HasChosen -= Choose;
    }
}