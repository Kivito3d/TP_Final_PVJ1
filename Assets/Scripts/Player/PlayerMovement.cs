using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float speed;

    [SerializeField] private PlayerStats stats;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isStop=false;
    public bool IsStop
    {
        get {return isStop; }
        set { isStop=value; }
    }

    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody rb;

    [Header ("Raycast Settings")]
    [SerializeField]private bool isGrounded;

    [SerializeField] private float rayDistance;

    [SerializeField] private LayerMask groundLayer;

    [Header ("Animation Settings")]

    private Animator animator;
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int GroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //


        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        speed = Input.GetKey(KeyCode.LeftShift) ? stats.runSpeed : stats.walkSpeed;

        CheckGround();

        Vector2 inputDir = new Vector2(moveHorizontal, moveVertical);

        animator.SetFloat(SpeedHash, inputDir.magnitude * speed);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            animator.SetTrigger(JumpHash);
            //animator.SetTrigger("Jump");

        }


    }

    private void FixedUpdate()
    {
        Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);

    }
    private void CheckGround()
    {
        Vector3 origin = transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        Color ColorRay = isGrounded ? Color.green : Color.red;

        animator.SetBool(GroundedHash,isGrounded);

        Debug.DrawRay(origin,Vector3.down*rayDistance,ColorRay);
    }

    public void RotateCharacter(float rotateY)
    {
        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
    }


}

