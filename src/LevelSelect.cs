using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
	public TextMeshProUGUI[] times;

	private void OnEnable()
	{
		UpdateTimes();
	}

	private void UpdateTimes()
	{
		for (int i = 0; i < times.Length; i++)
		{
			times[i].text = Timer.GetFormattedTime(SaveManager.Instance.state.times[i]);
		}
	}

	public void LoadLevel(int i)
	{
		SceneManager.LoadScene(i + 1);
	}
}
