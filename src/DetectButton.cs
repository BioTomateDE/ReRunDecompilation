using UnityEngine;

public class DetectButton : MonoBehaviour
{
    public LayerMask whatIsButton;

    private MyButton currentButton;

    public void Update()
    {
        CheckInput();
        Transform _playerCam = PlayerMovement.Instance.playerCam;
        if (Physics.Raycast(_playerCam.position, _playerCam.forward, out var _hitInfo, 3.5f, whatIsButton))
        {
            if (!currentButton)
            {
                Crosshair.Instance.ChangeCrosshair(Crosshair.CrosshairMode.Button);
            }
            currentButton = _hitInfo.transform.parent.GetComponent<MyButton>();
        }
        else
        {
            if (currentButton)
            {
                Crosshair.Instance.ChangeCrosshair(Crosshair.CrosshairMode.Normal);
            }
            currentButton = null;
        }
    }

    private void CheckInput()
    {
        if (currentButton && Input.GetButtonDown("Use"))
        {
            currentButton.ActivateButton();
        }
    }
}
