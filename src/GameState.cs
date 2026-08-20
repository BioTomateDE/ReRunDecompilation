using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameState : MonoBehaviour
{
    public GameObject ppVolume;

    public PostProcessProfile pp;

    private AmbientOcclusion ambientOcclusion;

    private Bloom bloom;

    private LensDistortion lens;

    public bool graphics = true;

    public bool muted;

    public bool blur = true;

    public bool shake = true;

    public bool slowmo = true;

    private float sensitivity = 1f;

    private float volume;

    private float music;

    public float fov = 1f;

    public static GameState Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 200;
        ambientOcclusion = pp.GetSetting<AmbientOcclusion>();
        bloom = pp.GetSetting<Bloom>();
        lens = pp.GetSetting<LensDistortion>();
    }

    public void Start()
    {
        graphics = SaveManager.Instance.state.graphics;
        shake = SaveManager.Instance.state.cameraShake;
        blur = SaveManager.Instance.state.motionBlur;
        slowmo = SaveManager.Instance.state.slowmo;
        muted = SaveManager.Instance.state.muted;
        sensitivity = SaveManager.Instance.state.sensitivity;
        music = SaveManager.Instance.state.music;
        volume = SaveManager.Instance.state.volume;
        fov = SaveManager.Instance.state.fov;
        UpdateSettings();
    }

    public void SetGraphics(bool _pretty)
    {
        int _qualityLevel = _pretty ? 5 : 0;
        QualitySettings.SetQualityLevel(_qualityLevel);
        graphics = _pretty;
        ambientOcclusion.enabled.value = _pretty;
        lens.enabled.value = _pretty;
        bloom.enabled.value = _pretty;
        SaveManager.Instance.state.graphics = _pretty;
        SaveManager.Instance.Save();
    }

    public void SetShake(bool _enabled)
    {
        shake = _enabled;
        SaveManager.Instance.state.cameraShake = _enabled;
        SaveManager.Instance.Save();
    }

    public void SetSlowmo(bool _enabled)
    {
        slowmo = _enabled;
        SaveManager.Instance.state.slowmo = _enabled;
        SaveManager.Instance.Save();
    }

    public float SetSensitivity(float _rawSens, bool _clamp = true)
    {
        sensitivity = _clamp ? Mathf.Clamp(_rawSens, 0f, 5f) : _rawSens;
        if (PlayerInput.Instance)
        {
            PlayerInput.Instance.UpdateSensitivity(sensitivity);
        }
        SaveManager.Instance.state.sensitivity = sensitivity;
        SaveManager.Instance.Save();
        return sensitivity;
    }

    public float SetMusic(float _rawMusicVolume, bool _clamp = true)
    {
        music = _clamp ? Mathf.Clamp(_rawMusicVolume, 0f, 1f) : _rawMusicVolume;
        MusicController.Instance.UpdateMusic(music);
        SaveManager.Instance.state.music = music;
        SaveManager.Instance.Save();
        return music;
    }

    public float SetVolume(float _rawVolume, bool _clamp = true)
    {
        volume = _clamp ? Mathf.Clamp(_rawVolume, 0f, 1f) : _rawVolume;
        AudioListener.volume = volume;
        SaveManager.Instance.state.volume = volume;
        SaveManager.Instance.Save();
        return volume;
    }

    public void ApplySettings()
    {
        AudioListener.volume = volume;
        if (PlayerInput.Instance)
        {
            PlayerInput.Instance.UpdateSensitivity(sensitivity);
        }
        if (MoveCamera.Instance)
        {
            MoveCamera.Instance.UpdateFov(fov);
        }
    }

    public float SetFov(float _rawFov, bool _clamp = true)
    {
        fov = _clamp ? Mathf.Clamp(_rawFov, 50f, 150f) : _rawFov;
        if (MoveCamera.Instance)
        {
            MoveCamera.Instance.UpdateFov(fov);
        }
        SaveManager.Instance.state.fov = fov;
        SaveManager.Instance.Save();
        return fov;
    }

    private void UpdateSettings()
    {
        SetGraphics(graphics);
        SetSensitivity(sensitivity);
        SetMusic(music);
        SetVolume(volume);
        SetFov(fov);
        SetShake(shake);
        SetSlowmo(slowmo);
    }

    public bool GetGraphics() => graphics;
    public float GetSensitivity() => sensitivity;
    public float GetVolume() => volume;
    public float GetMusic() => music;
    public float GetFov() => fov;
    public bool GetMuted() => muted;
}
