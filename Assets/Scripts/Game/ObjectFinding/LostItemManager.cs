using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class LostItemManager : MonoBehaviour
{
    internal static LostItemManager instance;
    public LostItem[] lostItemPool;

    public void FindItem(int objectId)
    {
        LostItem item = lostItemPool.Single(x => x.itemId == objectId);
         Debug.Log(item.name);
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
}
