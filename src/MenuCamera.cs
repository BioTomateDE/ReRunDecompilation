using EZCameraShake;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
	public void Start()
	{
		Invoke("StartShake", 0.5f);
	}

	public void StartShake()
	{
		CameraShaker.Instance.StartShake(2.5f, 0.1f, 0.5f);
	}
}
