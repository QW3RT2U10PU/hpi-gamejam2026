using GameJam;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public partial class DialogueContainer : PanelContainer
{
    public static DialogueContainer ActiveDialogueContainer { get; private set; }

    public EventHandler<string> HasChosen;

    private IEnumerator<(string, ICollection<string>, Dialogue)> _dialogueIterator;

    /// <summary>
    /// when false, blocks advancing to the next line (e.g. during choices)
    /// </summary>
    private bool _canContinue = true;

    /// <summary>
    /// stores the player that triggered the dialogue IF it has an active interaction indicator, null else
    /// </summary>
    private Player _storedInteractor;

    public Dialogue CurrentDialogue
    {
        set
        {
            _dialogueIterator = value.GetEnumerator();

            Enable();
            NextLine();
        }
    }

    private void NextLine()
    {
        if (!_dialogueIterator.MoveNext())
        {
            Disable();
            return;
        }

        (string text, var choices, var dialogue) = _dialogueIterator.Current;

        if (choices != null)
        {
            _canContinue = false;
            LoadChoices(choices);
            return;
        }

        var timer = GetNode<Timer>("%SansTimer");
        var label = GetNode<RichTextLabel>("%Text");

        var characterMatch = Regex.Match(text, @"^\[([^\]]+)\]");

        label.VisibleCharacters = characterMatch.Success ? characterMatch.Length : 0;
        label.Text = text.Trim();

        var sampler = GetNode<GodotObject>("%SansHehehehehe");
        sampler.Set("env_sustain", dialogue.TalkingTime);

        timer.OneShot = true;
        timer.Timeout += () =>
        {
            if (dialogue.Note != "" && dialogue.TalkingSound != null) 
                sampler.Call("x_play_note", dialogue.TalkingSound, dialogue.Note, dialogue.Octave);

            label.VisibleCharacters += 1;
            if (label.VisibleCharacters < text.Trim().Length) timer.Start(dialogue.TalkingTime);
        };

        timer.Start(dialogue.TalkingTime);
    }


    public void LoadChoices(ICollection<string> options)
    {
        GetNode<TabContainer>("%TextOrChoice").CurrentTab = 1;
        var choiceContainer = GetNode<Container>("%ChoiceOptions");
        foreach (var n in choiceContainer.GetChildren()) n.QueueFree();
        foreach (var s in options.Order())
        {
            Button choice = new();
            choice.Text = s;
            choice.Pressed += () =>
            {
                HasChosen?.Invoke(this, s);
                foreach (Node n in choiceContainer.GetChildren()) n.QueueFree();
                GetNode<TabContainer>("%TextOrChoice").CurrentTab = 0;
                _canContinue = true;
                NextLine();
            };
            choiceContainer.AddChild(choice);
        }
    }


    public void Enable()
    {
        Player p = GetOwner<Level>().GetNode<Player>("%Player");
        _storedInteractor = p.GetNode<InteractionIndicator>("InteractionIndicator").Enabled ? p : null;
        p.NotifyInteractable(false);

        ActiveDialogueContainer = this;
        Visible = true;
        ProcessMode = ProcessModeEnum.Always;
        GetTree().Paused = true;
        GetNode<RichTextLabel>("%Text").VisibleCharacters = 0;
    }

    public void Disable()
    {
        _storedInteractor?.NotifyInteractable(true);
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
            if (_canContinue) NextLine();
        }
    }
}