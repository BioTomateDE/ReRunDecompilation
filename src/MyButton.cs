using UnityEngine;

public class MyButton : MonoBehaviour
{
	public Manipulate[] manipulations;

	public GameObject topButton;

	private Vector3 topButtonPos;

	private bool done;

	public void Awake()
	{
		topButtonPos = topButton.transform.position;
	}

	public void ActivateButton()
	{
		if (!done)
		{
			Manipulate[] array = manipulations;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Activate();
			}
			done = true;
			AudioSource componentInChildren = GetComponentInChildren<AudioSource>();
			if ((bool)componentInChildren)
			{
				componentInChildren.Play();
			}
		}
	}

	public void Update()
	{
		if (done)
		{
			topButton.transform.localPosition = Vector3.Lerp(topButton.transform.localPosition, Vector3.down * 0.2f, Time.deltaTime * 5f);
		}
	}
}
