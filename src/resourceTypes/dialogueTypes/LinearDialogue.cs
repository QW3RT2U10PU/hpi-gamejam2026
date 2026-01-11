using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class LinearDialogue : Dialogue
{
    [Export] public Godot.Collections.Array<Dialogue> Dialogues {get; set;} = [];

    public override IEnumerator<(string, ICollection<string>, Dialogue)> GetEnumerator()
    {
        foreach (Dialogue d in Dialogues)
        {
            foreach ((var a, var b, var c) in d) yield return (a, b, c);
        }
    }
}
