using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManagerScript : MonoBehaviour
{

    private PlayerInputManager playerInputManager;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    public List<string> camLayer;
    public int stringInt;
    public LayerMask player1Mask;
    public LayerMask player2Mask;

    CharacterController cc;

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

        cc = playerObject.gameObject.GetComponentInChildren<CharacterController>();
        cc.enabled = false;

        if (stringInt == 0)
        {
            playerObject.GetComponentInChildren<PlayerController>().currentPlayer = PlayerController.players.P1;          
            playerCam.cullingMask = ~player2Mask;
            playerObject.position = spawnPoint1.position + new Vector3(0, 2, 0);

        }
        else
        {
            playerObject.GetComponentInChildren<PlayerController>().currentPlayer = PlayerController.players.P2;
            playerCam.cullingMask = ~player1Mask;
            playerObject.position = spawnPoint2.position + new Vector3(0, 2, 0);
        }
        cc.enabled = true;
        stringInt++;

    }

}
