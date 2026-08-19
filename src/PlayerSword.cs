using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private List<Player> enemiesHit;

    public GameObject arrow;

    public RandomSfx sfx;

    public void Awake()
    {
        enemiesHit = new List<Player>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Player component = other.transform.root.GetComponent<Player>();
            if (!enemiesHit.Contains(component))
            {
                enemiesHit.Add(component);
                CameraShaker.Instance.ShakeOnce(6f, 8f, 0.2f, 0.2f);
                Player _player2 = other.transform.root.GetComponent<Player>();// probably redundant
                _player2.Damage(50 + (int)(PlayerMovement.Instance.GetVelocity().magnitude * 1.4f), other.transform.position);
                Vector3 _vector = PlayerMovement.Instance.playerCam.forward + (PlayerMovement.Instance.GetVelocity() * 0.08f);
                component.GetTorso().GetComponent<Rigidbody>().AddForce(_vector * 5000f);
                sfx.Randomize();
                Hitmarker.Instance.StartHitmarker();
            }
        }
    }

    public void ResetList()
    {
        enemiesHit = new List<Player>();
    }
}
