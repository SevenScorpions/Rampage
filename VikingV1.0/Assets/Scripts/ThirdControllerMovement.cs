using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ThirdControllerMovement : MonoBehaviour
{
    Animator animator;
    public CharacterController controller;
    public Transform cam;

    public float sprint = 9;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float turnSmoothTime = 0.1f;
    public float attackSpeed = 1;
    public float blockTime = 1f;
    public AudioSource audioSource;

    Vector3 velocity;
    bool isGrounded;
    private float turnSmoothVelocity;
    private bool isWalking;
    private bool isRunning;
    private bool isAttacking;
    private string currentState;
    private bool movePressing;
    private bool jumpPressing;
    private bool sprintPressing;
    private bool attackPressing;
    private bool attackQueue = false;
    private bool attackComplete;
    private bool animationComplete;
    private float attackCountdown;
    private float animationCountdown;
    private bool blockPressing;
    private bool blockQueue;
    private float currentAngle;


    const string PLAYER_IDLE = "idle";
    const string PLAYER_ATTACKING = "attacking";
    const string PLAYER_OUT = "out";
    const string PLAYER_WALK = "walk";
    const string PLAYER_RUN = "run";
    const string PLAYER_ATTACK = "attack";
    const string PLAYER_ATTACK_1 = "attack_1";
    const string PLAYER_ATTACK_2 = "attack_2";
    const string PLAYER_ATTACK_3 = "attack_3";
    const string PLAYER_ATTACK_4 = "attack_4";
    const string PLAYER_FORWARD = "forward";
    const string PLAYER_BACKWARD = "backward";
    const string PLAYER_RIGHT = "right";
    const string PLAYER_LEFT = "left";
    private bool leftPressing;
    private bool rightPressing;
    private bool forwardPressing;
    private bool backwardPressing;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        currentState = PLAYER_IDLE;
        attackCountdown = Time.time;
        animationCountdown = Time.time;
        attackQueue = false;
    }
    void ChangeAnimationState(string newSate)
    {
        if(currentState!=newSate)
        {
            currentState = newSate;
        }
    }
    // Update is called once per frame
    void Update()
    {
        attackComplete = Time.time >= attackCountdown;
        animationComplete = Time.time >= animationCountdown;

        jumpPressing = Input.GetKeyDown(KeyCode.Space);
        movePressing = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        forwardPressing= Input.GetKey(KeyCode.W);
        backwardPressing = Input.GetKey(KeyCode.S);
        leftPressing = Input.GetKey(KeyCode.A);
        rightPressing= Input.GetKey(KeyCode.D);
        
        sprintPressing = Input.GetKey(KeyCode.LeftShift);
        attackPressing = Input.GetKeyDown(KeyCode.Mouse0);
        blockPressing = Input.GetKeyDown(KeyCode.Mouse1);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isWalking = animator.GetBool(PLAYER_WALK);
        isRunning = animator.GetBool(PLAYER_RUN);
        isAttacking = animator.GetBool(PLAYER_ATTACKING);
        if (attackPressing && isGrounded && currentState!=PLAYER_ATTACK_4)
        {
            attackQueue = true;
        }
        Gravity();
        if(!attackQueue && animationComplete)
        {
            if(jumpPressing && isGrounded)
            {
                Jump();
            }
            Move(sprint);
            if (leftPressing)
            {
                if(!forwardPressing&&!backwardPressing)
                animator.SetBool(PLAYER_LEFT, true);
                {
                    currentState = PLAYER_LEFT;
                }
            }
            else if(animator.GetBool(PLAYER_LEFT))
            {
                animator.SetBool(PLAYER_LEFT,false);
            }
            if(rightPressing)
            {
                animator.SetBool(PLAYER_RIGHT, true);
                if (!forwardPressing && !backwardPressing)
                {
                    currentState = PLAYER_RIGHT;
                }
            }
            else if(animator.GetBool(PLAYER_RIGHT))
            {
                animator.SetBool(PLAYER_RIGHT,false);
            }

            if (forwardPressing)
            {
                currentState = PLAYER_FORWARD;
                animator.SetBool(PLAYER_FORWARD, true);
            }
            else if (animator.GetBool(PLAYER_FORWARD))
            {
                animator.SetBool(PLAYER_FORWARD, false);
            }
            if (backwardPressing)
            {
                animator.SetBool(PLAYER_BACKWARD, true);
                currentState = PLAYER_BACKWARD;
            }
            else if (animator.GetBool(PLAYER_BACKWARD))
            {
                animator.SetBool(PLAYER_BACKWARD, false);
            }

            if (!movePressing || !isGrounded)
            {
                currentState = PLAYER_IDLE;
                audioSource.enabled = false;
            }
            else
            {
                audioSource.enabled=true;
            }
        }
        if (isAttacking && animationComplete)
        {
            animator.SetBool(PLAYER_ATTACKING, false);
            currentState = PLAYER_IDLE;
            animator.SetTrigger(PLAYER_OUT);
        }
        
        if (attackQueue && attackComplete)
        {
            attackQueue = false;
            audioSource.enabled = false;
            if (isWalking)
            {
                animator.SetBool(PLAYER_WALK, false);
            }
            if(isRunning)
            {
                animator.SetBool(PLAYER_RUN, false);
            }    
            Attack();
            currentAngle = cam.eulerAngles.y;
        }
    }
    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        animator.SetTrigger("jump");
    }
    void Move(float speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;   
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
    }
    void Gravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void Attack()
    {
        switch(currentState)
        {
            case PLAYER_ATTACK_1:
                ChangeAnimationState(PLAYER_ATTACK_2);
                break;
            case PLAYER_ATTACK_2:
                ChangeAnimationState(PLAYER_ATTACK_3);
                break;
            case PLAYER_ATTACK_3:
                ChangeAnimationState(PLAYER_ATTACK_4);
                break;
            default:
                ChangeAnimationState(PLAYER_ATTACK_1);
                break;
        }
        animator.SetTrigger(PLAYER_ATTACK);
        if(!isAttacking)
        {
            animator.SetBool(PLAYER_ATTACKING, true);
        }
        animationCountdown = Time.time + 1.2f/attackSpeed;
        attackCountdown = Time.time + 1.2f/attackSpeed - 0.3f;
        Invoke("ApplyDamage", 0.2f / attackSpeed);
    }
    public Transform attackPoint;
    public float attackRange = 0f;
    public LayerMask EnemyLayer;
    void ApplyDamage()
    {
        System.Random r = new System.Random();
        int i = r.Next(0, 10) % 2;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange,EnemyLayer);
        if (hitEnemies.Length > 0)
        {
            i += 2;   
        }
        AudioManager.instance.PlayOneShot("attackSound" + i);
        foreach (Collider enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(25);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(attackPoint!= null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
