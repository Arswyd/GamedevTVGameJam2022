using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField] bool isToggledOnExit = true;
    [SerializeField] bool isToggled = false;

    Animator animator;
    bool isColliding = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!isColliding && other.tag == "Player")
        {
            isColliding = true;
            isToggled = !isToggled;
            CheckAnimationState(isToggled);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (isColliding && other.tag == "Player")
        {
            isColliding = false;
            if (isToggledOnExit) 
            {
                isToggled = !isToggled;
                CheckAnimationState(isToggled);
            }
        }
    }

    public bool GetToggledState()
    {
        return isToggled;
    }

    void CheckAnimationState(bool value)
    {
        animator.SetBool("Push", value);
    }
}
