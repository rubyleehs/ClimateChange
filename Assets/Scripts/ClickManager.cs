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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) OnClick();
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
        }
    }
}
