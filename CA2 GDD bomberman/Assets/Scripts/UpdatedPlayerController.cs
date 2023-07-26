using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedPlayerController : MonoBehaviour
{



    [Header("Grenade")]
    public GameObject bombPrefab;
    public float throwForce = 10f;
    public float throwWindUpRate = 10f;
    public float maxThrowForce = 40f;
    public float throwCooldown = 2.5f;
    float currentThrowCooldown;
    bool onCooldown = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        GameObject bomb = Instantiate(bombPrefab, transform.position + new Vector3( 0, 2,0), transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
