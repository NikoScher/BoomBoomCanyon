using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    CharacterController playerController;

    // Respawn pose
    public Transform respawn;
    public bool doRespawn = false;

    // Number of player deaths
    public int deaths = 0;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    // Keep in mind the character controller component needs to be disabled as it has its...
    // own internal variables for velocity and position that mess with the transform component of an object
    void Update()
    {
        if (doRespawn) {
            doRespawn = false;
            
            playerController.enabled = false;
            transform.SetPositionAndRotation(respawn.position, respawn.rotation);
            playerController.enabled = true;    // Make sure to re-enable it!

            // Can add more code here if you want to do more on respawn
            deaths++;
        }
    }
}
