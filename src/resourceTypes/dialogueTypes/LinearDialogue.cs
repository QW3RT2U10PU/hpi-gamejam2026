using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class LinearDialogue : Dialogue
{
    [Export] public Godot.Collections.Array<Dialogue> Dialogues {get; set;} = [];

    public override IEnumerator<(string, ICollection<string>)> GetEnumerator()
    {
        foreach (Dialogue d in Dialogues)
        {
            foreach (var val in d) yield return val;
        }
    }
}
