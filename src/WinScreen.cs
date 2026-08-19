using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
	public TextMeshProUGUI timer;

	public Image image;

	public static WinScreen Instance;

	public void Awake()
	{
		Instance = this;
		base.gameObject.SetActive(value: false);
	}

	public void OnEnable()
	{
		image.CrossFadeAlpha(0f, 0f, ignoreTimeScale: true);
		image.CrossFadeAlpha(1f, 1f, ignoreTimeScale: true);
		timer.text = Timer.GetFormattedTime(Timer.Instance.GetTimer());
	}

	public void NextLevel()
	{
		int num = SceneManager.GetActiveScene().buildIndex + 1;
		AutoSplitterData.isLoading = 1;
		AutoSplitterData.levelID = num;
		int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
		MonoBehaviour.print("next: " + num + ", scenes: " + sceneCountInBuildSettings);
		if (num >= sceneCountInBuildSettings)
		{
			SceneManager.LoadScene("Menu");
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
