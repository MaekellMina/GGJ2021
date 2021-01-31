using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

	bool shaking;

    void Update()
    {
		if (shake_intensity > 0)
		{
			shaking = true;
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			//transform.rotation = new Quaternion(
			//originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
			//originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
			//originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
			//originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
			shake_intensity -= shake_decay;
		}
		else
		{
			if (shaking)
			{
				transform.position = originPosition;
				shaking = false;
			}
		}
    }

    public void Shake(float intensity, float decay)
    {
		shaking = true;
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = intensity;
        shake_decay = decay;
    }
}
