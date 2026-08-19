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
                fps.gameObject.SetActive(false);
            }
            return;
        }
        if (!fps.gameObject.activeInHierarchy)
        {
            fps.gameObject.SetActive(true);
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
        console.gameObject.SetActive(true);
        console.Select();
        console.ActivateInputField();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseConsole()
    {
        console.gameObject.SetActive(false);
        Cursor.lockState = previousCursorState;
        Cursor.visible = previousVisible;
    }

    public void RunCommand()
    {
        string _text = console.text;
        consoleLog.text += _text + "\n";
        if (_text.Length < 2 || _text.Length > 30 || CountWords(_text) != 2)
        {
            console.text = "";
            console.Select();
            console.ActivateInputField();
            return;
        }
        console.text = "";
        string _argument = _text.Substring(_text.IndexOf(' ') + 1);
        string _command = _text.Substring(0, _text.IndexOf(' '));
        if (!float.TryParse(_argument, out var _float))
        {
            consoleLog.text += "Command not found\n";
            return;
        }
        switch (_command)
        {
            case "fps":
                OpenCloseFps((int)_float);
                break;
            case "fpslimit":
                FpsLimit((int)_float);
                break;
            case "fov":
                ChangeFov((int)_float);
                break;
            case "sens":
                ChangeSens(_float);
                break;
            case "volume":
                ChangeVolume(_float);
                break;
            case "speed":
                OpenCloseSpeed((int)_float);
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
        const string text = "The console can be used for simple commands.\nEvery command must be followed by number i (0 = false, 1 = true)\n<i><b>fps 1</b></i>            shows fps\n<i><b>speed 1</b></i>      shows speed\n<i><b>fov i</b></i>             sets fov to i\n<i><b>sens i</b></i>          sets sensitivity to i\n<i><b>fpslimit i</b></i>    sets max fps\n<i><b>TAB</b></i>              to open/close the console\n";
        consoleLog.text += text;
    }

    private void FpsLimit(int _fps)
    {
        Application.targetFrameRate = _fps;
        consoleLog.text += "Max FPS set to " + _fps + "\n";
    }

    private void OpenCloseFps(int _open)
    {
        MonoBehaviour.print("n, " + (_open == 1));
        fpsOn = _open == 1;
        consoleLog.text += "FPS set to " + fpsOn + "\n";
    }

    private void OpenCloseSpeed(int _open)
    {
        speedOn = _open == 1;
        consoleLog.text += "Speedometer set to " + _open == 1 + "\n";
    }

    private void ChangeFov(int _fov)
    {
        GameState.Instance.SetFov(_fov);
        TextMeshProUGUI textMeshProUGUI = consoleLog;
        textMeshProUGUI.text = textMeshProUGUI.text + "FOV set to " + _fov + "\n";
    }

    private void ChangeSens(float _sens)
    {
        GameState.Instance.SetSensitivity(_sens);
        TextMeshProUGUI textMeshProUGUI = consoleLog;
        textMeshProUGUI.text = textMeshProUGUI.text + "Sensitivity set to " + _sens + "\n";
    }

    private void ChangeVolume(float _volume)
    {
        AudioListener.volume = _volume;
        TextMeshProUGUI textMeshProUGUI = consoleLog;
        textMeshProUGUI.text = textMeshProUGUI.text + "Volume set to " + _volume + "\n";
    }

    private static int CountWords(string text)
    {
        int wordCount = 0;
        int i = 0;
        while (i < text.Length && char.IsWhiteSpace(text[i]))
        {
            i++;
        }
        while (i < text.Length)
        {
            while (i < text.Length && !char.IsWhiteSpace(text[i]))
            {
                i++;
            }
            wordCount++;
            while (i < text.Length && char.IsWhiteSpace(text[i]))
            {
                i++;
            }
        }
        return wordCount;
    }

    public bool IsConsoleOpen()
    {
        return console.isActiveAndEnabled;
    }
}
