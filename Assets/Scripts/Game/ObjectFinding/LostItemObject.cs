using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostItemObject : MonoBehaviour
{
    public int itemId;

    public void OnMouseDown()
    {
        LostItemManager.instance.FindItem(itemId);
    }
}
