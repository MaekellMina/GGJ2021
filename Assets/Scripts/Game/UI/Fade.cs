using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    void Update()
    {
		GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
		if (GetComponent<CanvasGroup>().alpha <= 0)
			Destroy(gameObject);
    }
}
