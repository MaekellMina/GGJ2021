using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineUI : MonoBehaviour
{
	[SerializeField]
	private float timeIntervalPerFrame;
	[SerializeField]
	private RectTransform timeTrackerPin;
	[SerializeField]
	private RectTransform axis;

	private float axisWidth;
	private float intervalWidth;
	private int curIntervalIndex;
	private Coroutine timeTrackingCoroutine;

	private void Start()
	{
		axisWidth = axis.rect.width;
		intervalWidth = axisWidth / 48;
	}

	public void StartTracking()
	{
		timeTrackerPin.anchoredPosition3D = Vector3.zero;
		curIntervalIndex = 0;
		if (timeTrackingCoroutine != null)
			StopCoroutine(timeTrackingCoroutine);
		timeTrackingCoroutine = StartCoroutine(TimeTracking_IEnum());
	}

    private IEnumerator TimeTracking_IEnum()
	{      
		while (true)
		{
			Debug.Log(timeTrackerPin.anchoredPosition3D.x);
			float t = 0;
			float startX = timeTrackerPin.anchoredPosition3D.x;
			if (startX >= axisWidth)
				startX = 0;
			float destinationX = startX + intervalWidth;
			FrameManager.instance.DisplayFrame(curIntervalIndex);
			while (t < 1)  
			{
				if (GameManager.instance.IsPlayState())
				{
					timeTrackerPin.anchoredPosition3D = Vector3.Lerp(Vector3.right * startX, Vector3.right * destinationX, t);
					t += Time.deltaTime / timeIntervalPerFrame;
				}
				yield return null;
			}
			timeTrackerPin.anchoredPosition3D = Vector3.right * destinationX;
			curIntervalIndex++;
			yield return null;
		}
	}
}
