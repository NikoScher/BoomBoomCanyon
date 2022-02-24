using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Time taken to destroy entity after Destroy() is called
    [SerializeField] float destroyWaitTime = 2.0f;

    // Sound effect made when entity is destoryed
    AudioSource destroy;

    void OnTriggerEnter(Collider collider)
    {
        // Check if object has been hit with "Projectile" e.g, cannonball or rock
        // Transform to under the level so object is hidden from player while audio plays
        GameObject entity = collider.gameObject;
        if(entity.tag == "Projectile") {
            destroy.Play();
            transform.position = new Vector3(0, -10, 0);
            Destroy(gameObject, destroyWaitTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        destroy = GetComponent<AudioSource>();
    }
}
