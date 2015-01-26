using UnityEngine;
using System.Collections;

public class SuspendBattleTrigger : MonoBehaviour
{
    #region Variables / Properties

    public string RecognizedTag = "Player";
    public bool BattlesSuspended = false;

    private float _oldBattleRate;
    private BattleManager _manager;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _manager = BattleManager.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        if (who.tag != RecognizedTag)
            return;

        _oldBattleRate = _manager.BattleRate;
        _manager.BattleRate = 0.0f;
        BattlesSuspended = true;
    }

    public void OnTriggerExit(Collider who)
    {
        if (who.tag != RecognizedTag)
            return;

        _manager.BattleRate = _oldBattleRate;
        BattlesSuspended = false;
    }

    #endregion Hooks
}
