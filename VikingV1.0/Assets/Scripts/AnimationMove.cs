using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMove : MonoBehaviour
{

    Animator animator;
    int isRunningHash;
    int isWalkingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        //Move();   
    }
    void Move()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        bool forwardPressing = Input.GetKey("w") || Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("a");
        bool runPressing = Input.GetKey("left shift");

        if (!isWalking && forwardPressing)
        {
            animator.SetBool("isWalking", true);
        }
        if (isWalking && !forwardPressing)
        {
            animator.SetBool("isWalking", false);
        }
        if (!isRunning && forwardPressing && runPressing)
        {
            animator.SetBool("isRunning", true);
        }
        if (isRunning && (!forwardPressing || !runPressing))  
        {
            animator.SetBool("isRunning", false);
        }
    }
}
