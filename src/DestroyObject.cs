using UnityEngine;

public class DestroyObject : MonoBehaviour
{
	public float time = 2f;

	private void Start()
	{
		Invoke("DestroySelf", time);
	}

	private void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
