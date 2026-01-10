using Godot;
using System.Collections.Generic;

public partial class DialogueContainer : PanelContainer
{
  private IEnumerator<string> dialogueIterator = null;
  public Dialogue CurrentDialogue {set
    {
      dialogueIterator = value.GetEnumerator();
      Enable();
      NextLine();
    }
  }

	public void NextLine()
  {
    if (dialogueIterator.MoveNext())
    {
      GetNode<RichTextLabel>("%Text").Text = dialogueIterator.Current;
    }
    else Disable();
  }

  public void Enable()
  {
    Visible = true;
    ProcessMode = ProcessModeEnum.Always;
    GetTree().Paused = true;
  }

  public void Disable()
  {
    Visible = false;
    ProcessMode = ProcessModeEnum.Disabled;
    GetTree().Paused = false;
  }

  public override void _Input(InputEvent @event)
  {
	  if (@event.IsActionPressed("interact"))
	  {
      NextLine();
      GetViewport().SetInputAsHandled();
	  }
  }
}
