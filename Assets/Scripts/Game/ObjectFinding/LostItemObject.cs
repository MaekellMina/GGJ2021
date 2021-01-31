using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostItemObject : MonoBehaviour
{
    public int itemId;
	public string itemName;
	public bool isStatic;
	public AnchorPoint[] possibleAnchorPoints;
	public LostItemObject reference;
	public GameObject correct;

	private void Start()
	{
		if(reference != null)
		    possibleAnchorPoints = reference.possibleAnchorPoints;
	}

	public void OnMouseDown()
    {
        LostItemManager.instance.FindItem(itemId);
    }

}
