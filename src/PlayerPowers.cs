using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
	public bool gun;

	public Weapon weapon;

	public static PlayerPowers Instance;

	public void Awake()
	{
		Instance = this;
	}

	public void DoubleJump()
	{
		PlayerMovement.Instance.maxJumps = 2;
		PlayerMovement.Instance.jumpsLeft = 2;
	}

	public void Gun()
	{
		gun = true;
		global::Gun.Instance.SpawnGun();
	}

	public void FireGun()
	{
		if (gun && weapon.Shoot(PlayerMovement.Instance.HitPoint()))
		{
			global::Gun.Instance.Shoot();
		}
	}

	public void Dash()
	{
	}

	public void Speed()
	{
	}
}
