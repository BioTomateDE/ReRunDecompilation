using UnityEngine;

public class LoadSettings : MonoBehaviour
{
	public void Start()
	{
		if ((bool)GameState.Instance)
		{
			GameState.Instance.ApplySettings();
			base.transform.GetChild(0).gameObject.SetActive(value: false);
		}
	}
}
