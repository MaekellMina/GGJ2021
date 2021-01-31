using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class LostItemManager : MonoBehaviour
{
    internal static LostItemManager instance;
    public LostItemObject[] lostItemPool;
	public Transform lostItemPoolParent;
    public Transform lostItemListParent;
	public GameObject lostItemTextPrefab;

    [SerializeField]
    private int numAdditionalItemsToDisplay;
	[SerializeField]
	private int numItemsToFind;

	public GameObject wrongFeedbackPrefab;
	public Transform wrongFeedbackParent;

	public AnchorPoint[] anchorPoints;

	private List<int> itemIdsToDisplay;
	private List<int> itemIdsToFind;
	private List<LostItemText> lostItemTexts;
	public int ItemsFound { set; get; }
      
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return; 
        }
        instance = this;
    }
    
	private void Start()
	{
		for (int i = 0; i < 12; i++)
		{
			GameObject instance = Instantiate(lostItemTextPrefab) as GameObject;
			CacheManager.Store("LostItemText", instance);
		}
	}

	public void CreateItemsList()
	{
		for (int i = 0; i < anchorPoints.Length; i++)
		{
			anchorPoints[i].ResetAnchor();
		}
		
		for (int i = 0; i < lostItemPool.Length; i++)
		{
			lostItemPool[i].transform.SetParent(lostItemPoolParent, false);
			lostItemPool[i].gameObject.SetActive(false);
			lostItemPool[i].correct.SetActive(false);
		}

		itemIdsToDisplay = new List<int>();

		List<LostItemObject> lostItemPoolCopy = lostItemPool.ToList();
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
			AnchorPoint anchorPoint = null;
			while(anchorPoint == null) 
			{
				AnchorPoint x = lostItemPoolCopy[randomIndex].possibleAnchorPoints[Random.Range(0, lostItemPoolCopy[randomIndex].possibleAnchorPoints.Length)];
				if(!x.taken)
				{
					anchorPoint = x;
					x.taken = true;
				}
			}
			lostItemPoolCopy[randomIndex].transform.SetParent(anchorPoint.transform,false);
			lostItemPoolCopy[randomIndex].transform.localPosition = Vector3.zero;
			lostItemPoolCopy[randomIndex].GetComponent<SpriteRenderer>().color = new Color(anchorPoint.GetComponent<SpriteRenderer>().color.r, anchorPoint.GetComponent<SpriteRenderer>().color.g, anchorPoint.GetComponent<SpriteRenderer>().color.b, 1);
			lostItemPoolCopy[randomIndex].GetComponent<SpriteRenderer>().sortingOrder = anchorPoint.GetComponent<SpriteRenderer>().sortingOrder;
			lostItemPoolCopy[randomIndex].gameObject.SetActive(true);
			anchorPoint.frame.dynamicEntities.Add(lostItemPoolCopy[randomIndex].GetComponent<SpriteRenderer>());
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

		if (lostItemTexts != null)
		{
			for (int i = lostItemTexts.Count - 1; i >= 0; i--)
			{
				CacheManager.Store("LostItemText", lostItemTexts[i].gameObject);
			}
		}

		lostItemTexts = new List<LostItemText>();
		foreach (var itemId in itemIdsToFind)
        {
            var itemName = lostItemPool.Single(x => x.itemId == itemId).itemName;

			GameObject lostItemText = CacheManager.ActivateRandom("LostItemText");
            lostItemText.transform.SetParent(lostItemListParent, false);
			lostItemText.transform.position = lostItemListParent.position;
            lostItemText.transform.localScale = Vector3.one;
			lostItemText.GetComponent<LostItemText>().AssignInfo(itemId, itemName);
			lostItemTexts.Add(lostItemText.GetComponent<LostItemText>());
        }

		ItemsFound = 0;
	}

	public void FindItem(int itemId)
    {
		if (!GameManager.instance.IsPlayState())
			return;
		
		if(itemIdsToFind.Contains(itemId))
		{
			LostItemText itemText = lostItemTexts.Single(x => x.AssignedID == itemId);
			if (!itemText.CrossedOut)
			{
				LostItemObject item = lostItemPool.Single(x => x.itemId == itemId);
				itemText.CrossOut();
				Debug.Log(item.itemName);
				item.correct.SetActive(true);
				ItemsFound++;

				if(ItemsFound>= numItemsToFind)
				{
					GameManager.instance.b_pass = true;
					GameManager.instance.ChangeGameState(GameManager.GAMESTATES.GAMEOVER);
				}
			}
		}
		else
		{
			//wrong
			GameManager.instance.livesUI.DecreaseLives();

			GameObject wrongFeedbackInstance = Instantiate(wrongFeedbackPrefab, wrongFeedbackParent) as GameObject;
			wrongFeedbackInstance.GetComponent<RectTransform>().anchoredPosition3D = Input.mousePosition;
		}
    }

	public int GetNumItemsToFind()
	{
		return numItemsToFind;
	}
}
