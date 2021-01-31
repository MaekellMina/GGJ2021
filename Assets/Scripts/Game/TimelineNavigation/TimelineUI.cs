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
	[SerializeField]
	private GameObject pauseButton;
	[SerializeField]
	private GameObject playButton;

	public bool IsPaused { set; get; }

	private float axisWidth;
	private float intervalWidth;
	private int curIntervalIndex;
	private Coroutine timeTrackingCoroutine;

	private const int NUM_INTERVALS = 48;

	private void Start()
	{
		axisWidth = axis.rect.width;
		intervalWidth = axisWidth / NUM_INTERVALS;
	}

	public void StartTracking()
	{
		IsPaused = false;
		playButton.SetActive(false);
		pauseButton.SetActive(true);
		timeTrackerPin.anchoredPosition3D = Vector3.zero;
		curIntervalIndex = 0;
		if (timeTrackingCoroutine != null)
			StopCoroutine(timeTrackingCoroutine);
		timeTrackingCoroutine = StartCoroutine(TimeTracking_IEnum());
	}

    public void StartTrackingOnIndex(int startIndex)
	{
		AudioManager.instance.PlayAudioClip(5);
		IsPaused = false;
        playButton.SetActive(false);
        pauseButton.SetActive(true);
		timeTrackerPin.anchoredPosition3D = Vector3.right * (startIndex * intervalWidth);
		curIntervalIndex = startIndex;
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
			AudioManager.instance.PlayAudioClip(4);
			while (t < 1)  
			{
				if (GameManager.instance.IsPlayState() && !IsPaused)
				{
					timeTrackerPin.anchoredPosition3D = Vector3.Lerp(Vector3.right * startX, Vector3.right * destinationX, t);
					t += Time.deltaTime / timeIntervalPerFrame;
				}
				yield return null;
			}
			timeTrackerPin.anchoredPosition3D = Vector3.right * destinationX;
			curIntervalIndex++;
			if (curIntervalIndex >= NUM_INTERVALS)
				curIntervalIndex = 0;
			yield return null;
		}
	}

    public void PauseAudio()
	{
		AudioManager.instance.PlayAudioClip(5);
	}
}
