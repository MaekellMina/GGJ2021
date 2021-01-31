using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
	public SpriteRenderer spriteRenderer { set; get; }
	public bool taken { set; get; }

    public void ResetAnchor()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		taken = false;
	}
}
