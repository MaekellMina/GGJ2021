using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
	public FrameObject frame;
	public SpriteRenderer spriteRenderer { set; get; }
	public bool taken;

    public void ResetAnchor()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		taken = false;
		frame.dynamicEntities.Clear();
	}
}
