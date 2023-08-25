using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    UpdatedPlayerController abilitiesPC;
    public enum players { P1 , P2}
    public players currentPlayer;

    [Header("CooldownUI")]
    public GameObject cooldownui;
    public GameObject blastPackUI;
    public GameObject boomBotUI;
    public GameObject iceWallUI;


    public TextMeshProUGUI blastPackText;
    public TextMeshProUGUI boomBotText;
    public TextMeshProUGUI iceWallText;


    private Vector2 movementInput = Vector2.zero;

    [Header("Hurt Effect")]
    [SerializeField] Material hurtColor1, hurtColor2;

    [SerializeField] private SkinnedMeshRenderer bomberManRenderer;
    private Material defaultMaterial;

    private GameObject hurtPixel;
    [SerializeField] private RenderTexture HurtPixelRender;


    [Header("Audio")]
    AudioSource aS;
    public AudioClip hurtSound;
    public AudioClip macerenaSound;


    // Start is called before the first frame update
    void Start()
    {
        bomberManRenderer = bomberManRenderer.GetComponent<SkinnedMeshRenderer>();
        defaultMaterial = bomberManRenderer.material;

        aS = GetComponent<AudioSource>();

        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();

        lvlManager = FindObjectOfType<LevelManager>();

        playerCurrentHealth = playerMaxHealth;

        abilitiesPC = GetComponent<UpdatedPlayerController>();


        if (currentPlayer == players.P1)
        {
            healthBar = lvlManager.player1Hp;
            cooldownui = lvlManager.player1UI;
            hurtPixel = lvlManager.dmgEffectP1;
            gameObject.tag = "Player1";
        }
        else
        {
            healthBar = lvlManager.player2Hp;
            cooldownui = lvlManager.player2UI;
            hurtPixel = lvlManager.dmgEffectP2;
            gameObject.tag = "Player2";
        }


        healthBar.SetMaxHealth(playerMaxHealth);
        connectCds();
    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;
        movementInput = context.ReadValue<Vector2>();
        anim.SetBool("Dance", false);
        aS.Stop();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;
        //Debug.Log("Jumped");
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

    public void Rotate(InputAction.CallbackContext context)
    {
        cameraControlRef.GetComponent<CameraControl>().mousePos = context.ReadValue<Vector2>();
    }

    public void Emote(InputAction.CallbackContext context)
    {
        if (context.started && movementInput == Vector2.zero)
        {
            anim.SetBool("Dance", true);
            aS.PlayOneShot(macerenaSound);
        }
    }


    public void connectCds()
    {
        blastPackUI = cooldownui.transform.Find("BlastPackCD").gameObject;
        blastPackText = blastPackUI.GetComponent<TextMeshProUGUI>();

        boomBotUI = cooldownui.transform.Find("BoomBotCD").gameObject;
        boomBotText = boomBotUI.GetComponent<TextMeshProUGUI>();

        iceWallUI = cooldownui.transform.Find("IceWallCD").gameObject;
        iceWallText = iceWallUI.GetComponent<TextMeshProUGUI>();
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

        blastPackText.text = abilitiesPC.currentBlastPackCD.ToString("0");
        boomBotText.text = abilitiesPC.currentBoomBotCD.ToString("0");
        iceWallText.text = abilitiesPC.currentIceWallCD.ToString("0");

    }       

    public void TakeDamage(int damage)
    {
        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
            playerCurrentHealth -= damage;
            StartCoroutine(HurtEffect());

            if(playerCurrentHealth > 100) playerCurrentHealth = 100;


            healthBar.SetHealth(playerCurrentHealth);
        }       
    }


    IEnumerator HurtEffect()
    {
        aS.PlayOneShot(hurtSound);
        hurtPixel.SetActive(true);
        //Debug.Log(cameraControlRef.gameObject);
        cameraControlRef.gameObject.GetComponent<Camera>().targetTexture = HurtPixelRender;

        float timer = 0.1f;

        for(int i = 0; i<3; i++)
        {
            bomberManRenderer.material = hurtColor1;
            yield return new WaitForSeconds(timer);
            bomberManRenderer.material = hurtColor2;
            yield return new WaitForSeconds(timer);

            Debug.Log(timer);
            timer += 0.1f;

        }

        bomberManRenderer.material = defaultMaterial;
        hurtPixel.SetActive(false);
        cameraControlRef.gameObject.GetComponent<Camera>().targetTexture = null;
    }
}
