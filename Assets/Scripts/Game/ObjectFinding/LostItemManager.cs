using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class LostItemManager : MonoBehaviour
{
    internal static LostItemManager instance;
    public LostItem[] lostItemPool;
    public Transform lostItemListParent;
    public GameObject lostItemPrefab;
       
    [SerializeField]
    private int numAdditionalItemsToDisplay;
	[SerializeField]
	private int numItemsToFind;

	private List<int> itemIdsToDisplay;
	private List<int> itemIdsToFind;
      
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return; 
        }
        instance = this;
        CreateItemsList();
    }

	public void CreateItemsList()
	{
		itemIdsToDisplay = new List<int>();

		List<LostItem> lostItemPoolCopy = lostItemPool.ToList();
		int count = lostItemPoolCopy.Count;

        //add all static items first
		for (int i = count - 1; i >= 0; i--)
		{
			if (lostItemPoolCopy[i].isStatic)
			{
				itemIdsToDisplay.Add(lostItemPoolCopy[i].itemId);
				lostItemPoolCopy.RemoveAt(i);
			}
		}

        //add random items
		for (int i = 0; i < numAdditionalItemsToDisplay; i++)
		{
			int randomIndex = Random.Range(0, lostItemPoolCopy.Count);
			itemIdsToDisplay.Add(lostItemPoolCopy[randomIndex].itemId);
			lostItemPoolCopy.RemoveAt(randomIndex);
		}
        
		//generate list of items to find
		itemIdsToFind = new List<int>();
		List<int> itemIdsToDisplayCopy = itemIdsToDisplay.ToArray().ToList();
		for (int i = 0; i < numItemsToFind; i++)
		{
			int randomIndex = Random.Range(0, itemIdsToDisplayCopy.Count);
			itemIdsToFind.Add(itemIdsToDisplayCopy[randomIndex]);
			itemIdsToDisplayCopy.RemoveAt(randomIndex);
		}

        foreach (var item in itemIdsToFind)
        {
            var itemName = lostItemPool.Single(x => x.itemId == item).name;
            
            GameObject instance = Instantiate(lostItemPrefab) as GameObject;
            instance.transform.SetParent(lostItemListParent);
            instance.transform.localScale = Vector3.one;
            instance.GetComponent<Text>().text = itemName;
        }

	}

	public void FindItem(int objectId)
    {
        LostItem item = lostItemPool.Single(x => x.itemId == objectId);
        Debug.Log(item.name);
    }

}
