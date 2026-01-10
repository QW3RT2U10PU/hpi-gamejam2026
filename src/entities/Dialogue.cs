using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

[GlobalClass]
public partial class Dialogue : Resource, IEnumerable<string>
{
    [Export]
    Godot.Collections.Array<string> lines;

    public IEnumerator<string> GetEnumerator()
    {
        foreach (string s in lines) yield return s;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
