using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    public Transform camRig;

    public float camMoveSpeed;
    public float camRotateSpeed;

    private Quaternion startRot;
    public static Quaternion targetRot;
    public static Direction camFaceDirection = Direction.N;

    private static IEnumerator camRotateRoutine;

    private void Awake()
    {
        startRot = camRig.rotation;
    }
    private void Update()
    {
        MoveCameraRig(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        


        if (Input.GetKeyDown(KeyCode.E)) RotateCamRigRight();
        if (Input.GetKeyDown(KeyCode.Q)) RotateCamRigLeft();
    }

    public void MoveCameraRig(Vector2 delta)
    {
        camRig.position += ((camRig.right - camRig.forward) * delta.x+ (camRig.right + camRig.forward) * delta.y) * camMoveSpeed * Time.deltaTime;
    }

    public void SmoothRotateCameraRigToTarget()
    {
        if (camRig.rotation == targetRot) return;
        if (camRotateRoutine != null) return;
        camRotateRoutine = SmoothRotateCameraRigToTargetRoutine();
        StartCoroutine(camRotateRoutine);     
    }

    public IEnumerator SmoothRotateCameraRigToTargetRoutine()
    {
        while(camRig.rotation != targetRot)
        {
            camRig.rotation = Quaternion.RotateTowards(camRig.rotation, targetRot, camRotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        camRotateRoutine = null;
    }

    public void RotateCameraToDirection(Direction direction)
    {
        camFaceDirection = direction;
        targetRot = startRot * Quaternion.Euler(Vector3.up * 45 * (int)direction);
        SmoothRotateCameraRigToTarget();
        Board.UpdateAllTextRotation(camMoveSpeed);
    }

    public void RotateCamRigRight()
    {
        RotateCameraToDirection((Direction)(((int)camFaceDirection + 2) % 8));
    }

    public void RotateCamRigLeft()
    {
        RotateCameraToDirection((Direction)(((int)camFaceDirection + 6) % 8));
    }
}
