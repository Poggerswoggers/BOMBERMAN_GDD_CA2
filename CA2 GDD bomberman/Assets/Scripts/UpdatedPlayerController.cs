using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.InputSystem;

public class UpdatedPlayerController : MonoBehaviour
{
    public Camera cam;
    
    public Cinemachine3rdPersonAim tpsAimCam;

    public float teleportCD = 0f;

    [Header("Grenade")]
    public bool isCharging; 

    public GameObject bombPrefab;
    public float throwForce = 10f;
    public float throwWindUpRate = 10f;
    public float maxThrowForce = 40f;
    public float throwCooldown = 2.5f;
    public float upwardsThrowForce = 2f;
    float currentThrowCooldown;
    bool onCooldown = false;

    [Header("IceWall")]
    public bool canCastIceWall;

    [SerializeField] private bool isUsingWall = false;
    public float iceWallCD = 14f; 
    public float currentIceWallCD = 0f;
    public KeyCode wallCastKeybind, directionKeybind;
    public float wallRange;
    public GameObject iceWallPreview, iceWallObject;
    public LayerMask layermask;
    [SerializeField] private bool direction, casting;

    public bool changeDir;
   

    [Header("BoomBot")]
    public GameObject boomBotPrefab;
    public float boomBotCD = 12f;
    public float currentBoomBotCD = 0f;
    public KeyCode boomBotKeybind;
    public float spawnRange;

    [Header("BlastPack")]
    public GameObject blastPackPrefab;
    public float blastPackThrowForce = 30f;
    public float blastPackCD = 6f;
    public float currentBlastPackCD = 0f;
    public float torque = 1f;
    public KeyCode blastPackKeybind;
    


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


    public void OnFire(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;

        if (context.started)
        {
            if (isUsingWall)
            { 
                canCastIceWall = true;
            }
            else
            {
                isCharging = true;
            }
        }


        if(context.canceled)
        {
            if (canCastIceWall)
            {
                canCastIceWall = false;
            }


            else if(!onCooldown)
            {   
                isCharging = false;

                anim.SetTrigger("ThrowingBomb");
                anim.SetBool("IsChargingBomb", false);            

                ThrowBomb();
                currentThrowCooldown = throwCooldown;
                onCooldown = true;
            }
            throwForce = 10f;
        }
        else
        {
            //canCastIceWall = false;
        }
    }

    public void preIceWall(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;
        if (context.started)
        {
            isUsingWall = true;
        }
        if (context.canceled)
        {
            isUsingWall = false;
            iceWallPreview.SetActive(false);
        }
    }


    public void OnBoomBot(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;
        if (context.started && currentBoomBotCD <= 0)
        {
            CastingBoomBot();
        }
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (lvlManager.gameBegin == false) return;
        if (context.started && currentBlastPackCD <= 0)
        {
            ThrowBlastPack();
            currentBlastPackCD = blastPackCD;
        }
    }

    public void OnChangeDir(InputAction.CallbackContext context)
    {
        if (context.started && !changeDir)
        {
            changeDir = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        

        if (lvlManager != null)
        {
            if (lvlManager.gameOver) return;
        }


        //teleport CD
        if(teleportCD >= 0)
        {
            teleportCD -= Time.deltaTime;
        }

        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, cam.nearClipPlane);
        Ray ray = cam.ViewportPointToRay(viewportCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tpsAimCam.AimCollisionFilter))
        {
            debugTransform.position = raycastHit.point;
        }



        if (isCharging && !isUsingWall)
        {
            //Debug.Log("tewwst");
            if (!onCooldown)
            {
                if (throwForce >= maxThrowForce) return;
                anim.SetBool("IsChargingBomb", true);
                throwForce += throwWindUpRate * Time.deltaTime;
            }

            //throwForce += throwWindUpRate * Time.deltaTime;
        }


