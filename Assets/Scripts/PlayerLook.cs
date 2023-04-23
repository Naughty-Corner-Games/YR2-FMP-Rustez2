using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float sensX = 100f, sensY = 100f;
    [SerializeField] private Transform cam = null;
    [SerializeField] private Transform orientation = null;
    [SerializeField] private Camera fpscam;

    private float mouseX, mouseY;
    private float xRotation, yRotation;
    private float multiplier = 0.01f;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        fpscam = Camera.main;
        transform.rotation = Quaternion.Euler(mouseX, mouseY, transform.rotation.eulerAngles.z);
    }

    private void Update()
    {
        if(Time.timeScale == 1)
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}