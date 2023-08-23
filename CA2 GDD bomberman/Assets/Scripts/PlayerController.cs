using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;
    public HealthBar healthBar;


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
    LevelManager lvlManager;

    public enum players { P1 , P2}
    public players currentPlayer;

    private Vector2 movementInput = Vector2.zero;





    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();

        lvlManager = FindObjectOfType<LevelManager>();

        playerCurrentHealth = playerMaxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(playerMaxHealth);
        }

    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumped");
        if (context.performed && isGrounded)
        {
            
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }          
    }

    public void onAds(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraControlRef.AdsMode(true);
        }
        if (context.canceled)
        {
            cameraControlRef.AdsMode(false);
        }
    }

    public void OnBombing(InputAction.CallbackContext context)
    {
        if (context.performed) ;
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        cameraControlRef.GetComponent<CameraControl>().mousePos = context.ReadValue<Vector2>();
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundcheckPos.position, radius, whatIsGround);

        anim.SetBool("JumpTrigger", !isGrounded);


        //Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal" + currentPlayer.ToString()), 0, Input.GetAxisRaw("Vertical" + currentPlayer.ToString()));
        Vector3 dir = new Vector3(movementInput.x, 0, movementInput.y);

        anim.SetFloat("MoveX", dir.x);
        anim.SetFloat("MoveY", dir.z);

        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
        }


        if (dir.magnitude >= 0.1f)
        {
            Vector3 displacement = transform.TransformDirection(dir.normalized);
            cc.Move(displacement * moveSpeed * Time.deltaTime);
        }


        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }

        

        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
        

    }       

    public void TakeDamage(int damage)
    {
        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
            playerCurrentHealth -= damage;


            healthBar.SetHealth(playerCurrentHealth);
        }       
    }
}
