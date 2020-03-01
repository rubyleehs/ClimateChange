using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public float clickRaycastDistance = 100;
    private Camera mainCam;

    private Ray camRay;
    private RaycastHit camRayHit;
    private Transform camRayHitTransform;

    private IMouseOverable lastMouseOverable;

    // Update is called once per frame
    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        camRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out camRayHit, clickRaycastDistance)) camRayHitTransform = camRayHit.transform;
        else camRayHitTransform = null;


        OnClick(camRayHitTransform);
        OnMouseOver(camRayHitTransform);
        
    }

    public void OnClick(Transform hit)
    {
        if (hit == null) return;
        IClickable clickable = hit.GetComponent<IClickable>();
        if (clickable == null) return;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) clickable.OnClick();
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) clickable.OnClickDown();
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) clickable.OnClickUp();        
    }

    public void OnMouseOver(Transform hit)
    {
        
        IMouseOverable mouseOverable = null;
        if(hit != null) mouseOverable = hit.GetComponent<IMouseOverable>();
        if (lastMouseOverable != mouseOverable)
        {
            if (lastMouseOverable != null) lastMouseOverable.OnMouseExit();
            lastMouseOverable = mouseOverable;
            if (mouseOverable == null) return;
            mouseOverable.OnMouseEnter();
        }
        if(mouseOverable != null) mouseOverable.OnMouseOver();      
    }
}
