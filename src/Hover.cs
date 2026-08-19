using UnityEngine;

public class Hover : MonoBehaviour
{
	private Vector3 desiredPos;

	private float startY;

	public void Awake()
	{
		desiredPos = base.transform.position;
		startY = base.transform.position.y;
	}

	public void Update()
	{
		desiredPos.y = startY + Mathf.PingPong(Time.time, 1f) - 0.5f;
		base.transform.position = Vector3.Lerp(base.transform.position, desiredPos, Time.deltaTime);
		base.transform.Rotate(Vector3.up, 0.25f);
	}
}
