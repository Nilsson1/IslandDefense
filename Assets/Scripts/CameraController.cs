using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    private RaycastHit rightMouseHit;
    private RaycastHit leftMouseHit;
    private Ray ray;

    private int edgeSize = 10;
    private int speed = 10;
    private float moveCam = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        CameraOnScreenClick();
    }

    private void HandleMovement()
    {
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            Vector3 move = new Vector3(moveCam * Time.deltaTime * speed, 0,  0);
            transform.Translate(move, Space.World);
        }
        if (Input.mousePosition.x < 0 + edgeSize)
        {
            Vector3 move = new Vector3(-moveCam * Time.deltaTime * speed, 0, 0);
            transform.Translate(move, Space.World);
        }
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            Vector3 move = new Vector3(0, 0, moveCam * Time.deltaTime * speed);
            transform.Translate(move, Space.World);
        }
        if (Input.mousePosition.y < 0 + edgeSize)
        {
            Vector3 move = new Vector3(0, 0, -moveCam * Time.deltaTime * speed);
            transform.Translate(move, Space.World);
        }
    }

    private void CameraOnScreenClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out leftMouseHit))
            {
                GameObject gameObj = leftMouseHit.transform.gameObject;
                LeftMouseSelectEvent leftMouseSelect = new LeftMouseSelectEvent();
                leftMouseSelect.selectedGameObject = gameObj;
                leftMouseSelect.FireEvent();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out rightMouseHit))
            {
                RightMouseSelectEvent rightMouseClick = new RightMouseSelectEvent();
                rightMouseClick.rightClickGameObject = rightMouseHit.transform.gameObject;
                rightMouseClick.mousePosition = rightMouseHit.point;
                rightMouseClick.FireEvent();
            }
        }
    }
}
