using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private Camera mainCam;
    // Update is called once per frame
    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetButton("Fire1")) OnLeftClick();
        if (Input.GetButton("Fire2")) OnRightClick();
    }

    public void OnLeftClick()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit,))
        //{
        //}
    }

    public void OnRightClick()
    {

    }
}
