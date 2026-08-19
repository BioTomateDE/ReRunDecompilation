using UnityEngine;

public class ControlMusic : MonoBehaviour
{
	private float desiredFreq;

	private void Awake()
	{
	}

	private void Update()
	{
		if ((bool)MusicController.Instance)
		{
			float b = 0.02f;
			if (PlayerMovement.Instance.GetVelocity().magnitude > 15f && (!GameManager.Instance.playerDead & GameManager.Instance.playing))
			{
				b = 1f;
			}
			desiredFreq = Mathf.Lerp(desiredFreq, b, Time.deltaTime * 5f);
			MusicController.Instance.SetFreq(desiredFreq);
		}
	}
}
