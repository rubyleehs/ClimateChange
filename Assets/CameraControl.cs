using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform camRig;

    public float camMoveSpeed;
    public float camRotateSpeed;

    private Quaternion targetRot;
    
    private void Update()
    {
        MoveCameraRig(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        SmoothRotateCameraRig(targetRot);


        if(Input.GetButtonDown("Fire1")) RotateCamRigRight();
    }

    public void MoveCameraRig(Vector2 delta)
    {
        camRig.position += ((camRig.right - camRig.forward) * delta.x+ (camRig.right + camRig.forward) * delta.y) * camMoveSpeed * Time.deltaTime;
    }

    public void SmoothRotateCameraRig(Quaternion targetRot)
    {
        camRig.rotation = Quaternion.RotateTowards(camRig.rotation, targetRot, camRotateSpeed * Time.deltaTime);
    }

    public void RotateCamRigRight()
    {
        targetRot = camRig.rotation * Quaternion.Euler(Vector3.up * 90);
    }

    public void RotateCamRigLeft()
    {
        targetRot = camRig.rotation * Quaternion.Euler(Vector3.up * -90);
    }


}
