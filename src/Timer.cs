using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
	private TextMeshProUGUI text;

	private float timer;

	private bool stop;

	public static Timer Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		text = GetComponent<TextMeshProUGUI>();
		stop = false;
		StartTimer();
	}

	public void StartTimer()
	{
		stop = false;
		timer = 0f;
	}

	public void Update()
	{
		if (GameManager.Instance.playing && !stop)
		{
			timer += Time.deltaTime;
			AutoSplitterData.inGameTime = timer;
			text.text = GetFormattedTime(timer);
		}
	}

	public static string GetFormattedTime(float f)
	{
		if (f == 0f)
		{
			return "nan";
		}
		string arg = Mathf.Floor(f / 60f).ToString("00");
		string arg2 = Mathf.Floor(f % 60f).ToString("00");
		string text = (f * 1000f % 1000f).ToString("00");
		if (text.Equals("100"))
		{
			text = "99";
		}
		return $"{arg}:{arg2}:{text}";
	}

	public float GetTimer()
	{
		return timer;
	}

	public void Stop()
	{
		stop = true;
	}

	public int GetMinutes()
	{
		return (int)Mathf.Floor(timer / 60f);
	}
}
