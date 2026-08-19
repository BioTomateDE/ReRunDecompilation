using DitzelGames.FastIK;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Transform target;

	public Transform hips;

	private float speed = 4f;

	public Weapon gun;

	public FastIKFabric ik;

	private void Start()
	{
		target = PlayerMovement.Instance.transform;
		ReportToGameManager();
		ik.Target = target;
	}

	private void ReportToGameManager()
	{
		GameManager.Instance.AddEnemy(base.gameObject);
	}

	private void Update()
	{
		if (!GameManager.Instance.isRewinding && (bool)target)
		{
			Vector3 vector = target.transform.position - base.transform.position;
			if (Vector3.Angle(base.transform.forward, vector) > 10f)
			{
				hips.transform.rotation = Quaternion.Slerp(hips.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * speed);
			}
			hips.transform.rotation = Quaternion.Euler(0f, hips.transform.rotation.eulerAngles.y, 0f);
			ShootLogic();
		}
	}

	private void ShootLogic()
	{
		Vector3 normalized = (target.position - gun.transform.position).normalized;
		if (Physics.Raycast(gun.transform.position, normalized, out var hitInfo, 100f) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			gun.cooldown = Random.Range(0.6f, 2f);
			gun.Shoot(hitInfo.point);
		}
	}
}
