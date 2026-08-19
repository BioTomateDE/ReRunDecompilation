using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private float sensitivity;

    private float volume;

    private float music;

    private float fov;

    private bool shake;

    private bool graphics;

    public Slider sliderSens;

    public Slider sliderVol;

    public Slider sliderMusic;

    public Slider sliderFov;

    public TextMeshProUGUI sensText;

    public TextMeshProUGUI fovText;

    public Image shakeBtn;

    public Image graphicsBtn;

    public void Start()
    {
        sensitivity = SaveManager.Instance.state.sensitivity;
        volume = SaveManager.Instance.state.volume;
        music = SaveManager.Instance.state.music;
        fov = SaveManager.Instance.state.fov;
        shake = SaveManager.Instance.state.cameraShake;
        graphics = SaveManager.Instance.state.graphics;
        UpdateAllSliders();
        UpdateAllButtons();
    }

    private void UpdateAllSliders()
    {
        sliderSens.value = sensitivity;
        sliderFov.value = fov;
        sliderMusic.value = music;
        sliderVol.value = volume;
        sensText.text = string.Concat(sensitivity);
        fovText.text = string.Concat(fov);
    }

    private void UpdateAllButtons()
    {
        shakeBtn.enabled = shake;
        graphicsBtn.enabled = graphics;
    }

    public void ToggleGraphics()
    {
        graphics = !graphics;
        SaveManager.Instance.state.graphics = graphics;
        SaveManager.Instance.Save();
        GameState.Instance.SetGraphics(graphics);
        UpdateAllButtons();
    }

    public void ToggleShake()
    {
        shake = !shake;
        SaveManager.Instance.state.cameraShake = shake;
        SaveManager.Instance.Save();
        GameState.Instance.SetShake(shake);
        UpdateAllButtons();
    }

    public void Sensitivity()
    {
        sensitivity = Mathf.Round(sliderSens.value * 100f) / 100f;
        SaveManager.Instance.state.sensitivity = sensitivity;
        SaveManager.Instance.Save();
        GameState.Instance.SetSensitivity(sensitivity);
        sensText.text = string.Concat(sensitivity);
    }

    public void Fov()
    {
        fov = sliderFov.value;
        SaveManager.Instance.state.fov = fov;
        SaveManager.Instance.Save();
        GameState.Instance.SetFov(fov);
        fovText.text = string.Concat(fov);
    }

    public void Volume()
    {
        volume = sliderVol.value;
        SaveManager.Instance.state.volume = volume;
        SaveManager.Instance.Save();
        GameState.Instance.SetVolume(volume);
    }

    public void Music()
    {
        music = sliderMusic.value;
        SaveManager.Instance.state.music = music;
        SaveManager.Instance.Save();
        GameState.Instance.SetMusic(music);
    }
}
