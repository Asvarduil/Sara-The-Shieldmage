using System;
using UnityEngine;

using UnityObject = UnityEngine.Object;

public abstract class AsvarduilFrame<T>
{
	#region Variables / Properties

	public T Content;
	public float AnimationDelay = 0.042f;

	#endregion Variables / Properties
}
