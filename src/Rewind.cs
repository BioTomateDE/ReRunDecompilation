using UnityEngine;

public class Rewind : MonoBehaviour
{
    private class RewindObject
    {
        public Vector3 position;

        public Vector3 velocity;

        public RewindObject(Vector3 pos, Vector3 vel)
        {
            position = pos;
            velocity = vel;
        }
    }

    private int tick;

    private int seconds = 2;

    private int bufferSize;

    private RewindObject[] playerHistory;

    private Rigidbody rb;

    public LineRenderer lr;

    private bool reversing;

    private Vector3 reversePos;

    private float t;

    public void Awake()
    {
        bufferSize = (int)(1f / Time.fixedDeltaTime) * seconds;
        lr.positionCount = bufferSize;
        rb = GetComponent<Rigidbody>();
        playerHistory = new RewindObject[bufferSize];
        RewindObject _rewindObject = new(base.transform.position, Vector3.zero);
        for (int i = 0; i < bufferSize; i++)
        {
            playerHistory[i] = _rewindObject;
        }
        tick = bufferSize;
    }

    public void FixedUpdate()
    {
        RewindObject rewindObject = new(base.transform.position, rb.velocity);
        playerHistory[tick % bufferSize] = rewindObject;
        tick++;
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < bufferSize; i++)
        {
            lr.SetPosition(i, playerHistory[(tick - bufferSize + i) % bufferSize].position);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reversePos = playerHistory[(tick - bufferSize) % bufferSize].position;
            reversing = true;
            rb.isKinematic = true;
            t = 0f;
        }
        if (reversing)
        {
            t += Time.deltaTime;
            base.transform.position = Vector3.Lerp(base.transform.position, reversePos, t);
        }
    }
}
