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

	private const float _activateLockout = 0.25f;
	private float _lastActivation;

	private AsvarduilSpriteSystem _spriteSystem;
	private TreasureManager _treasureManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_treasureManager = TreasureManager.Instance;
		_spriteSystem = GetComponentInChildren<AsvarduilSpriteSystem>();

        LoadChestState();
	}

    public override void OnInteraction()
    {
        TextThread interactThread;
        if (IsTaken)
            interactThread = _entityText.GetThreadByName(TakenThreadName);
        else
            interactThread = _entityText.GetThreadByName(ItemGetThreadName);

        // On interact, open the chest.
        _spriteSystem.PlaySingleFrame(OpenChestAnimation);

        _controller.PresentTextThread(interactThread);
    }

	#endregion Engine Hooks

	#region Methods

	public void LoadChestState()
	{
		IsTaken = _treasureManager.HasObtainedTreasure(TreasureName);
		string animation = IsTaken
			? OpenChestAnimation
			: ClosedChestAnimation;

        InteractText = IsTaken ? "Open" : "Examine";

		_spriteSystem.PlaySingleFrame(animation);
	}

	#endregion Methods
}
