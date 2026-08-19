using UnityEngine;

public class Wind : MonoBehaviour
{
	private AudioSource wind;

	private void Awake()
	{
		wind = GetComponent<AudioSource>();
	}

	private void Update()
	{
		float value = PlayerMovement.Instance.GetVelocity().magnitude / 60f;
		value = Mathf.Clamp(value, 0f, 0.85f);
		if (!PlayerMovement.Instance.grounded)
		{
			value *= 2f;
		}
		wind.volume = Mathf.Lerp(wind.volume, value, Time.deltaTime * 5f);
	}
}
