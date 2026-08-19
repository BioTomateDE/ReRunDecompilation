using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform spawnPos;

    public GameObject enemy;

    [HideInInspector]
    public bool isRewinding;

    private float rewindTime = 1f;

    private float rewindSeconds = 0.175f;

    private Vector3 vel;

    [HideInInspector]
    public bool playerDead;

    [HideInInspector]
    public bool paused;

    [HideInInspector]
    public bool playing = true;

    private Transform playerTransform;

    public GameObject rewindSymbol;

    public GameObject deathScreen;

    public static GameManager Instance;

    private List<GameObject> enemies;

    private List<Vector3> positions;

    private float t;

    public void Awake()
    {
        Instance = this;
        enemies = new List<GameObject>();
        positions = new List<Vector3>();
        AutoSplitterData.levelID = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void Start()
    {
        playerTransform = PlayerMovement.Instance.transform;
        playerTransform.position = spawnPos.position;
        t = 1f;
        Time.timeScale = 1f;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        positions.Add(enemy.transform.position);
    }

    private void RestartEnemies()
    {
        foreach (GameObject _enemy in enemies)
        {
            Object.Destroy(_enemy);
        }
        enemies = new List<GameObject>();
        foreach (Vector3 _pos in positions)
        {
            enemies.Add(Object.Instantiate(enemy, _pos, Quaternion.identity));
        }
        positions = new List<Vector3>();
    }

    public void Update()
    {
        if (isRewinding)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, spawnPos.position, t);
            t += Time.deltaTime * 0.17f;
            PPController.Instance.UpdateFx(Mathf.Clamp(t * 10f, 0f, 1f));
            if (Vector3.Distance(playerTransform.position, spawnPos.position) < 0.1f)
            {
                StopRewinding();
            }
        }
    }

    public void PlayerDied()
    {
        UIManager.Instance.HidePause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathScreen.SetActive(true);
        playerDead = true;
        PlayerStatus.Instance.Damage(100);
        playing = false;
    }

    public void LevelDone()
    {
        UIManager.Instance.HidePause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playing = false;
    }

    public void StartRewind()
    {
        isRewinding = true;
        t = 0f;
        PlayerMovement.Instance.GetRb().useGravity = false;
        PlayerMovement.Instance.GetRb().velocity = Vector3.zero;
        PPController.Instance.StartRewind();
        rewindSymbol.SetActive(true);
        RestartEnemies();
    }

    private void StopRewinding()
    {
        isRewinding = false;
        PlayerMovement.Instance.GetRb().useGravity = true;
        t = 1f;
        PPController.Instance.StopRewind();
        rewindSymbol.SetActive(false);
    }

    public void Restart(){}
}
