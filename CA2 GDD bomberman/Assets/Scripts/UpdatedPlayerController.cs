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

    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, cam.nearClipPlane);
        Ray ray = cam.ViewportPointToRay(viewportCenter);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tpsAimCam.AimCollisionFilter))
        {
            debugTransform.position = raycastHit.point;
        }



        if (Input.GetButton("Fire1"))
        {
            if (throwForce >= maxThrowForce) return;
            throwForce += throwWindUpRate * Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1"))
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

        //quaternion.lookRot(aimDir);

        Vector3 aimDirection = (debugTransform.position - firePoint.position).normalized;
        rb.AddForce(aimDirection * throwForce, ForceMode.VelocityChange);

        Debug.Log(aimDirection);

    }
}
