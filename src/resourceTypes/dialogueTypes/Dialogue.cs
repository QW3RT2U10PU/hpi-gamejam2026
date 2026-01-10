using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class Dialogue : Resource, IEnumerable<(string, ICollection<string>)>
{
    public abstract IEnumerator<(string, ICollection<string>)> GetEnumerator();
    //{
    //    foreach (string s in lines) yield return s;
    //}

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
