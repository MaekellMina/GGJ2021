using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameObject : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] backgrounds;
	[SerializeField]
	private SpriteRenderer[] entities;

	private Coroutine fadeCoroutine;

	public void Show()
	{
		gameObject.SetActive(true);
		if (fadeCoroutine != null)
			StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(Fade_IEnum(1));
		
	}

    public void Hide()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(Fade_IEnum(0));
    }

    public void ResetFrame()
	{
		for (int i = 0; i < backgrounds.Length; i++)
		{
			backgrounds[i].color = new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, 0);
		}

		for (int i = 0; i < entities.Length; i++)
		{
			entities[i].color = new Color(entities[i].color.r, entities[i].color.g, entities[i].color.b, 0);
		}

		gameObject.SetActive(false);
	}
    
    private IEnumerator Fade_IEnum(int targetAlpha)
	{
		float t = 0;
		float startAlpha = backgrounds[0].color.a;
         
        while(t<1)
		{
			for (int i = 0; i < backgrounds.Length; i++)
			{
				backgrounds[i].color = Color.Lerp(new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, startAlpha),
				                                  new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, targetAlpha),
												  t);
			}
			for (int i = 0; i < entities.Length; i++)
            {
                entities[i].color = Color.Lerp(new Color(entities[i].color.r, entities[i].color.g, entities[i].color.b, startAlpha),
				                               new Color(entities[i].color.r, entities[i].color.g, entities[i].color.b, targetAlpha),
                                                  t);
            }
			t += Time.deltaTime / 0.7f;
			yield return null;
		}

		for (int i = 0; i < backgrounds.Length; i++)
        {
			backgrounds[i].color = new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, targetAlpha);
        }
        for (int i = 0; i < entities.Length; i++)
        {
			entities[i].color = new Color(entities[i].color.r, entities[i].color.g, entities[i].color.b, targetAlpha);
        }

		if (targetAlpha == 0)
			gameObject.SetActive(false);
	}

}
