using System;
using UnityEngine;

[Serializable]
public class AsvarduilControlAxis
{
	#region Variables / Properties

	public string Name;
	public string PositiveKey;
	public string NegativeKey;
	public bool IsInverted = false;
	public bool Sharp = false;
	public float DeadZone = 0.1f;
	public float Sensitivity = 1.0f;

	private float _value;

	#endregion Variables / Properties

	#region Methods

	[Obsolete("Use IsPositive() instead!")]
	public bool GetPositiveKey()
	{
		return IsPositive();
	}

	[Obsolete("Use IsNegative() instead!")]
	public bool GetNegativeKey()
	{
		return IsNegative();
	}

	public float GetAxis()
	{
		switch(Application.platform)
		{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsWebPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.OSXDashboardPlayer:
			case RuntimePlatform.LinuxPlayer:
				return GetKey();

			case RuntimePlatform.BB10Player:
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
			case RuntimePlatform.WP8Player:
				throw new Exception("Key-based Axes not supported on mobile platforms!");

			default:
				throw new Exception("Platform not supported: " + Application.platform);
		}
	}

	public bool PositiveKeyDown()
	{
		return !string.IsNullOrEmpty(PositiveKey) && Input.GetKeyDown(PositiveKey);
	}

	public bool NegativeKeyDown()
	{
		return !string.IsNullOrEmpty(NegativeKey) && Input.GetKeyDown(NegativeKey);
	}

	public float GetKeyDown()
	{
		if(PositiveKeyDown())
		{
			PositiveAxisCalculation();
		}
		else if(NegativeKeyDown())
		{
			NegativeAxisCalculation();
		}
		else
		{
			_value = 0;
		}
		
		return DeadZoneCalculation(_value);
	}

	public bool PositiveKeyUp()
	{
		return !string.IsNullOrEmpty(PositiveKey) && Input.GetKeyUp(PositiveKey);
	}
	
	public bool NegativeKeyUp()
	{
		return !string.IsNullOrEmpty(NegativeKey) && Input.GetKeyUp(NegativeKey);
	}

	public float GetKeyUp()
	{
		if(PositiveKeyUp())
		{
			PositiveAxisCalculation();
		}
		else if(NegativeKeyUp())
		{
			NegativeAxisCalculation();
		}
		else
		{
			_value = 0;
		}
		
		return DeadZoneCalculation(_value);
	}

	public bool IsPositive()
	{
		return !string.IsNullOrEmpty(PositiveKey) && Input.GetKey(PositiveKey);
	}
	
	public bool IsNegative()
	{
		return !string.IsNullOrEmpty(NegativeKey) && Input.GetKey(NegativeKey);
	}

	private float GetKey()
	{
		if(IsPositive())
		{
			PositiveAxisCalculation();
		}
		else if(IsNegative())
		{
			NegativeAxisCalculation();
		}
		else
		{
			_value = 0;
		}
		
		return DeadZoneCalculation(_value);
	}

	private void PositiveAxisCalculation()
	{
		_value = AxisCalculation();
	}

	private void NegativeAxisCalculation()
	{
		_value = -(AxisCalculation());
	}

	private float AxisCalculation()
	{
		float value = Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity);
		if(IsInverted)
			value = -value;
		
		return value;
	}

	private float DeadZoneCalculation(float moveValue)
	{
		bool isInDeadZone = (moveValue > -1 * DeadZone) && (moveValue < DeadZone);
		return isInDeadZone ? 0.0f : moveValue;
	}

	#endregion Methods
}
