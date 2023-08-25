using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceWall : MonoBehaviour
{
    public float health;
    public float duration;
    public float raiseSpeed;
    public float destroyDelay;
    public float destroyPushForce;
    public float destroyRotationForce;

    private SkinnedMeshRenderer rend;
    private MeshCollider col;
    private float blendAmount = 0; //current wall height
    private bool isRaised = false;


    public AudioClip breakSound;
    public AudioClip buildSound;
    AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        
        rend = GetComponent<SkinnedMeshRenderer>();
        col = GetComponent<MeshCollider>();
        audiosource = GetComponent<AudioSource>();

        var iceWalls = GetComponentsInChildren<IceWall>();
        foreach(IceWall wall in iceWalls)
        {
            wall.transform.SetParent(null);
        }
        audiosource.PlayOneShot(buildSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRaised)
        {
            blendAmount += raiseSpeed * Time.deltaTime; //increase wall height
            rend.SetBlendShapeWeight(0, blendAmount);
            Mesh bakeMesh = new Mesh();
            rend.BakeMesh(bakeMesh);
            col.sharedMesh = bakeMesh;

            if (blendAmount >= 100) isRaised = true; //stop loop once wall at max height

        }

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

            Component[] fractures = GetComponentsInChildren(typeof(Rigidbody), true); //get all the fragment game objects
            foreach (Rigidbody child in fractures)
            {
                child.transform.SetParent(null);
                child.gameObject.SetActive(true);
                Destroy(child.gameObject, destroyDelay);

                var forceDir = child.position - transform.position;
                if (child != transform)
                {
                    Vector3 randomTorque; //calculate fragment breakage animation
                    randomTorque.x = Random.Range(-destroyRotationForce, destroyRotationForce);
                    randomTorque.y = Random.Range(-destroyRotationForce, destroyRotationForce);
                    randomTorque.z = Random.Range(-destroyRotationForce, destroyRotationForce);

                    child.AddTorque(randomTorque);
                    child.AddForce(forceDir.normalized * destroyPushForce, ForceMode.VelocityChange);

                }
                if (child == fractures.Last()) Destroy(gameObject); //clear the icewall object once destroyed

            }


        }

        if (duration <= 0) health = 0; //plays fracture animation once time out 
        else duration -=Time.deltaTime; //duration countdown 

    }
}
