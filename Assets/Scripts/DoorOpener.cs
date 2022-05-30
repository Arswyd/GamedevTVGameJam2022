// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [Header("Switch")]
    [SerializeField] SwitchHandler switchHandler;

    [Header("State")]
    [SerializeField] GameObject openState;
    [SerializeField] GameObject closedState;

    [SerializeField] bool isOpen = false;

    void Update() 
    {
        CheckIfOpen();

        if (switchHandler == null) { return; }
        if (isOpen == switchHandler.GetToggledState()) { return; }
        if (switchHandler.GetToggledState())
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }

    void CheckIfOpen()
    {
        openState.SetActive(isOpen);
        closedState.SetActive(!isOpen);
    }
}
