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
    float currentThrowCooldown;
    bool onCooldown = false;

    [SerializeField] private Transform debugTransform;

    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = cam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tpsAimCam.AimCollisionFilter))
        {
            debugTransform.position = raycastHit.point;
        }



        if (Input.GetMouseButton(0))
        {
            if (throwForce >= maxThrowForce) return;
            throwForce += throwWindUpRate * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!onCooldown)
            {
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
    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, firePoint.position, transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        Debug.Log(tpsAimCam.AimTargetReticle);
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Cast a ray from the camera through the center of the screen
        Ray ray = cam.ScreenPointToRay(screenCenter);

        // Perform the raycast
        Debug.Log(ray.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            Debug.Log(hit);
            // Handle the raycast hit, e.g., interact with the hit object
        }


        rb.AddForce(firePoint.forward * throwForce, ForceMode.VelocityChange);

        CalculateAimAt();

    }

    public void CalculateAimAt()
    {

    }

}
