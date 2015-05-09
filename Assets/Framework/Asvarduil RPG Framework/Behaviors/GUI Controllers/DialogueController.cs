using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class DialogueController : ManagerBase<DialogueController>
{
	#region Variables / Properties
	
	public TextThread ActiveTextThread;
	public int TextIndex = 0;
	public int TextContentCount = 0;

	private ControlManager _controlManager;
    private InteractionPresenter _interact;
	private DialoguePresenter _dialogueGUI;

    private List<DialogueEventHook> _eventFunctions;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_controlManager = ControlManager.Instance;
        _interact = GetComponentInChildren<InteractionPresenter>();
		_dialogueGUI = GetComponentInChildren<DialoguePresenter>();

        _eventFunctions = new List<DialogueEventHook>();
	}

    public void OnDestroy()
    {
        // Cleanup code.
        foreach(var hook in _eventFunctions)
        {
            if (hook == null)
                continue;

            hook.Function = null;
        }

        _eventFunctions = null;
    }

	#endregion Engine Hooks

	#region Methods

    public void RegisterEventHook(string eventName, Func<List<string>, IEnumerator> eventFunction)
    {
        DialogueEventHook newHook = new DialogueEventHook
        {
            Name = eventName,
            Function = eventFunction
        };

        _eventFunctions.Add(newHook);
        DebugMessage("Registered event '" + eventName + "'.");
    }

    public void PrepareInteraction(string interactText, Action onInteract)
    {
        _interact.SetInteractText(interactText);
        _interact.ReadyInteractEvent(onInteract);
        _interact.PresentGUI(true);
    }

    public void ClearInteraction()
    {
        _interact.PresentGUI(false);
    }

	public void PresentEntityText(EntityText entityText)
	{
		TextThread thread = entityText.GetCurrentTextThread();
		PresentTextThread(thread);
	}

	public void PresentTextThread(TextThread thread)
	{
		string message = string.Format("Activating thread {0}...", thread.Name);
		DebugMessage(message);

		_controlManager.RadiateSuspendCommand();

		ActiveTextThread = thread;
		TextIndex = 0;
		TextContentCount = thread.TextContent.Count;

		_dialogueGUI.PresentGUI(true);
		UpdateGUI();
	}

	public void EndConversation()
	{
		DebugMessage("Conversation has been ended by Dialogue Event.");

		ActiveTextThread = null;
		TextIndex = 0;
		TextContentCount = 0;

		_controlManager.RadiateResumeCommand();
		_dialogueGUI.PresentGUI(false);
	}

	public void AdvanceThread(int targetID)
	{
		TextIndex = targetID;
		UpdateGUI();
	}

	private void UpdateGUI()
	{
		if(ActiveTextThread == null)
			throw new Exception("The active text thread is unassigned, but trying to be advanced!");

        DebugMessage("TextIndex: " + TextIndex + "; TextContentCount: " + TextContentCount);

		if(TextIndex > TextContentCount - 1)
		{
            DebugMessage("No more text to present; ending dialogue thread.");
			EndConversation();
		}
		else
		{
			_dialogueGUI.PresentText(ActiveTextThread.TextContent[TextIndex]);
		}
	}

    public IEnumerator ExecuteDialogueEvent(string eventName, List<string> eventArgs)
    {
        DebugMessage("Controller is executing dialogue event: " + eventName + "...");

        // Find the first coroutine on the child behaviors with a name that matches the event name.
        DialogueEventHook coroutine = _eventFunctions.FirstOrDefault(f => f.Name == eventName);
        if (coroutine == default(DialogueEventHook))
        {
            DebugMessage("Could not find an event named " + eventName + " in the registered event list.");
            yield break;
        }

        DebugMessage(eventName + " is registered!  Doing it.");
        yield return StartCoroutine(coroutine.Function(eventArgs));
    }

	#endregion Methods
}
