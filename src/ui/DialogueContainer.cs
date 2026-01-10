using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DialogueContainer : PanelContainer
{
  public static DialogueContainer ActiveDialogueContainer {get; private set;} = null;

  public EventHandler<string> HasChoosen;

  private IEnumerator<(string, ICollection<string>)> dialogueIterator = null;

  /// <summary>
  /// when false, blocks advancing to the next line (e. g. during choices)
  /// </summary>
  private bool canContinue = true;

  public Dialogue CurrentDialogue {set
    {
      dialogueIterator = value.GetEnumerator();
      Enable();
      NextLine();
    }
  }

	private void NextLine()
  {
    if (dialogueIterator.MoveNext())
    {
      (string text, var choice) = dialogueIterator.Current;
      if (choice == null) GetNode<RichTextLabel>("%Text").Text = text;
      else
      {
        canContinue = false;
        LoadChoices(choice);
      }
    }
    else Disable();
  }


  public void LoadChoices(ICollection<string> options)
  {
    GetNode<TabContainer>("%TextOrChoice").CurrentTab = 1;
    var choiceContainer = GetNode<Container>("%ChoiceOptions");
    foreach (Node n in choiceContainer.GetChildren()) n.QueueFree();
    foreach (string s in options.Order())
    {
      Button choice = new();
      choice.Text = s;
      choice.Pressed += () => {
        HasChoosen?.Invoke(this, s);
        foreach (Node n in choiceContainer.GetChildren()) n.QueueFree();
        GetNode<TabContainer>("%TextOrChoice").CurrentTab = 0;
        canContinue = true;
        NextLine();
      };
      choiceContainer.AddChild(choice);
    }
  }


  public void Enable()
  {
    ActiveDialogueContainer = this;
    Visible = true;
    ProcessMode = ProcessModeEnum.Always;
    GetTree().Paused = true;
  }

  public void Disable()
  {
    ActiveDialogueContainer = null;
    Visible = false;
    ProcessMode = ProcessModeEnum.Disabled;
    GetTree().Paused = false;
  }

  public override void _Input(InputEvent @event)
  {
	  if (@event.IsActionPressed("interact"))
	  {
      GetViewport().SetInputAsHandled();
      if (canContinue) NextLine();
	  }
  }
}
