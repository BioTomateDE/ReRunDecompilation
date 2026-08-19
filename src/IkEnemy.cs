using DitzelGames.FastIK;
using UnityEngine;

public class IkEnemy : MonoBehaviour
{
    public LayerMask whatIsGround;

    public float heightAboveGround;

    public FastIKFabric[] legs;

    private Transform[] legTargets;

    private Vector3[] targetPositions;

    private Vector3[] currentPositions;

    public Vector3 legTargetOffset;

    public Transform root;

    private float thresholdDistance;

    private float[] legProgress;

    private RigidEnemy rigidEnemy;

    public float legSpeed = 10f;

    private Vector3 currentVelocity;

    public float upAmount = 2f;

    public void Start()
    {
        rigidEnemy = GetComponent<RigidEnemy>();
        legTargets = new Transform[legs.Length];
        targetPositions = new Vector3[legs.Length];
        currentPositions = new Vector3[legs.Length];
        legProgress = new float[legs.Length];
        InitLegTargets();
        if (heightAboveGround == 0f)
        {
            heightAboveGround = legs[0].CompleteLength;
            thresholdDistance = heightAboveGround;
        }
        UpdateLegTargets();
        UpdateCurrentLegPosition(0);
        UpdateCurrentLegPosition(1);
        InvokeRepeating("SlowUpdate", 1f, 1f);
    }

    public void Update()
    {
        currentVelocity = rigidEnemy.GetVelocity() * thresholdDistance;
        UpdateLegTargets();
        UpdateCurrentLegPositions(thresholdDistance);
        LerpLegs();
    }

    public void SlowUpdate()
    {
        UpdateCurrentLegPositions(thresholdDistance * 0.2f);
    }

    private void InitLegTargets()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            int _chainLength = legs[i].ChainLength;
            Transform parent = legs[i].transform;
            while (_chainLength > 0)
            {
                parent = parent.parent;
                _chainLength--;
            }
            legTargets[i] = parent;
        }
    }

    private void UpdateLegTargets()
    {
        for (int i = 0; i < legTargets.Length; i++)
        {
            Vector3 _vector = legTargets[i].position - root.position;
            if (Physics.Raycast(legTargets[i].position + (legTargetOffset.x * _vector) + currentVelocity + Vector3.up, Vector3.down, out var hitInfo, 50f, whatIsGround))
            {
                targetPositions[i] = hitInfo.point;
            }
        }
    }

    private void UpdateCurrentLegPositions(float _threshold)
    {
        for (int i = 0; i < legs.Length && (OppositeLegGrounded(i) || legProgress[i] >= 0.01f || CheckDistanceFromTargetPoint(i) >= 4f); i++)
        {
            if (CheckDistanceFromTargetPoint(i) > _threshold)
            {
                UpdateCurrentLegPosition(i);
            }
        }
    }

    private bool OppositeLegGrounded(int _leg)
    {
        _leg = (_leg + 1) % legs.Length;
        return legProgress[_leg] < 0.01f;
    }

    private float CheckDistanceFromTargetPoint(int _leg)
    {
        return Vector3.Distance(currentPositions[_leg], targetPositions[_leg]);
    }

    private void UpdateCurrentLegPosition(int _leg)
    {
        currentPositions[_leg] = targetPositions[_leg];
        legProgress[_leg] = 1f;
    }

    private void LerpLegs()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            Transform target = legs[i].Target;
            legProgress[i] = Mathf.Lerp(legProgress[i], 0f, Time.deltaTime * legSpeed);
            Vector3 vector = Vector3.up * upAmount * legProgress[i];
            target.position = Vector3.Lerp(target.position, currentPositions[i] + vector, Time.deltaTime * legSpeed);
        }
    }

    public void OnDrawGizmos() { }

    public void CollectGarbage()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            Object.Destroy(legs[i].Target.gameObject);
        }
    }

    public void ForceCurrentPosition(int _index)
    {
        if (legProgress != null)
        {
            legProgress[_index] = 1f;
            legs[_index].Target.position = legs[_index].transform.position;
        }
    }
}
