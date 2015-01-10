using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class SequenceManager : ManagerBase<SequenceManager>
{
	#region Variables / Properties

	public List<SequenceState> SequenceStates;
	public List<QuestState> QuestStates;

	#endregion Variables / Properties

	#region Methods

	public bool EvaluateRange(SequenceRange range)
	{
		bool result = SequenceStates.Any(s => s.Name == range.Name
		                                      && s.Counter >= range.MinCounter
		                                      && s.Counter <= range.MaxCounter);

		return result;
	}

	public SequenceState FindSequenceByName(string name)
	{
		SequenceState state = SequenceStates.FirstOrDefault(s => s.Name == name);
		if(state == default(SequenceState))
		{
			string message = string.Format("Sequence Manager has no thread named {0}.", name);
			DebugMessage(message, DebuggableBehavior.LogLevel.LogicError);
		}

		return state;
	}

	public QuestState FindQuestBySequenceName(string name)
	{
		QuestState state = QuestStates.FirstOrDefault(q => q.SequenceName == name);
		if(state == default(QuestState))
		{
			string message = string.Format("Sequence Manager has no quest tied to thread {0}.", name);
			DebugMessage(message, DebuggableBehavior.LogLevel.LogicError);
		}
		
		return state;
	}

	public void IncrementSequence(string name, int amount = 1)
	{
		SequenceState state = SequenceStates.FirstOrDefault(s => s.Name == name);
		if(state == default(SequenceState))
		{
			string message = string.Format("Sequence Manager has no thread named {0}.  Creating and initializing it.", name);
			DebugMessage(message, DebuggableBehavior.LogLevel.Warning);
            state = new SequenceState
            {
                Name = name,
                Counter = 0
            };
		}

		state.Counter++;

		DebugMessage("Raised thread " + name + " to state #" + state.Counter);
	}

    public void UpdateSequence(string name, int phase)
    {
        SequenceState state = SequenceStates.FirstOrDefault(s => s.Name == name);
        if (state == default(SequenceState))
        {
            string message = string.Format("Sequence Manager has no thread named {0}.  Creating and initializing it.", name);
            DebugMessage(message, DebuggableBehavior.LogLevel.Warning);
            state = new SequenceState
            {
                Name = name,
                Counter = phase
            };

            SequenceStates.Add(state);
        }

        state.Counter = phase;

        DebugMessage("Raised thread " + name + " to state #" + state.Counter);
    }

	public void UpdateQuest(string name, string questDetails)
	{
		QuestState state = QuestStates.FirstOrDefault(q => q.SequenceName == name);
		if(state == default(QuestState))
		{
			string message = string.Format("Sequence Manager has no quest for thread {0}.  Creating and initializing it.", name);
			DebugMessage(message, LogLevel.Warning);
            state = new QuestState
            {
                QuestName = name,
                SequenceName = name
            };

            QuestStates.Add(state);
		}

		state.QuestDetails = questDetails;

		DebugMessage("Updated quest for thread " + name + " with new details.");
	}

	#endregion Methods
}
