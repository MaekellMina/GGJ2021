using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    public GameObject WinBanner;
    public GameObject LoseBanner;
	public Text timeElapsedUI;

    public void SetGameOverBanner(bool isWinner, float timeElapsed)
    {
		if (isWinner)
		{
			WinBanner.SetActive(true);
			LoseBanner.SetActive(false);
			timeElapsedUI.gameObject.SetActive(true);
			timeElapsedUI.text = ((int)timeElapsed).ToString();
		}
		else
		{
			WinBanner.SetActive(false);
            LoseBanner.SetActive(true);
			timeElapsedUI.gameObject.SetActive(false);
		}
    }
}
