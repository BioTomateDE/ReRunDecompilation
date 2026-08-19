using TMPro;
using UnityEngine;

public class Debug : MonoBehaviour
{
	public TextMeshProUGUI fps;

	public TMP_InputField console;

	public TextMeshProUGUI consoleLog;

	private bool fpsOn;

	private bool speedOn;

	private bool pingOn;

	private bool bandwidthOn;

	private float deltaTime;

	public static Debug Instance;

	private float byteUp;

	private float byteDown;

	private CursorLockMode previousCursorState;

	private bool previousVisible;

	public void Awake()
	{
		Instance = this;
	}

	public void Update()
	{
		Fps();
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (console.isActiveAndEnabled)
			{
				CloseConsole();
			}
			else
			{
				OpenConsole();
			}
		}
	}

	private void Fps()
	{
		if (!fpsOn && !speedOn && !pingOn && !bandwidthOn)
		{
			if (!fps.enabled)
			{
				fps.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!fps.gameObject.activeInHierarchy)
		{
			fps.gameObject.SetActive(value: true);
		}
		string text = "";
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		float num = deltaTime * 1000f;
		float num2 = 1f / deltaTime;
		if (fpsOn)
		{
			text += $"{num:0.0} ms ({num2:0.} fps)";
		}
		if (speedOn && (bool)PlayerMovement.Instance)
		{
			Vector3 velocity = PlayerMovement.Instance.GetVelocity();
			text = text + "\nm/s: " + $"{new Vector2(velocity.x, velocity.z).magnitude:F1}";
		}
		fps.text = text;
	}

	private void OpenConsole()
	{
		previousCursorState = Cursor.lockState;
		previousVisible = Cursor.visible;
		console.gameObject.SetActive(value: true);
		console.Select();
		console.ActivateInputField();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void CloseConsole()
	{
		console.gameObject.SetActive(value: false);
		Cursor.lockState = previousCursorState;
		Cursor.visible = previousVisible;
	}

	public void RunCommand()
	{
		string text = console.text;
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + text + "\n";
		if (text.Length < 2 || text.Length > 30 || CountWords(text) != 2)
		{
			console.text = "";
			console.Select();
			console.ActivateInputField();
			return;
		}
		console.text = "";
		string s = text.Substring(text.IndexOf(' ') + 1);
		string text2 = text.Substring(0, text.IndexOf(' '));
		if (!float.TryParse(s, out var result))
		{
			consoleLog.text += "Command not found\n";
			return;
		}
		switch (text2)
		{
		case "fps":
			OpenCloseFps((int)result);
			break;
		case "fpslimit":
			FpsLimit((int)result);
			break;
		case "fov":
			ChangeFov((int)result);
			break;
		case "sens":
			ChangeSens(result);
			break;
		case "volume":
			ChangeVolume(result);
			break;
		case "speed":
			OpenCloseSpeed((int)result);
			break;
		case "help":
			Help();
			break;
		}
		console.Select();
		console.ActivateInputField();
	}

	private void Help()
	{
		string text = "The console can be used for simple commands.\nEvery command must be followed by number i (0 = false, 1 = true)\n<i><b>fps 1</b></i>            shows fps\n<i><b>speed 1</b></i>      shows speed\n<i><b>fov i</b></i>             sets fov to i\n<i><b>sens i</b></i>          sets sensitivity to i\n<i><b>fpslimit i</b></i>    sets max fps\n<i><b>TAB</b></i>              to open/close the console\n";
		consoleLog.text += text;
	}

	private void FpsLimit(int n)
	{
		Application.targetFrameRate = n;
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + "Max FPS set to " + n + "\n";
	}

	private void OpenCloseFps(int n)
	{
		MonoBehaviour.print("n, " + (n == 1));
		fpsOn = n == 1;
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + "FPS set to " + fpsOn + "\n";
	}

	private void OpenCloseSpeed(int n)
	{
		speedOn = n == 1;
		consoleLog.text += "Speedometer set to " + n == 1 + "\n";
	}

	private void ChangeFov(int n)
	{
		GameState.Instance.SetFov(n);
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + "FOV set to " + n + "\n";
	}

	private void ChangeSens(float n)
	{
		GameState.Instance.SetSensitivity(n);
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + "Sensitivity set to " + n + "\n";
	}

	private void ChangeVolume(float n)
	{
		AudioListener.volume = n;
		TextMeshProUGUI textMeshProUGUI = consoleLog;
		textMeshProUGUI.text = textMeshProUGUI.text + "Volume set to " + n + "\n";
	}

	private int CountWords(string text)
	{
		int num = 0;
		int i;
		for (i = 0; i < text.Length && char.IsWhiteSpace(text[i]); i++)
		{
		}
		while (i < text.Length)
		{
			for (; i < text.Length && !char.IsWhiteSpace(text[i]); i++)
			{
			}
			num++;
			for (; i < text.Length && char.IsWhiteSpace(text[i]); i++)
			{
			}
		}
		return num;
	}

	public bool IsConsoleOpen()
	{
		return console.isActiveAndEnabled;
	}
}
