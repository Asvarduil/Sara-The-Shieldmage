using System;
using System.Collections.Generic;
using UnityEngine;

using UnityObject = UnityEngine.Object;

public abstract class AsvarduilAnimation<T>
{
	public string Name;
	public AsvarduilAnimationType AnimationType;
	public List<T> Frames;
}
