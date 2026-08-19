using UnityEngine;

public class DestroyObject : MonoBehaviour
{
	public float time = 2f;

	public void Start()
	{
		Invoke("DestroySelf", time);
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
