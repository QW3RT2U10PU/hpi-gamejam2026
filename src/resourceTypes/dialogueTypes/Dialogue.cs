using Godot;
using System.Collections;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class Dialogue : Resource, IEnumerable<(string, ICollection<string>)>
{
    /// <summary>
    /// Time for one character to be printed
    /// </summary>
    [Export]
    public virtual double TalkingTime { get; set; } = 0.025;
    
    /// <summary>
    /// Sound played when a character is printed
    /// </summary>
    [Export]
    public virtual Resource TalkingSound { get; set; }
    
    [Export]
    public virtual string Note { get; set; }
    
    [Export]
    public virtual int Octave { get; set; }
    
    public abstract IEnumerator<(string, ICollection<string>)> GetEnumerator();
    //{
    //    foreach (string s in lines) yield return s;
    //}
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
