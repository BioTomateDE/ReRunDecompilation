using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public TextMeshProUGUI[] times;

    public void OnEnable()
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

    public void LoadLevel(int _level)
    {
        SceneManager.LoadScene(_level + 1);
    }
}
