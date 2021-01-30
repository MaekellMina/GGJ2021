using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public GameObject[] xLife;
    private int currentX;
    private bool dead;

    void Start()
    {
        currentX = 0;

        foreach (var life in xLife)
            life.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
            Debug.Log("u dead");
        else if (Input.GetKeyDown(KeyCode.A))
            IncreaseXLives();
        
    }

    public void IncreaseXLives()
    {
        xLife[currentX].SetActive(true);
        currentX++;

        if (currentX == 5)
                dead = true;
    }
}
