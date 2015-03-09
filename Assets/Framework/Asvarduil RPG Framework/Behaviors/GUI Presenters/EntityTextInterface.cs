using UnityEngine;
using System.Collections;

public class EntityTextInterface : DebuggableBehavior
{
	#region Variables / Properties

	public string AffectedTag = "Player";
    public string InteractText = "Talk";
	
	protected EntityText _entityText;
	protected ControlManager _controls;
	protected DialogueController _controller;

	#endregion Variables / Properties

	#region Engine Hooks

	public virtual void Start()
	{
		_entityText = GetComponent<EntityText>();
		_controller = DialogueController.Instance;
	}

    public virtual void OnInteraction()
    {
        _controller.PresentEntityText(_entityText);
    }

	public void OnTriggerEnter(Collider who)
	{
		if(who.tag != AffectedTag)
			return;

        _controller.PrepareInteraction(InteractText, () => OnInteraction());
	}

	public void OnTriggerExit(Collider who)
	{
		if(who.tag != AffectedTag)
			return;

        _controller.ClearInteraction();
	}

	#endregion Engine Hooks
}
