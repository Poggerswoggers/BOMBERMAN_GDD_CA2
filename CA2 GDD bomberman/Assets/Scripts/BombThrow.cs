using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public float explosionDelay = 3f;
    float explosionCountdown;
    bool startCD;
    bool hasExploded = true;

    // Start is called before the first frame update
    void Start()
    {
        explosionCountdown = explosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(startCD && explosionCountdown > 0 && !hasExploded)
        {
            Explode();   
        }
    
      
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            //start countdown for explosion only upon hitting ground or wall (tentative)
            startCD = true;
        }
    }

    public void Explode()
    {

    }
}
