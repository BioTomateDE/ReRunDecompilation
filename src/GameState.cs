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

    public float cameraShake = 1f;

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
        graphics = _pretty;
        ambientOcclusion.enabled.value = _pretty;
        lens.enabled.value = _pretty;
        bloom.enabled.value = _pretty;
        if (!graphics)
        {
            QualitySettings.SetQualityLevel(0);
        }
        if (graphics)
        {
            QualitySettings.SetQualityLevel(5);
        }
        SaveManager.Instance.state.graphics = _pretty;
        SaveManager.Instance.Save();
    }

    public void SetBlur(bool _enabled)
    {
    }

    public void SetShake(bool _enabled)
    {
        shake = _enabled;
        if (_enabled)
        {
            cameraShake = 1f;
        }
        else
        {
            cameraShake = 0f;
        }
        SaveManager.Instance.state.cameraShake = _enabled;
        SaveManager.Instance.Save();
    }

    public void SetSlowmo(bool _enabled)
    {
        slowmo = _enabled;
        SaveManager.Instance.state.slowmo = _enabled;
        SaveManager.Instance.Save();
    }

    public void SetSensitivity(float _rawSens)
    {
        sensitivity = Mathf.Clamp(_rawSens, 0f, 5f);
        if (PlayerInput.Instance)
        {
            PlayerInput.Instance.UpdateSensitivity(sensitivity);
        }
        SaveManager.Instance.state.sensitivity = sensitivity;
        SaveManager.Instance.Save();
    }

    public void SetMusic(float _rawMusicVolume)
    {
        music = Mathf.Clamp(_rawMusicVolume, 0f, 1f);
        MusicController.Instance.UpdateMusic(music);
        SaveManager.Instance.state.music = music;
        SaveManager.Instance.Save();
    }

    public void SetVolume(float _rawVolume)
    {
        volume = Mathf.Clamp(_rawVolume, 0f, 1f);
        AudioListener.volume = volume;
        SaveManager.Instance.state.volume = volume;
        SaveManager.Instance.Save();
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

    public void SetFov(float _rawFov)
    {
        fov = Mathf.Clamp(_rawFov, 50f, 150f);
        if (MoveCamera.Instance)
        {
            MoveCamera.Instance.UpdateFov(fov);
        }
        SaveManager.Instance.state.fov = fov;
        SaveManager.Instance.Save();
    }

    private void UpdateSettings()
    {
        SetGraphics(graphics);
        SetBlur(blur);
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
