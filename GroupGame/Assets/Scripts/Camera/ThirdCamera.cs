using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    private float rotationY, rotationX;
    private Quaternion rotationP, rotationT;

    public float rotateSpeed = 1f;

    public Transform target, player;

        void CameraRotate()
    {
        rotationY -= Input.GetAxis("Mouse Y") * rotateSpeed;
        rotationX += Input.GetAxis("Mouse X") * rotateSpeed;
        rotationY = Mathf.Clamp(rotationY, -35, 60);

        rotationP = player.rotation;
        rotationT = target.rotation;

        player.rotation = Quaternion.Lerp(rotationP, Quaternion.Euler(0f, rotationX, 0f), 0.2f);
        target.rotation = Quaternion.Lerp(rotationT, Quaternion.Euler(rotationY, rotationX, 0f), 0.2f);

        transform.LookAt(target);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotate();
    }
}
