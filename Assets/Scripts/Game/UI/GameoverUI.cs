﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
    public GameObject WinBanner;
    public GameObject LoseBanner;

    public void SetGameOverBanner(bool isWinner)
    {
        if (isWinner)
            WinBanner.SetActive(true);
        else
            LoseBanner.SetActive(false);
    }
}