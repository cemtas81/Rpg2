using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public Camera mainCamera;
    bool isDragging = false;
    ClickableObject CO;

    int forceButton = 0;
    int breakButton = 1;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;

        if (PlayerPrefs.HasKey("swapButtons"))
        {
            if (PlayerPrefs.GetInt("swapButtons") == 1)
            {
                forceButton = 1;
                breakButton = 0;
            }
            else
            {
                forceButton = 0;
                breakButton = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Left click down -- telekinesis start
        if (Input.GetMouseButtonDown(forceButton) && !UIManager.isPaused)
        {
            isDragging = true;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                CO = hit.collider.gameObject.GetComponent<ClickableObject>();
                if (CO != null)
                {
                    CO.StartTelekinesisPower(mainCamera);
                }
            }
        }
        // Left click up -- telekinesis end
        if (Input.GetMouseButtonUp(forceButton) || (UIManager.isPaused && isDragging))
        {
            isDragging = false;
            if (CO != null)
            {
                CO.StopTelekinesisPower();
                CO = null;
            }
        }

        // Right click -- breaker
        if (Input.GetMouseButtonUp(breakButton) && !isDragging && !UIManager.isPaused)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                CO = hit.collider.gameObject.GetComponent<ClickableObject>();
                if (CO != null)
                {
                    CO.BreakPower();
                }
            }
            CO = null;
        }
    }
}
