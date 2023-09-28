using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObj : MonoBehaviour
{
    Animator animator;
    public Transform obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animator)
        {
            if(obj!=null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(obj.position);
            }
        }
    }
}
