using System.Collections.Generic;
using SimpleJSON;

public class NemesisManager : ManagerBase<NemesisManager>
{
    #region Variables / Properties

    public List<NemesisParty> NemesisParties;

    private DialogueController _dialogueController;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _dialogueController = DialogueController.Instance;
    }

    public void Update()
    {
        for(int i = 0; i < NemesisParties.Count; i++)
        {
            NemesisParty current = NemesisParties[i];
            //DebugMessage("Last Step At: " + current.LastStepStarted
            //             + "; Current Step Duration: " + current.CurrentObjectiveDuration
            //             + "; Next Step At: " + (current.LastStepStarted + current.CurrentObjectiveDuration));

            if (current.IsPlanStepComplete())
            {
                // Implementation A: Direct update...
                //var contingency = current.ProceedToPlanOutcome(NemesisPlanOutcome.Success);
                //StartCoroutine(_dialogueController.ExecuteGameEventGroup(contingency.Events));
                //DebugMessage("New action: " + current.CurrentObjective.Name);

                // Implementation B: Use "Dialogue Event" (refactor needs to happen...)
                DebugMessage("Tick!");

                var args = new List<string>
                {
                    current.NemesisPartyName
                };

                StartCoroutine(_dialogueController.ExecuteDialogueEvent("CompleteNemesisPlan", args));
            }
        }
    }

    #endregion Hooks

    #region Methods

    public NemesisParty GetNemesisPartyByName(string nemesisPartyName)
    {
        NemesisParty result = null;
        for(int i = 0; i < NemesisParties.Count; i++)
        {
            NemesisParty current = NemesisParties[i];
            if (current.NemesisPartyName == nemesisPartyName)
            {
                result = current;
                break;
            }
        }

        return result;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["NemesisParties"] = new JSONArray();
        for (int i = 0; i < NemesisParties.Count; i++)
        {
            NemesisParty current = NemesisParties[i];
            state["NemesisParties"].Add(current.ExportState());
        }

        return state;
    }

    #endregion Methods
}
