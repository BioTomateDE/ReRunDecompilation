using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debug : MonoBehaviour
{
    public TextMeshProUGUI overlay;

    public TMP_InputField console;

    public TextMeshProUGUI consoleLog;

    private bool showFps;
    private bool showSpeed;
    private bool showPosition;
    private bool showJumps;

    private float deltaTime;
    private const float DELTATIME_SMOOTHING = 0.1f;

    private bool clamp = true;
    private bool verbose;

    private bool cheatsEverEnabled;
    private bool allowCheats;
    internal bool god;

    public static Debug Instance;

    private CursorLockMode previousCursorState;

    private bool previousVisible;

    private List<string> commandHistory = new();
    private int commandHistoryIndex = -1;

    private static readonly HashSet<string> _numericCommands = new() {
       "cheats", "clamp", "verbose", "fps", "speed", "pos", "showjumps", "graphics", "fov", "sens", "volume", "fpslimit", "jumps", "god",
    };

    private const float SCREEN_PADDING = 5f;

    public void Awake()
    {
        Instance = this;
        consoleLog.text = "Debug console :3\n";  // previous: CONSOLE - type "help 1"

        RectTransform rt = overlay.rectTransform;
        rt.sizeDelta = new(400f, 100f); // previous: (200, 50)
        rt.anchoredPosition = new(SCREEN_PADDING, -SCREEN_PADDING); // previous: (0, 0)

        console.textComponent.margin = new(20, 0, 0, 0);
    }

    public void Update()
    {
        UpdateOverlay();
        if (console.isActiveAndEnabled)
        {
            CapLines(consoleLog.text, consoleLog);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (console.isActiveAndEnabled)
                CloseConsole();
            else
                OpenConsole();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && commandHistoryIndex != 0)
        {
            if (commandHistoryIndex == -1)
            {
                commandHistoryIndex = commandHistory.Count;
            }
            console.text = commandHistory[--commandHistoryIndex];
            console.caretPosition = console.text.Length;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (commandHistoryIndex == -1 || commandHistoryIndex + 1 >= commandHistory.Count)
            {
                // deletes the query when pressing Down at the end of the history (could also be a no-op)
                commandHistoryIndex = -1;
                console.text = "";
            }
            else
            {
                console.text = commandHistory[++commandHistoryIndex];
                console.caretPosition = console.text.Length;
            }
        }
    }

    private void UpdateOverlay()
    {
        string _text = "";
        if (cheatsEverEnabled)
        {
            string _color = allowCheats ? "#ff0000" : "#ffff00";
            _text += "<color=" + _color + ">Cheats</color>\n";
        }

        // custom deltatime to prevent jitter
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * DELTATIME_SMOOTHING;
        if (showFps)
        {
            float _dt = deltaTime * 1000f;
            float _fps = 1f / deltaTime;
            if (verbose)
                _text += $"FPS: {_fps:F1} ({_dt:F3} ms)\n";
            else
                _text += $"FPS: {_fps:F0} ({_dt:F2} ms)\n";
        }

        if (showSpeed && PlayerMovement.Instance)
        {
            Vector3 _velocity = PlayerMovement.Instance.GetVelocity();
            float _speed = new Vector2(_velocity.x, _velocity.z).magnitude;
            if (verbose)
                _text += $"Speed: {_speed:F4} m/s\n";
            else
                _text += $"Speed: {_speed:F1} m/s\n";
        }

        if (showPosition && PlayerMovement.Instance)
        {
            Vector3 _pos = PlayerMovement.Instance.GetPosition();
            if (verbose)
                _text += $"Position: {_pos.x:F3}, {_pos.y:F3}, {_pos.z:F3}\n";
            else
                _text += $"Position: {_pos.x:F1}, {_pos.y:F1}, {_pos.z:F1}\n";
        }

        if (showJumps && PlayerMovement.Instance is { } player)
        {
            int _jumps = player.maxJumps - player.jumpsLeft;
            _text += $"Jumps: {_jumps}/{player.maxJumps}\n";
        }

        overlay.text = _text;
    }

    private void WriteLine(string line)
    {
        consoleLog.text += line + "\n";
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
        console.text = "";
    }

    public bool IsConsoleOpen()
    {
        return console.isActiveAndEnabled;
    }

    public void RunCommand()
    {
        string _text = console.text.Trim();
        console.text = "";
        console.Select();
        console.ActivateInputField();
        if (_text == "")
        {
            return;
        }
        WriteLine("> " + _text);
        commandHistory.Add(_text);
        if (commandHistory.Count > 69)
        {
            // very good garbage collection
            commandHistory.RemoveAt(0);
        }
        commandHistoryIndex = -1;

        int _spaceIndex = _text.IndexOf(' ');
        string _command = _spaceIndex == -1 ? _text : _text.Substring(0, _spaceIndex);
        string _argument = _spaceIndex == -1 ? "" : _text.Substring(_spaceIndex + 1);

        // these commands require no arguments (arg is ignored)
        switch (_command)
        {
            case "help":
                if (_argument == "cheats")
                    HelpCheats();
                else
                    Help();
                return;
            case "clear":
                consoleLog.text = "";
                return;
        }

        // these commands require a number argument
        if (!_numericCommands.Contains(_command))
        {
            WriteLine("Unknown command (type help for help)");
            return;
        }

        if (!float.TryParse(_argument, out float _float))
        {
            WriteLine("Invalid number");
            return;
        }

        // when adding a new command, make sure to also add it to _numericCommands!
        switch (_command)
        {
            case "cheats":
                ToggleCheats(_float != 0);
                break;
            case "clamp":
                clamp = _float != 0;
                WriteLine(clamp ? "Enabled clamping" : "Disabled clamping (be careful!)");
                break;
            case "verbose":
                verbose = _float != 0;
                showFps = showJumps = showPosition = showSpeed = true;
                WriteLine((verbose ? "Enabled" : "Disabled") + " verbose output");
                break;
            case "fps":
                showFps = _float != 0;
                WriteLine((showFps ? "Enabled" : "Disabled") + " FPS meter");
                break;
            case "speed":
                showSpeed = _float != 0;
                WriteLine((showSpeed ? "Enabled" : "Disabled") + " speedometer");
                break;
            case "pos":
                showPosition = _float != 0;
                WriteLine((showPosition ? "Enabled" : "Disabled") + " compass");
                break;
            case "showjumps":
                showJumps = _float != 0;
                WriteLine((showJumps ? "Enabled" : "Disabled") + " jump count text");
                break;
            case "graphics":
                bool _pretty = _float != 0;
                GameState.Instance.SetGraphics(_pretty);
                WriteLine("Set graphics quality to " + (_pretty ? "Pretty" : "Shit"));
                break;
            case "sens":
                float _clampedSens = GameState.Instance.SetSensitivity(_float, clamp);
                WriteLine("Set sensitivity to " + _clampedSens);
                break;
            case "volume":
                // The original code did `AudioListener.volume = _volume;` instead which doesn't clamp.
                // That's why you could set the volume way too high lol.
                float _clampedVolume = GameState.Instance.SetVolume(_float, clamp);
                WriteLine("Set volume to " + _clampedVolume);
                break;
            case "music":
                float _clampedMusicVolume = GameState.Instance.SetMusic(_float, clamp);
                WriteLine("Set music volume to " + _clampedMusicVolume);
                break;
            case "fov":
                float _clampedFov = GameState.Instance.SetFov((int)_float, clamp);
                WriteLine("Set FOV to " + _clampedFov);
                break;
            case "fpslimit":
                Application.targetFrameRate = (int)_float;
                WriteLine("Set maximum FPS to " + (int)_float);
                break;
            case "god":
                if (CheatsDisabled()) return;
                god = _float != 0;
                WriteLine((showPosition ? "Enabled" : "Disabled") + " god mode");
                break;
            case "jumps":
                if (CheatsDisabled()) break;
                int _jumps = (int)_float;
                PlayerMovement.Instance.maxJumps = _jumps;
                PlayerMovement.Instance.jumpsLeft = _jumps;
                WriteLine("Set maximum jumps to " + _jumps);
                break;
        }
    }

    private void Help()
    {
        WriteHelpCommand("help", "shows this help");
        WriteHelpCommand("clear", "clears console log");
        WriteHelpCommand("cheats 1", "allows cheats to be enabled");
        WriteHelpCommand("clamp 0", "disables value clamping");
        WriteHelpCommand("verbose 1", "shows additional information that may be relevant to you");
        WriteHelpCommand("fps 1", "shows FPS");
        WriteHelpCommand("speed 1", "shows horizontal speed");
        WriteHelpCommand("pos 1", "shows coordinates");
        WriteHelpCommand("showjumps 1", "shows jump count");
        WriteHelpCommand("graphics 1", "enables pretty graphics");
        WriteHelpCommand("sens X", "sets mouse sensitivity to X");
        WriteHelpCommand("volume X", "sets volume to X");
        WriteHelpCommand("music X", "sets music volume to X");
        WriteHelpCommand("fov N", "sets FOV to N");
        WriteHelpCommand("fpslimit N", "limits FPS to N");
    }

    private void HelpCheats()
    {
        WriteHelpCommand("god 1", "enables invincibility");
        WriteHelpCommand("jumps N", "sets max jump count to N");
    }

    private void WriteHelpCommand(string command, string description)
    {
        WriteLine($"<b>{command}</b>  -  {description}");
    }

    private void ToggleCheats(bool _allow)
    {
        allowCheats = _allow;
        if (_allow)
        {
            cheatsEverEnabled = true;
            WriteLine("Allowed cheats (see <b>help cheats</b>)");
        }
        else
        {
            WriteLine("Disallowed cheats (restart game for indicator to go away)");
        }
    }

    private bool CheatsDisabled()
    {
        if (!allowCheats)
        {
            WriteLine("Cheats are disabled");
        }
        return !allowCheats;
    }

    private static void CapLines(string log, TextMeshProUGUI tmp)
    {
        const int MAX_LINES = 19;

        tmp.ForceMeshUpdate();
        var textInfo = tmp.textInfo;

        if (textInfo.lineCount > MAX_LINES)
        {
            int lineIndex = textInfo.lineCount - MAX_LINES;
            int charIndex = textInfo.lineInfo[lineIndex].firstCharacterIndex;
            tmp.text = tmp.text.Substring(charIndex);
        }
    }
}
