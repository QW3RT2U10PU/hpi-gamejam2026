using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class DialogueLines : Dialogue
{
    [Export] public Godot.Collections.Array<string> Lines {get; set;} = [];

    public override IEnumerator<(string, ICollection<string>, Dialogue)> GetEnumerator()
    {
        foreach (string s in Lines) yield return (s, null, this);
    }
}
