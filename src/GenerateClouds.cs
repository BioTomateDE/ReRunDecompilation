using UnityEngine;

public class GenerateClouds : MonoBehaviour
{
	public GameObject cloud;

	private int n = 100;

	public void Start()
	{
		MakeClouds();
	}

	private void MakeClouds()
	{
		for (int i = 0; i < n; i++)
		{
			Vector3 position = base.transform.position + Vector3.right * Random.Range(-250, 250) + Vector3.forward * Random.Range(-250, 250) + Vector3.up * Random.Range(-10, 10);
			Vector3 localScale = base.transform.localScale * Random.Range(0.75f, 1.5f);
			Quaternion rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			Object.Instantiate(cloud, position, rotation).transform.localScale = localScale;
		}
	}
}
