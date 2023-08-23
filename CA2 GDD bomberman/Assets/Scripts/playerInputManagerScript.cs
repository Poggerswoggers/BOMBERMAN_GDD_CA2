using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManagerScript : MonoBehaviour
{

    private PlayerInputManager playerInputManager;

    public Transform warmupSpawn;

    public List<string> camLayer;
    public int stringInt;
    public LayerMask player1Mask;
    public LayerMask player2Mask;

    // Start is called before the first frame update
    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }


    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Ran");
        // Get the GameObject representing the player
        Transform playerObject = playerInput.gameObject.transform.parent;
        Debug.Log(playerObject);

        //
        for (int i= 0; i<playerObject.childCount; i++)
        {
            Transform childTransform = playerObject.transform.GetChild(i);
            if (childTransform.tag == "Camera")
            {
                int layerInt = LayerMask.NameToLayer(camLayer[stringInt]);
                
       
                childTransform.gameObject.layer = layerInt;
            }
        }

        Camera playerCam = playerObject.GetComponentInChildren<Camera>();

        if (stringInt == 0)
        {
            playerObject.GetComponentInChildren<PlayerController>().currentPlayer = PlayerController.players.P1;
            playerCam.cullingMask = ~player2Mask;
            
        }
        else
        {
            playerObject.GetComponentInChildren<PlayerController>().currentPlayer = PlayerController.players.P2;
            playerCam.cullingMask = ~player1Mask;
        }
    
        stringInt++;

        playerObject.position = warmupSpawn.position + new Vector3(0,2,0);


    }

}
