using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    [SerializeField] SwitchHandler switchHandler;
    [SerializeField] GameObject[] beams;

    bool isOperating = false;

    void Update() 
    {
        if (switchHandler == null) { return; }
        if (isOperating == switchHandler.GetToggledState()) { return; }
        if (isOperating)
        {
            isOperating = false;
            ToggleBeams(false);
        }
        else
        {
            isOperating = true;
            ToggleBeams(true);
        }
    }

    void ToggleBeams(bool value)
    {
        foreach(GameObject beam in beams) 
        {
            beam.SetActive(value);
        }
    }
}
