using System;
using UnityEngine;

[RequireComponent(typeof(IkEnemy))]
public class RigidEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        active = 0,
        tumbling = 1,
        falling = 2,
        recovering = 3,
        dead = 4
    }

    public Transform root;

    public Transform head;

    public Transform torso;

    private Rigidbody rb;

    private Rigidbody headRb;

    private Rigidbody torsoRb;

    private float force;

    [HideInInspector]
    public EnemyState state = EnemyState.recovering;

    [HideInInspector]
    public IkEnemy ik;

    private Transform[] groundChecks;

    public bool minOneGrounded;

    private int nLegs;

    public float legPushForce = 0.55f;

    public float groundCheckRadius = 0.2f;

    public float moveLegsWithSpeedScale = 0.25f;

    public float moveSpeed = 10f;

    private float rotationForce = 0.01f;

    public float maxRotationForce = 0.05f;

    private float stabilizeForce = 1f;

    public float recoverTime = 2f;

    public float recoveryForce = 0.3f;

    public float tumbleAngle = 30f;

    public float fallAngle = 70f;

    public float getupMagT = 0.2f;

    public float getupAng = 15f;

    private bool ragdoll;

    [HideInInspector]
    public bool recovering;

    public void Start()
    {
        rb = root.GetComponent<Rigidbody>();
        if (head)
        {
            headRb = head.GetComponent<Rigidbody>();
        }
        else
        {
            headRb = rb;
        }
        if (torso)
        {
            torsoRb = torso.GetComponent<Rigidbody>();
        }
        else
        {
            torsoRb = rb;
        }
        ik = GetComponent<IkEnemy>();
        CaluclateForce();
        UpdateState(EnemyState.active);
        nLegs = ik.legs.Length;
        groundChecks = new Transform[nLegs];
        for (int i = 0; i < nLegs; i++)
        {
            groundChecks[i] = ik.legs[i].transform;
        }
        DisableSelfCollision(_ignore: true);
    }

    public void FixedUpdate()
    {
        if (state == EnemyState.dead)
        {
            return;
        }
        minOneGrounded = false;
        for (int i = 0; i < nLegs; i++)
        {
            if (Physics.CheckSphere(groundChecks[i].position, groundCheckRadius, ik.whatIsGround))
            {
                minOneGrounded = true;
            }
        }
        float _totalDist = 0f;
        if (state == EnemyState.active || state == EnemyState.tumbling || state == EnemyState.recovering || state == EnemyState.falling)
        {
            if (!Physics.Raycast(root.position, Vector3.down, out RaycastHit hitInfo, ik.heightAboveGround * 3f, ik.whatIsGround))
            {
                UpdateState(EnemyState.falling);
            }
            else
            {
                _totalDist = hitInfo.distance;
            }
        }
        float _angle = Vector3.Angle(Vector3.up, root.up);
        if (state == EnemyState.falling)
        {
            if (_totalDist != 0f && _totalDist < ik.heightAboveGround * 1.5f && _angle < 50f)
            {
                UpdateState(EnemyState.active);
                CancelInvoke("GetUp");
                ConfigureLegs(_makeRagdoll: false);
                recovering = false;
            }
            else if (!IsInvoking("GetUp"))
            {
                Invoke("GetUp", recoverTime);
            }
            return;
        }
        if (state == EnemyState.recovering)
        {
            bool _collides = Physics.CheckSphere(root.position, 0.5f, ik.whatIsGround);
            if (_totalDist < ik.heightAboveGround || _collides)
            {
                headRb.AddForce(Vector3.up * force * recoveryForce * 1.1f);
                rb.AddForce(Vector3.up * force * recoveryForce * 0.9f);
            }
            if ((_angle < getupAng && torsoRb.velocity.magnitude < getupMagT) || (_totalDist > ik.heightAboveGround * 0.85f && _totalDist < ik.heightAboveGround * 1.85f && _angle < 30f))
            {
                UpdateState(EnemyState.active);
                CancelInvoke("RecoveryCooldown");
                Invoke("RecoveryCooldown", 2f);
            }
            return;
        }
        if (state == EnemyState.active && rb.velocity.magnitude < 1f && _totalDist > ik.heightAboveGround && _totalDist < ik.heightAboveGround + (ik.heightAboveGround * 0.1f))
        {
            headRb.AddForce(Vector3.up * force * 0.86f);
            return;
        }
        float _height = Mathf.Clamp(1f - (RootHeight() / ik.heightAboveGround), -1f, 1f);
        if (_angle < tumbleAngle)
        {
            UpdateState(EnemyState.active);
        }
        else if (_angle < fallAngle)
        {
            UpdateState(EnemyState.tumbling);
        }
        else if (_angle > fallAngle)
        {
            UpdateState(EnemyState.falling);
        }
        if (minOneGrounded)
        {
            rb.AddForce(root.up * force * _height * 2f);
            rb.AddForce(root.up * force * legPushForce);
        }
        if (_totalDist < ik.heightAboveGround * 2f)
        {
            StabilizingBody();
        }
    }

    public void RecoveryCooldown()
    {
        recovering = false;
    }

    public void Concuss()
    {
        UpdateState(EnemyState.falling);
        ConfigureLegs(_makeRagdoll: true);
        recovering = true;
        Invoke("GetUp", recoverTime * UnityEngine.Random.Range(0.7f, 1.5f));
    }

    public void GetUp()
    {
        if (Physics.CheckSphere(root.position, ik.heightAboveGround * 0.5f, ik.whatIsGround))
        {
            UpdateState(EnemyState.recovering);
            ConfigureLegs(_makeRagdoll: false);
        }
        else
        {
            Invoke("GetUp", recoverTime);
        }
    }

    private void ConfigureLegs(bool _makeRagdoll)
    {
        if (_makeRagdoll == ragdoll)
        {
            return;
        }
        ragdoll = _makeRagdoll;
        for (int i = 0; i < ik.legs.Length; i++)
        {
            int _chainLength = ik.legs[i].ChainLength;
            Transform _parent = ik.legs[i].transform;
            while (_chainLength > 0)
            {
                _parent = _parent.parent;
                if (_makeRagdoll)
                {
                    _parent.gameObject.AddComponent<CharacterJoint>().connectedBody = _parent.parent.GetComponent<Rigidbody>();
                }
                else
                {
                    UnityEngine.Object.Destroy(_parent.gameObject.GetComponent<Joint>());
                }
                _chainLength--;
            }
            Rigidbody[] componentsInChildren = _parent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody obj in componentsInChildren)
            {
                obj.isKinematic = !_makeRagdoll;
                obj.interpolation = _makeRagdoll ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
            }
            ik.legs[i].enabled = !_makeRagdoll;
            ik.ForceCurrentPosition(i);
        }
    }

    private float RootHeight()
    {
        if (Physics.Raycast(root.position, Vector3.down, out var hitInfo, 10f, ik.whatIsGround))
        {
            return hitInfo.distance;
        }
        return 0f;
    }

    private void StabilizingBody()
    {
        headRb.AddForce(Vector3.up * force * stabilizeForce);
        torsoRb.AddForce(Vector3.down * force * stabilizeForce);
    }

    private void CaluclateForce()
    {
        float _totalMass = 0f;
        Rigidbody[] _bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody _body in _bodies)
        {
            if (!_body.isKinematic)
            {
                _totalMass += _body.mass;
            }
        }
        force = _totalMass * (-Physics.gravity.y);
    }

    public void RotateBody(Vector3 _dir)
    {
        float _y = root.transform.eulerAngles.y;
        float _y2 = Quaternion.LookRotation(_dir).eulerAngles.y;
        float _deltaAngle = Mathf.DeltaAngle(_y, _y2);
        _deltaAngle = Mathf.Clamp(_deltaAngle, -2f, 2f);
        rb.AddTorque(Vector3.up * _deltaAngle * force * rotationForce);
    }

    public void MoveBody(Vector3 _dir)
    {
        rb.AddForce(_dir * moveSpeed * rb.mass);
        headRb.AddForce(_dir * moveSpeed * headRb.mass);
        torsoRb.AddForce(_dir * moveSpeed * torsoRb.mass);
    }

    public void UpdateState(EnemyState _newState)
    {
        if (state == _newState)
        {
            return;
        }
        state = _newState;
        switch (_newState)
        {
            case EnemyState.active:
                ConfigureRb(5f, 5f, maxRotationForce, 1f);
                break;
            case EnemyState.tumbling:
                ConfigureRb(1f, 4f, 0f, 0.1f);
                break;
            case EnemyState.falling:
                ConfigureRb(0f, 0f, 0f, 0f);
                Concuss();
                break;
            case EnemyState.recovering:
                ConfigureRb(4f, 4f, maxRotationForce, 0.15f);
                break;
            case EnemyState.dead:
                ConfigureRb(0f, 0f, 0f, 0f);
                KillRigidEnemy();
                break;
            default:
                rb.drag = 0f;
                rb.angularDrag = 0f;
                break;
        }
    }

    public void KillRigidEnemy()
    {
        DisableSelfCollision(_ignore: false);
        ConfigureLegs(_makeRagdoll: true);
        CancelInvoke();
        Transform[] _transforms = base.transform.GetComponentsInChildren<Transform>();
        foreach (Transform _transform in _transforms)
        {
            if (_transform.CompareTag("GrapplePoint"))
            {
                UnityEngine.Object.Destroy(_transform.gameObject);
            }
            _transform.tag = "Dead";
        }
        ik.CollectGarbage();
        base.gameObject.AddComponent<DestroyObject>().time = 10f;
        UnityEngine.Object.Destroy(this);
        UnityEngine.Object.Destroy(ik);
    }

    private void ConfigureRb(float _drag, float _angularDrag, float _rotation, float _stabilize)
    {
        if (_drag != -1f)
        {
            rb.drag = _drag;
            torsoRb.drag = _drag;
        }
        if (_angularDrag != -1f)
        {
            rb.angularDrag = _angularDrag;
            torsoRb.angularDrag = _angularDrag;
        }
        if (rotationForce != -1f)
        {
            // ^^^ this is probably a bug, it should check _rotation instead of rotationForce
            rotationForce = _rotation;
        }
        if (_stabilize != -1f)
        {
            stabilizeForce = _stabilize;
        }
    }

    public Vector3 GetVelocity()
    {
        if (!rb)
        {
            return Vector3.zero;
        }
        Vector3 _vector = rb.velocity * moveLegsWithSpeedScale;
        if (_vector.magnitude > 1f)
        {
            return _vector.normalized;
        }
        return _vector;
    }

    private void DisableSelfCollision(bool _ignore)
    {
        try
        {
            Collider[] _colliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < _colliders.Length; i++)
            {
                for (int j = i; j < _colliders.Length; j++)
                {
                    Physics.IgnoreCollision(_colliders[i], _colliders[j], _ignore);
                }
            }
        }
        catch (Exception)
        {
            // very good code dani
        }
    }
}
