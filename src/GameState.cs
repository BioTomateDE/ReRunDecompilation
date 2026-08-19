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

	private void Awake()
	{
		Instance = this;
		Application.targetFrameRate = 200;
		ambientOcclusion = pp.GetSetting<AmbientOcclusion>();
		bloom = pp.GetSetting<Bloom>();
		lens = pp.GetSetting<LensDistortion>();
	}

	private void Start()
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

	public void SetGraphics(bool b)
	{
		graphics = b;
		ambientOcclusion.enabled.value = b;
		lens.enabled.value = b;
		bloom.enabled.value = b;
		if (!graphics)
		{
			QualitySettings.SetQualityLevel(0);
		}
		if (graphics)
		{
			QualitySettings.SetQualityLevel(5);
		}
		SaveManager.Instance.state.graphics = b;
		SaveManager.Instance.Save();
	}

	public void SetBlur(bool b)
	{
	}

	public void SetShake(bool b)
	{
		shake = b;
		if (b)
		{
			cameraShake = 1f;
		}
		else
		{
			cameraShake = 0f;
		}
		SaveManager.Instance.state.cameraShake = b;
		SaveManager.Instance.Save();
	}

	public void SetSlowmo(bool b)
	{
		slowmo = b;
		SaveManager.Instance.state.slowmo = b;
		SaveManager.Instance.Save();
	}

	public void SetSensitivity(float s)
	{
		float num = (sensitivity = Mathf.Clamp(s, 0f, 5f));
		if ((bool)PlayerInput.Instance)
		{
			PlayerInput.Instance.UpdateSensitivity(sensitivity);
		}
		SaveManager.Instance.state.sensitivity = num;
		SaveManager.Instance.Save();
	}

	public void SetMusic(float s)
	{
		float f = (music = Mathf.Clamp(s, 0f, 1f));
		MusicController.Instance.UpdateMusic(f);
		SaveManager.Instance.state.music = f;
		SaveManager.Instance.Save();
	}

	public void SetVolume(float s)
	{
		float num = (AudioListener.volume = (volume = Mathf.Clamp(s, 0f, 1f)));
		SaveManager.Instance.state.volume = num;
		SaveManager.Instance.Save();
	}

	public void ApplySettings()
	{
		AudioListener.volume = volume;
		if ((bool)PlayerInput.Instance)
		{
			PlayerInput.Instance.UpdateSensitivity(sensitivity);
		}
		if ((bool)MoveCamera.Instance)
		{
			MoveCamera.Instance.UpdateFov(fov);
		}
	}

	public void SetFov(float f)
	{
		float num = (fov = Mathf.Clamp(f, 50f, 150f));
		if ((bool)MoveCamera.Instance)
		{
			MoveCamera.Instance.UpdateFov(fov);
		}
		SaveManager.Instance.state.fov = num;
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

	public bool GetGraphics()
	{
		return graphics;
	}

	public float GetSensitivity()
	{
		return sensitivity;
	}

	public float GetVolume()
	{
		return volume;
	}

	public float GetMusic()
	{
		return music;
	}

	public float GetFov()
	{
		return fov;
	}

	public bool GetMuted()
	{
		return muted;
	}
}
