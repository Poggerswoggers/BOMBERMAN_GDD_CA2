using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject westTP;
    public GameObject eastTP;

    public float timeBeforeTP = 4f;
    public bool westTPCheck = false;
    //true for west teleporter
    //false for east teleporter

    public AudioClip teleportSuccess;
    public AudioClip teleportFail;
    AudioSource tpAudio;

    // Start is called before the first frame update
    void Start()
    {
        tpAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<UpdatedPlayerController>())
        {

            Debug.Log("collision");
            if(collision.gameObject.GetComponent<UpdatedPlayerController>().teleportCD <= 0 )
            {
                Debug.Log("successful tp");
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(teleportSuccess);
                CharacterController cc = collision.GetComponent<CharacterController>();
                if (westTPCheck == true) //if this is west tp
                {
                    Debug.Log("this is west TP");
                    //collision.gameObject.transform.position = eastTP.transform.position;
                    cc.enabled = false;

                    collision.gameObject.transform.position = new Vector3(eastTP.transform.position.x,collision.gameObject.transform.position.y,eastTP.transform.position.z - 2);
                    collision.gameObject.GetComponentInParent<UpdatedPlayerController>().teleportCD = timeBeforeTP;
                    //collision.gameObject.transform.RotateAround(transform.position, transform.up, 180f); //rotate 180degrees
                    cc.enabled = true;

                }
                else if(westTPCheck == false) // if this is east tp
                {

                    Debug.Log("this is east TP");
                    cc.enabled = false;
                    //collision.gameObject.transform.position = westTP.transform.position;
                    collision.gameObject.transform.position = new Vector3(westTP.transform.position.x, collision.gameObject.transform.position.y, westTP.transform.position.z + 2);
                    collision.gameObject.GetComponentInParent<UpdatedPlayerController>().teleportCD = timeBeforeTP;
                    //collision.gameObject.transform.RotateAround(transform.position, transform.up, 180f); //rotate 180degrees
                    cc.enabled = true;
                }
            }
            else
            {
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(teleportFail);
            }
        }

        else if (collision.gameObject.GetComponent<Rigidbody>())
        {
            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(teleportSuccess);
            tpAudio.PlayOneShot(teleportSuccess);

            if (westTPCheck) //if this is west tp
            {


                collision.gameObject.transform.position = new Vector3(eastTP.transform.position.x, collision.gameObject.transform.position.y, eastTP.transform.position.z - 2);

                //collision.gameObject.transform.RotateAround(transform.position, transform.up, 180f); //rotate 180degrees

            }
            else if (!westTPCheck) // if this is east tp
            {
                collision.gameObject.transform.position = new Vector3(westTP.transform.position.x, collision.gameObject.transform.position.y, westTP.transform.position.z + 2);

                //collision.gameObject.transform.RotateAround(transform.position, transform.up, 180f); //rotate 180degrees
            }
        }




    }


}
