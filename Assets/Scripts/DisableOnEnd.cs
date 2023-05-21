using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script disables the gameobject's animator component, [for performance]
public class DisableOnEnd : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void OnClose()
    {
        animator.enabled = false;
    }
}
