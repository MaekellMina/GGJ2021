using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
	internal static FrameManager instance;   // singleton instance

	[SerializeField]
	private FrameObject[] frames;

	private int curFrameIndex { set; get; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        //Initialize class
        instance = this;
    }

	public void ResetFrames()
	{
		curFrameIndex = 0;
		for (int i = 0; i < frames.Length; i++)
		{
			frames[i].ResetFrame();
		}
		 
	}

	public void DisplayFrame(int index)
	{
		if(index != curFrameIndex)
		{
			//hide
			frames[curFrameIndex].Hide();
		}

		curFrameIndex = index;
		frames[index].Show();
	}
}
