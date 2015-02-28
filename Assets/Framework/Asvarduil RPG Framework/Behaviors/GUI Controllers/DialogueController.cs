using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class DialogueController : ManagerBase<DialogueController>
{
	#region Variables / Properties
	
	public TextThread ActiveTextThread;
	public int TextIndex = 0;
	public int TextContentCount = 0;

	private ControlManager _controlManager;
	//private PauseController _pauseController;
	private DialoguePresenter _dialogueGUI;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_controlManager = ControlManager.Instance;
		//_pauseController = PauseController.Instance;
		_dialogueGUI = GetComponentInChildren<DialoguePresenter>();
	}

	#endregion Engine Hooks

	#region Methods

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
		
		if(TextIndex > TextContentCount - 1)
		{
			EndConversation();
		}
		else
		{
			_dialogueGUI.PresentText(ActiveTextThread.TextContent[TextIndex]);
		}
	}

	#endregion Methods
}
