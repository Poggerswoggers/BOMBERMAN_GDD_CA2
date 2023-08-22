using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded;
    public int moveSpeed = 15;
    public float jumpHeight = 2f;
    public float gravityValue = -9.81f;
    [SerializeField] private Vector3 playerVelocity;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundcheckPos;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;

    public Animator anim;
    CharacterController cc;
    public CameraControl cameraControlRef;

    public enum players { P1 , P2}
    public players currentPlayer;



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundcheckPos.position, radius, whatIsGround);

        anim.SetBool("JumpTrigger", !isGrounded);
    

        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal" + currentPlayer.ToString()), 0, Input.GetAxisRaw("Vertical" + currentPlayer.ToString()));

        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.z);

        if (dir.magnitude >= 0.1f)
        {
            Vector3 displacement = transform.TransformDirection(dir.normalized);
            cc.Move(displacement * moveSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButton(1))
        {
            cameraControlRef.AdsMode(true);
        }
        else
        {
            cameraControlRef.AdsMode(false);
        }

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Has pressed Jumped");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
        

    }

    public void setTrigger()
    {
       
    }
}
