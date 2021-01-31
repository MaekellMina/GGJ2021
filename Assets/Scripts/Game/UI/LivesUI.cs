using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
	public CanvasGroup[] crossUI;
	public CameraShake cameraShake;
	public int Lives { set; get; }
	private int maxLives;
    

	[SerializeField]
	private AnimationCurve scaleAnimationCurve;

    public void ResetLives()
    {
		maxLives = crossUI.Length;
		Lives = maxLives;

		for (int i = 0; i < maxLives; i++)
		{
			crossUI[i].gameObject.SetActive(false);
			crossUI[i].alpha = 0;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
			DecreaseLives();
        
    }

    public void DecreaseLives()
    {
		if (Lives > 0)
		{
			cameraShake.Shake(0.4f, 0.07f);
			StartCoroutine(Crossout_IEnum(maxLives - Lives));
			Lives--;

        }

			if (Lives <= 0)
				Lives = 0;
    }

    //animates cross out
    private IEnumerator Crossout_IEnum(int index)
	{
		crossUI[index].gameObject.SetActive(true);

		float t = 0;
        while(t<1)
		{
			crossUI[index].alpha = t;
			crossUI[index].transform.localScale = Vector3.one * scaleAnimationCurve.Evaluate(t);

			t += Time.deltaTime / 0.4f;
			yield return null;
		}

		crossUI[index].alpha = 1;
		crossUI[index].transform.localScale = Vector3.one;

	}
}
