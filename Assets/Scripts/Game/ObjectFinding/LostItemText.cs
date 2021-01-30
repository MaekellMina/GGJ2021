using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostItemText : MonoBehaviour
{
	[SerializeField]
	private Text itemNameUI;
	[SerializeField]
	private Image crossoutUI;   

	public int AssignedID { set; get; }
	public bool CrossedOut { set; get; }

	private Coroutine crossOutCoroutine;

	public void AssignInfo(int id, string itemName)
	{
		AssignedID = id;
		itemNameUI.text = itemName;
		crossoutUI.gameObject.SetActive(false);
		CrossedOut = false;
	}

    public void CrossOut()
	{
		if (crossOutCoroutine != null)
			StopCoroutine(crossOutCoroutine);
		crossOutCoroutine = StartCoroutine(CrossOut_IEnum());
	}

    private IEnumerator CrossOut_IEnum()
	{
		float t = 0;
		CrossedOut = true;
		crossoutUI.fillAmount = 0;
		crossoutUI.gameObject.SetActive(true);
        while(t<1)
		{
			crossoutUI.fillAmount = Mathf.Lerp(0, 1, t);
			t += Time.deltaTime/0.4f;
			yield return null;
		}
		crossoutUI.fillAmount = 1;
	}
}
