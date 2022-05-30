using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    [Header("Switch")]
    [SerializeField] SwitchHandler switchHandler;

    [Header("Beams")]
    [SerializeField] GameObject[] beams;

    [Header("Flickering")]
    [SerializeField] float onTime = 0.5f;
    [SerializeField] float offTime = 0.5f;
    [SerializeField] float flickerOffset = 0f;
    [SerializeField] bool isFlickering = false;

    [Header("Operating")]
    [SerializeField] bool isOperating = false;

    float passedTime = 0f;

    void Awake() 
    {
        passedTime += flickerOffset;
    }

    void Update() 
    {
        if(isFlickering)
        {
            Flicker();
        }

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

    void Flicker()
    {
        passedTime += Time.deltaTime;
        if(isOperating == true && passedTime > onTime)
        {
            isOperating = false;
            ToggleBeams(false);
            passedTime = 0;
        }
        else if(isOperating == false && passedTime > offTime)
        {
            isOperating = true;
            ToggleBeams(true);
            passedTime = 0;
        }
    }
}
