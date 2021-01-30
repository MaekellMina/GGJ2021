using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform camera;
	[SerializeField]
	private float panSpeed = 3.0f;
	[SerializeField]
	private Transform leftLimit;
	[SerializeField]
	private Transform rightLimit;
	[SerializeField]
	private GameObject leftArrow;
	[SerializeField]
	private GameObject rightArrow;

    private Vector3 velocity = Vector3.zero;

	private Coroutine smoothStopCoroutine;

	public bool hoveringLeft { set; get; }
	public bool hoveringRight { set; get; }
	bool pressing;
	bool canPressLeft;
	bool canPressRight;

	private void Update()
	{
		if (!GameManager.instance.IsPlayState())
			return;
		
		if (camera.position.x > leftLimit.position.x)
		{
			canPressLeft = true;
			leftArrow.SetActive(true);
		}
		else
		{
			canPressLeft = false;
			leftArrow.SetActive(false);
			hoveringLeft = false;
		}

		if (camera.position.x < rightLimit.position.x)
		{
			canPressRight = true;
			rightArrow.SetActive(true);
		}
		else
		{
			canPressRight = false;
			rightArrow.SetActive(false);
			hoveringRight = false;
		}

		if(Input.GetMouseButton(0))
		{
			if (hoveringLeft)
			{
				if (canPressLeft)
				{
					pressing = true;
					velocity = Vector3.left * panSpeed;
				}
			}
			else if (hoveringRight)
			{
				if (canPressRight)
				{
					pressing = true;
					velocity = Vector3.right * panSpeed;
				}
			}
			else
			{
				if (pressing)
				{
					//smooth stop
					if (smoothStopCoroutine != null)
					{
						StopCoroutine(smoothStopCoroutine);
					}
					smoothStopCoroutine = StartCoroutine(SmoothStop());

					pressing = false;
				}
			}
		}
		else
        {
            if (pressing)
            {
                //smooth stop
                if (smoothStopCoroutine != null)
                {
                    StopCoroutine(smoothStopCoroutine);
                }
                smoothStopCoroutine = StartCoroutine(SmoothStop());

                pressing = false;
            }
        }

		//move
		camera.Translate(velocity * Time.deltaTime);
	}

	private IEnumerator SmoothStop()
	{
		float t = 0;
		Vector3 startVelocity = velocity;
        while(t< 1)
		{
			velocity = Vector3.Lerp(startVelocity, Vector3.zero, t);
			t += Time.deltaTime;
			yield return null;
		}

		velocity = Vector3.zero;
	}
}
