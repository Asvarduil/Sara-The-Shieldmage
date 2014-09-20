using UnityEngine;
using System.Collections;

public class TreasureEntity : EntityTextInterface
{
	#region Variables / Properties

	public bool IsTaken = false;
	public string TreasureName;
	public string TakenThreadName;
	public string ItemGetThreadName;
	public string OpenChestAnimation;
	public string ClosedChestAnimation;

	private AsvarduilSpriteSystem _spriteSystem;
	private TreasureManager _treasureManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_treasureManager = TreasureManager.Instance;
		_spriteSystem = GetComponentInChildren<AsvarduilSpriteSystem>();

		LoadInitialState();
	}

	#endregion Engine Hooks

	#region Methods

	public void LoadInitialState()
	{
		IsTaken = _treasureManager.HasObtainedTreasure(TreasureName);
		string animation = IsTaken
			? OpenChestAnimation
			: ClosedChestAnimation;

		_spriteSystem.PlaySingleFrame(animation);
	}

	public override void DrawMe()
	{
		if(InteractButton.IsClicked()
		   || _controlManager.GetAxisUp("Interact") > 0)
		{
			_maestro.PlayOneShot(ButtonSound);

			CanActivate = false;
			SetVisibility(false);

			TextThread thread;
			if(! IsTaken)
			{
				DebugMessage("The item is available...taking it.");

				IsTaken = true;
				_spriteSystem.PlaySingleFrame(OpenChestAnimation);
				_treasureManager.MarkTreasureAsObtained(TreasureName);

				thread = _entityText.GetThreadByName(ItemGetThreadName);
				if(thread == default(TextThread))
					DebugMessage("The entity text has no thread named " + ItemGetThreadName);
			}
			else
			{
				DebugMessage("There is no item in the chest.");
				thread = _entityText.GetThreadByName(TakenThreadName);

				if(thread == default(TextThread))
					DebugMessage("The entity text has no thread named " + TakenThreadName);
			}

			if(thread == default(TextThread))
				DebugMessage("Could not find a thread that corresponds to the treasure's state!", LogLevel.LogicError);
			else
				_dialogueController.PresentTextThread(thread);
		}
	}

	#endregion Methods
}
