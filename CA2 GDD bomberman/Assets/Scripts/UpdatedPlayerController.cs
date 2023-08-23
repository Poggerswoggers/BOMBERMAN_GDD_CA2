using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdatedPlayerController : MonoBehaviour
{
    public Camera cam;
    
    public Cinemachine3rdPersonAim tpsAimCam;

    [Header("Grenade")]
    public GameObject bombPrefab;
    public float throwForce = 10f;
    public float throwWindUpRate = 10f;
    public float maxThrowForce = 40f;
    public float throwCooldown = 2.5f;
    public float upwardsThrowForce = 2f;
    float currentThrowCooldown;
    bool onCooldown = false;

    [Header("IceWall")]
    private bool isUsingWall = false;
    public float iceWallCD = 14f; 
    public float currentIceWallCD = 0f;
    public KeyCode wallCastKeybind, directionKeybind;
    public float wallRange;
    public GameObject iceWallPreview, iceWallObject;
    public LayerMask layermask;
    private bool direction, casting;
   

    [Header("BoomBot")]
    public GameObject boomBotPrefab;
    public float boomBotCD = 12f;
    public float currentBoomBotCD = 0f;
    public KeyCode boomBotKeybind;
    public float spawnRange;


    [SerializeField] private Transform debugTransform;

    public Transform firePoint;

    PlayerController pc;
    LevelManager lvlManager;
    Animator anim;


    //NOTE: THIS PLAYER CONTROLLER IS MOSTLY FOR ABILITIES, THE MAIN ONE IS FOR MOVEMENT 


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        lvlManager = FindObjectOfType<LevelManager>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
        }


        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, cam.nearClipPlane);
        Ray ray = cam.ViewportPointToRay(viewportCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tpsAimCam.AimCollisionFilter))
        {
            debugTransform.position = raycastHit.point;
        }



        if (Input.GetButton("Fire1") && !isUsingWall)
        {
            if (!onCooldown)
            {
                if (throwForce >= maxThrowForce) return;
                anim.SetBool("IsChargingBomb", true);
            }

            throwForce += throwWindUpRate * Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1") && !isUsingWall)
        {
            if (!onCooldown)
            {

                anim.SetTrigger("ThrowingBomb");
                anim.SetBool("IsChargingBomb", false);

                ThrowBomb();
                currentThrowCooldown = throwCooldown;
                onCooldown = true;
            }
            throwForce = 10f;
            
        }

        //bomb cooldown timer
        if (onCooldown && currentThrowCooldown > 0.001)
        {
            currentThrowCooldown -= Time.deltaTime;
        }
        else onCooldown = false;


        if (casting) CastingIceWall();

        if (Input.GetKeyDown(wallCastKeybind) && currentIceWallCD <= 0)
        {
            casting = !casting; //casting = false;
            if (!casting) iceWallPreview.SetActive(false);
            isUsingWall = true;

           
        }
        //ice wall cooldown
        else if(currentIceWallCD > 0) currentIceWallCD -= Time.deltaTime;

        if(Input.GetKeyDown(boomBotKeybind) && currentBoomBotCD <= 0)
        {
            CastingBoomBot();
        }
        //boombot cooldown
        else if(currentBoomBotCD > 0) currentBoomBotCD -= Time.deltaTime;
        

    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        //quaternion.lookRot(aimDir);

        Vector3 aimDirection = (debugTransform.position - firePoint.position).normalized;
        Vector3 forceToAdd = aimDirection * throwForce + transform.up * upwardsThrowForce;
        rb.AddForce(forceToAdd, ForceMode.VelocityChange);

        Debug.Log(aimDirection);

    }

    void CastingIceWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wallRange, layermask))
        {
            if (!iceWallPreview.activeSelf)
            {
                iceWallPreview.SetActive(true);
            }

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (direction) rotation.y = 1; //direction is toggle
            else rotation.y = 0;

            iceWallPreview.transform.localRotation = rotation;
            iceWallPreview.transform.position = hit.point;

            if (Input.GetButtonUp("Fire1"))
            {
                Instantiate(iceWallObject, hit.point, iceWallPreview.transform.rotation);
                casting = false;
                isUsingWall = false;
                iceWallPreview.SetActive(false);
                currentIceWallCD = iceWallCD;
            }

        }
        else { iceWallPreview.SetActive(false); }


        if (Input.GetKeyDown(directionKeybind))
        {
            direction = !direction;
            
        }

    }

    void CastingBoomBot()
    {
        Vector3 spawnPos = cam.transform.forward * spawnRange;
        Instantiate(boomBotPrefab, spawnPos, gameObject.transform.rotation);
        if (gameObject.CompareTag("Player1"))
        {
            boomBotPrefab.GetComponent<BoomBot>().GetOtherPlayer("Player2");
        }
        else if (gameObject.CompareTag("Player2"))
        {
            boomBotPrefab.GetComponent<BoomBot>().GetOtherPlayer("Player1");
        }

        currentBoomBotCD = boomBotCD;
    }
}
