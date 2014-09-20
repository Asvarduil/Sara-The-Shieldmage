using System;
using UnityEngine;

[Serializable]
public class AsvarduilAccelerometer
{
	#region Variables / Properties
	
	public float UpdateInterval = (1.0f / 60.0f);
	public float LowPassKernelWidth = 1.0f;

	public float LowPassFilterFactor
	{
		get { return UpdateInterval / LowPassKernelWidth; }
	}

	private Vector3 _baseAcceleration;

	#endregion Variables / Properties

	#region Methods

	public void Initialize()
	{
		_baseAcceleration = Input.acceleration;
	}

	public static Vector3 GetAccelerometerRaw()
	{
		return Input.acceleration;
	}

	public Vector3 GetAccelerometerSmooth()
	{
		return Vector3.Lerp(_baseAcceleration, Input.acceleration, LowPassFilterFactor * Time.deltaTime);
	}

	public static Vector3 GetAccelerometerPrecise()
	{
		float samplingPeriod = 0.0f;
		Vector3 acceleration = Vector3.zero;

		// TODO: Revise to For loop to avoid allocations...
		foreach(AccelerationEvent accelEvent in Input.accelerationEvents)
		{
			acceleration += accelEvent.acceleration * Time.deltaTime;
			samplingPeriod += Time.deltaTime;
		}

		if(samplingPeriod > 0.0f)
			acceleration *= 1.0f / samplingPeriod;

		return acceleration;
	}

	#endregion Methods
}
