using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public float clickRaycastDistance = 100;
    private Camera mainCam;

    // Update is called once per frame
    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) OnClick();
    }


    public void OnClick()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, clickRaycastDistance))
        {
            IClickable clickable = hit.transform.GetComponent<IClickable>();
            if (clickable == null) return;

            clickable.OnClick();
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) clickable.OnClickDown();
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) clickable.OnClickUp();
        }
    }
}
