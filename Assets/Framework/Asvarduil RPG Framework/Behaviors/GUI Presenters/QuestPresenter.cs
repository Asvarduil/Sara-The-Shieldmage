using UnityEngine;
using System.Collections;

public class QuestPresenter : PresenterBase
{
    #region Variables / Properties

    public AsvarduilBox Background;
    public AsvarduilLabel QuestName;
    public AsvarduilLabel QuestDescription;
    public AsvarduilButton LastQuestButton;
    public AsvarduilButton NextQuestButton;

    private int _questId = 0;
    private SequenceManager _sequences;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _sequences = SequenceManager.Instance;

        base.Start();
    }

    public override void SetVisibility(bool isVisible)
    {
        float opacity = DetermineOpacity(isVisible);

        Background.TargetTint.a = opacity;
        QuestName.TargetTint.a = opacity;
        QuestDescription.TargetTint.a = opacity;
        LastQuestButton.TargetTint.a = opacity;
        NextQuestButton.TargetTint.a = opacity;
    }

    public override void DrawMe()
    {
        Background.DrawMe();
        QuestName.DrawMe();
        QuestDescription.DrawMe();

        if(LastQuestButton.IsClicked())
        {
            _maestro.PlayOneShot(ButtonSound);
            LoadLastQuest();
        }

        if(NextQuestButton.IsClicked())
        {
            _maestro.PlayOneShot(ButtonSound);
            LoadNextQuest();
        }
    }

    public override void Tween()
    {
        Background.Tween();
        QuestName.Tween();
        QuestDescription.Tween();
        LastQuestButton.Tween();
        NextQuestButton.Tween();
    }

    #endregion Hooks

    #region Methods

    public void ShowPresenter()
    {
        QuestState quest;

        if(_sequences.QuestStates.Count == 0)
            quest = new QuestState
            {
                QuestName = "The Quest Log Is Empty!",
                QuestDetails = "Talk to people around the world to find a quest."
            };
        else
            quest = _sequences.QuestStates[_questId];

        UpdatePresenter(quest);
        SetVisibility(true);
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
        QuestName.Text = quest.QuestName;
        QuestDescription.Text = quest.QuestDetails;
    }

    #endregion Methods
}