        //if (Input.GetButtonUp("Fire1") && !isUsingWall)
        {
            //if (!onCooldown)
            {

                //anim.SetTrigger("ThrowingBomb");
                //anim.SetBool("IsChargingBomb", false);

                //ThrowBomb();
                //currentThrowCooldown = throwCooldown;
                //onCooldown = true;
            }
            //throwForce = 10f;
            
        }

        //bomb cooldown timer
        if (onCooldown && currentThrowCooldown > 0.001)
        {
            currentThrowCooldown -= Time.deltaTime;
        }
        else onCooldown = false;



        if (isUsingWall && currentIceWallCD <= 0)
        {
            CastingIceWall();
            //if (!casting) iceWallPreview.SetActive(false);
            //isUsingWall = true;         
        }


        //ice wall cooldown
        else if(currentIceWallCD > 0) currentIceWallCD -= Time.deltaTime;


        //USE BOOM BOT
        //if(Input.GetKeyDown(boomBotKeybind) && currentBoomBotCD <= 0)
        //{
            //CastingBoomBot();
        //}
        //boombot cooldown
        if(currentBoomBotCD > 0) currentBoomBotCD -= Time.deltaTime;
        
        //if(Input.GetKeyDown(blastPackKeybind) && currentBlastPackCD <= 0)
        //{
            //ThrowBlastPack();
            //currentBlastPackCD = blastPackCD;
        //}
        if(currentBlastPackCD  > 0) currentBlastPackCD -= Time.deltaTime;

    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        //quaternion.lookRot(aimDir);

        Vector3 aimDirection = (debugTransform.position - firePoint.position).normalized;
        Vector3 forceToAdd = aimDirection * throwForce + transform.up * upwardsThrowForce;
        rb.AddForce(forceToAdd, ForceMode.VelocityChange);


    }

    void ThrowBlastPack()
    {
        

        if(!isUsingWall)
        {
            GameObject blastPack = Instantiate(blastPackPrefab, firePoint.position, transform.rotation);
            Rigidbody rb = blastPack.GetComponent<Rigidbody>();

            Vector3 aimDirection = (debugTransform.position - firePoint.position).normalized;
            Vector3 forceToAdd = aimDirection * blastPackThrowForce + transform.up * upwardsThrowForce;

            //rb.AddTorque(rb.transform.up * torque);
            rb.AddTorque(0, 1 * torque, 0, ForceMode.Force);
            rb.AddForce(forceToAdd, ForceMode.VelocityChange);

            Debug.Log(aimDirection);
        }



    }

    void CastingIceWall()
    {


        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wallRange, layermask))
        {
            Debug.Log(hit.transform.gameObject);    
            if (!iceWallPreview.activeSelf)
            {
                iceWallPreview.SetActive(true);
            }

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            if (direction) rotation.y = 1; //direction is toggle
            else rotation.y = 0;

            iceWallPreview.transform.localRotation = rotation;
            iceWallPreview.transform.position = hit.point;

            if (canCastIceWall )
            {
                Instantiate(iceWallObject, hit.point, iceWallPreview.transform.rotation);
                //canCastIceWall = false;
                isUsingWall = false;
                iceWallPreview.SetActive(false);
                currentIceWallCD = iceWallCD;
            }

        }
        else { iceWallPreview.SetActive(false); }


        if (changeDir)
        {
            direction = !direction;
            changeDir = false;
        }

    }

    void CastingBoomBot()
    {
        Vector3 spawnPos = gameObject.transform.position + transform.forward  * spawnRange;

        GameObject spawnedBB = Instantiate(boomBotPrefab, spawnPos, gameObject.transform.rotation);
        if (gameObject.CompareTag("Player1"))
        {
            spawnedBB.GetComponent<BoomBot>().GetOtherPlayer("Player2");
        }
        else if (gameObject.CompareTag("Player2"))
        {
            spawnedBB.GetComponent<BoomBot>().GetOtherPlayer("Player1");
        }

        currentBoomBotCD = boomBotCD;
    }
}
