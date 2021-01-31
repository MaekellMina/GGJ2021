using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameObject : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] backgrounds;
	[SerializeField]
	private SpriteRenderer[] staticEntities;
	[SerializeField]
	public List<SpriteRenderer> dynamicEntities;
	[SerializeField]
	private SpriteRenderer filter;
	private float filterMaxOpacity = -1;

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

		for (int i = 0; i < staticEntities.Length; i++)
		{
			staticEntities[i].color = new Color(staticEntities[i].color.r, staticEntities[i].color.g, staticEntities[i].color.b, 0);
		}

		for (int i = 0; i < dynamicEntities.Count; i++)
        {
			dynamicEntities[i].color = new Color(dynamicEntities[i].color.r, dynamicEntities[i].color.g, dynamicEntities[i].color.b, 0);
        }

		if (filter != null)
		{
			if (filterMaxOpacity < 0)
				filterMaxOpacity = filter.color.a;
			filter.color = new Color(filter.color.r, filter.color.g, filter.color.b, 0);
		}

		gameObject.SetActive(false);
	}
    
    private IEnumerator Fade_IEnum(int targetAlpha)
	{
		float t = 0;
		float startAlpha = backgrounds[0].color.a;
		float filterStartAlpha = filter != null ? filter.color.a : 0;
		float filterTargetAlpha = targetAlpha == 1 ? (filter != null ? filterMaxOpacity : 0) : 0;
         
        while(t<1)
		{
			for (int i = 0; i < backgrounds.Length; i++)
			{
				backgrounds[i].color = Color.Lerp(new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, startAlpha),
				                                  new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, targetAlpha),
												  t);
			}
			for (int i = 0; i < staticEntities.Length; i++)
            {
                staticEntities[i].color = Color.Lerp(new Color(staticEntities[i].color.r, staticEntities[i].color.g, staticEntities[i].color.b, startAlpha),
				                               new Color(staticEntities[i].color.r, staticEntities[i].color.g, staticEntities[i].color.b, targetAlpha),
                                                  t);
            }
			for (int i = 0; i < dynamicEntities.Count; i++)
            {
                dynamicEntities[i].color = Color.Lerp(new Color(dynamicEntities[i].color.r, dynamicEntities[i].color.g, dynamicEntities[i].color.b, startAlpha),
                                               new Color(dynamicEntities[i].color.r, dynamicEntities[i].color.g, dynamicEntities[i].color.b, targetAlpha),
                                                  t);
            }
            if(filter != null)
			    filter.color = Color.Lerp(new Color(filter.color.r, filter.color.g, filter.color.b, filterStartAlpha),
									  new Color(filter.color.r, filter.color.g, filter.color.b, filterTargetAlpha),
									  t);
			t += Time.deltaTime / 0.7f;
			yield return null;
		}

		for (int i = 0; i < backgrounds.Length; i++)
        {
			backgrounds[i].color = new Color(backgrounds[i].color.r, backgrounds[i].color.g, backgrounds[i].color.b, targetAlpha);
        }
		for (int i = 0; i < staticEntities.Length; i++)
        {
			staticEntities[i].color = new Color(staticEntities[i].color.r, staticEntities[i].color.g, staticEntities[i].color.b, targetAlpha);
        }
		for (int i = 0; i < dynamicEntities.Count; i++)
        {
            dynamicEntities[i].color = new Color(dynamicEntities[i].color.r, dynamicEntities[i].color.g, dynamicEntities[i].color.b, targetAlpha);
        }
		if (filter != null)
		    filter.color = new Color(filter.color.r, filter.color.g, filter.color.b, filterTargetAlpha);

		if (targetAlpha == 0)
			gameObject.SetActive(false);
	}

}
