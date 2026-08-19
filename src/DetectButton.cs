using UnityEngine;

public class DetectButton : MonoBehaviour
{
	public LayerMask whatIsButton;

	private MyButton currentButton;

	public void Update()
	{
		CheckInput();
		Transform playerCam = PlayerMovement.Instance.playerCam;
		if (Physics.Raycast(playerCam.position, playerCam.forward, out var hitInfo, 3.5f, whatIsButton))
		{
			if (!currentButton)
			{
				Crosshair.Instance.ChangeCrosshair(Crosshair.CrosshairMode.Button);
			}
			currentButton = hitInfo.transform.parent.GetComponent<MyButton>();
		}
		else
		{
			if ((bool)currentButton)
			{
				Crosshair.Instance.ChangeCrosshair(Crosshair.CrosshairMode.Normal);
			}
			currentButton = null;
		}
	}

	private void CheckInput()
	{
		if ((bool)currentButton && Input.GetButtonDown("Use"))
		{
			currentButton.ActivateButton();
		}
	}
}
