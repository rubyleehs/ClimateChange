using System;
using System.Collections;
using UnityEngine;

public static class Animation
{
	public static IEnumerator Tween(float duration, Action<float> lerpFunction) => Tween(duration, lerpFunction, Linear);
	public static IEnumerator Tween(float duration, Action<float> lerpFunction, Func<float, float> timeFunction)
	{
		float currentTime = 0;
		while ((currentTime += Time.deltaTime) < duration)
		{
			lerpFunction(timeFunction(currentTime / duration));
			yield return null;
		}
	}

	public static float Linear(float t) => t;

	public static float EaseInQuad(float t) => t * t;
	public static float EaseOutQuad(float t) => t * (2f - t);
	public static float EaseInOutQuad(float t)
		=> ((t *= 2f) < 1f)
			? 0.5f * t * t
			: -0.5f * ((t -= 1f) * (t - 2f) - 1f);

	public static float EaseInCubic(float t) => t * t * t;
	public static float EaseOutCubic(float t) => 1f + ((t -= 1f) * t * t);
	public static float EaseInOutCubic(float t)
		=> ((t *= 2f) < 1f)
			? 0.5f * t * t * t
			: 0.5f * ((t -= 2f) * t * t + 2f);
}