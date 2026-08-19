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

    public static string GetFormattedTime(float _timeSec)
    {
        if (_timeSec == 0f)
        {
            return "nan";
        }
        string mm = Mathf.Floor(_timeSec / 60f).ToString("00");
        string ss = Mathf.Floor(_timeSec % 60f).ToString("00");
        string fff = (_timeSec * 1000f % 1000f).ToString("00");
        if (fff.Equals("100"))
        {
            fff = "99";
        }
        return $"{mm}:{ss}:{fff}";
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
