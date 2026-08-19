using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject bullet;

	public GameObject muzzle;

	public Transform gunTip;

	public float force = 3000f;

	public float cooldown = 0.4f;

	private bool readyToShoot = true;

	public bool Shoot(Vector3 hitPoint)
	{
		if (!readyToShoot)
		{
			return false;
		}
		Vector3 normalized = (hitPoint - gunTip.position).normalized;
		Object.Instantiate(muzzle, gunTip.position, Quaternion.identity);
		Object.Instantiate(bullet, gunTip.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(normalized * force);
		readyToShoot = false;
		Invoke("GetReady", cooldown);
		return true;
	}

	private void GetReady()
	{
		readyToShoot = true;
	}
}
