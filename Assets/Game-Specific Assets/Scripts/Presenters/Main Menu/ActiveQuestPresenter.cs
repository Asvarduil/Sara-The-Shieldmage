using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveQuestPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text QuestName;
    public Text QuestDescription;

    private int _questId = 0;
    private SequenceManager _sequences;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _sequences = SequenceManager.Instance;
    }

    #endregion Hooks

    #region Methods

    public void ShowPresenter()
    {
        QuestState quest;

        if (_sequences.QuestStates.Count == 0)
            quest = new QuestState
            {
                QuestName = "The Quest Log Is Empty!",
                QuestDetails = "Talk to people around the world to find a quest."
            };
        else
            quest = _sequences.QuestStates[_questId];

        UpdatePresenter(quest);
    }

    private void LoadLastQuest()
    {
        _questId--;
        if (_questId < 0)
            _questId = _sequences.QuestStates.Count - 1;

        ShowPresenter();
    }

    private void LoadNextQuest()
    {
        _questId++;
        if (_questId > _sequences.QuestStates.Count - 1)
            _questId = 0;

        ShowPresenter();
    }

    private void UpdatePresenter(QuestState quest)
    {
        QuestName.text = quest.QuestName;
        QuestDescription.text = quest.QuestDetails;
    }


    #endregion Methods
}
