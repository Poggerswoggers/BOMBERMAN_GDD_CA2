using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float currentTimer;
    public float rechargeTimer = 15f;
    public int healAmmount = 30;

    public bool hpReady;
    public GameObject healthPack;

    public AudioSource aS;
    public AudioClip healAudio;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = rechargeTimer;
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
        }
        if(currentTimer <= 0)
        {
            hpReady = true;
            healthPack.SetActive(true);
        }
        else
        {
            hpReady = false;
            healthPack.SetActive(false);
            
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() && hpReady)
        {
            Debug.Log("Player detected");

            if(collision.gameObject.GetComponentInParent<PlayerController>().playerCurrentHealth != 100)
            {
                collision.gameObject.GetComponentInParent<PlayerController>().TakeDamage(-healAmmount);
                aS.PlayOneShot(healAudio);
                currentTimer = rechargeTimer;


                
            }
            
        }
    }
   
}
