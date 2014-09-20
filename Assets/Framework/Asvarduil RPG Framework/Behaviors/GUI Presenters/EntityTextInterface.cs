using UnityEngine;
using System.Collections;

public class EntityTextInterface : PresenterBase, ISuspendable
{
	#region Variables / Properties

	public bool CanActivate = false;
	public string AffectedTag = "Player";
	public AsvarduilButton InteractButton;
	
	protected EntityText _entityText;
	protected ControlManager _controlManager;
	protected DialogueController _dialogueController;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_entityText = GetComponent<EntityText>();
		_controlManager = ControlManager.Instance;
		_dialogueController = DialogueController.Instance;
	}

	public override void OnGUI()
	{
		if(! CanActivate)
			return;

		base.OnGUI();
	}

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
		InteractButton.TargetTint.a = opacity;

		CanActivate = isVisible;
	}

	public void OnTriggerEnter(Collider who)
	{
		if(who.tag != AffectedTag)
			return;

		SetVisibility(true);
	}

	public void OnTriggerExit(Collider who)
	{
		if(who.tag != AffectedTag)
			return;

		SetVisibility(false);
	}
	
	public override void DrawMe()
	{
		if(InteractButton.IsClicked()
		   || _controlManager.GetAxisUp("Interact") > 0)
		{
			_maestro.PlayOneShot(ButtonSound);

			CanActivate = false;
			SetVisibility(false);

			_dialogueController.PresentEntityText(_entityText);
		}
	}

	public override void Tween()
	{
		InteractButton.Tween();
	}

	public void Suspend()
	{
		CanActivate = false;
	}

	public void Resume()
	{
		CanActivate = true;
	}

	#endregion Engine Hooks
}
