using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[GlobalClass]
public partial class ChoiceDialogue : Dialogue
{
    [Export] public Godot.Collections.Dictionary<string, Dialogue> Options = [];

    private Dialogue choice = null;

    public override IEnumerator<(string, ICollection<string>)> GetEnumerator()
    {
        DialogueContainer.ActiveDialogueContainer.HasChoosen += Choose;
        yield return (null, Options.Keys);
        foreach (var val in choice) yield return val;
        choice = null;
    }

    public void Choose(object sender, string c)
    {
        choice = Options[c];
        GD.Print(choice);
        DialogueContainer.ActiveDialogueContainer.HasChoosen -= Choose;
    }
}
