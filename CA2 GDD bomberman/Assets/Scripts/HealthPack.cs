using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float currentTimer;
    public float rechargeTimer = 20f;
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
        if (collision.gameObject.GetComponent<PlayerController>() && hpReady)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(-healAmmount);
            aS.PlayOneShot(healAudio);
        }
    }
   
}
