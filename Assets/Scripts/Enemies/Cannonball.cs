using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    AudioSource cannonHit;
    MeshRenderer mesh;
    Collider collider;
    Rigidbody rigidBody;

    // Used to make sure that hit script is only ran once as cannonball may...
    // collide with multiple objects before its destroyed
    bool hit = false;

    void Start()
    {
        cannonHit = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();

        // Launch cannonball forward, launch orientation is handeled by ShootPlayer script
        Vector3 force = transform.forward.normalized * 50;
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hit) {
            hit = true;
            // Disable mesh and collider so other components like audio can run
            // As cannonball has collided, turn off mesh so player can't see it
            mesh.enabled = false;
            collider.enabled = false;

            // If cannonball collides with player then play audio and respawn player
            GameObject entity = collision.gameObject;
            if (entity.tag == "Player") {
                AudioSource playerHit = entity.GetComponent<AudioSource>();
                playerHit.Play();
                entity.GetComponent<Respawn>().doRespawn = true;
            }
            else {
                // Dud sound effect if cannonball hits terrain etc.
                cannonHit.Play();
            }

            Destroy(gameObject, 2.0f);  // Wait 2 seconds so audio can play
        }
    }
}
