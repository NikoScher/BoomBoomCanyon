using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    //Fields
    [SerializeField] Transform forceOrigin;     // Pose where force is emmited from when launching objects with Fire1
    [SerializeField] Transform holdTransform;   // Pose object is held in when holding object with Fire2

    List<Rigidbody> entityList = new List<Rigidbody>(); // List of all launchable objects within launch range

    Rigidbody heldEntity = null;

    public float launchFor = 40;    // Launch force

    void OnTriggerEnter(Collider collider)
    {
        Rigidbody entity;
        if(collider.GetComponent<Rigidbody>() != null) {                // Make sure object has rigid body
            entity = collider.GetComponent<Rigidbody>();
            Rigidbody checkEntity = entityList.Find(x=> x == entity);   // Make sure obecjt isn't already in list
            if (checkEntity == null)
                entityList.Add(entity);                                 // Add it if it isn't in list
        }
    }

    // If out of range, remove object from list
    void OnTriggerExit(Collider collider)
    {
        Rigidbody entity = collider.GetComponent<Rigidbody>();
        entityList.Remove(entity);
    }

    // Update is called once per frame
    void Update()
    {
        // Clear list of nulls, must be done as some objects may have been Destroy()'ed
        for(int i = 0; i < entityList.Count; i++) {
            if (entityList[i] == null)
                entityList.RemoveAt(i);
        }

        // If player holding object, ensure object is in the hold position
        if(heldEntity != null) {
            heldEntity.gameObject.tag = "Projectile";
            Vector3 smoothPose = Vector3.Lerp(heldEntity.position, holdTransform.position, 0.15f);  // Smooth transition, looks more natural
            heldEntity.MovePosition(smoothPose);
        }

        // Fire1 (Right Click): Slightly pushs objects away from player, if player holding object then launches it instead
        if (Input.GetButtonDown("Fire1")) {
            // Launch held object
            if(heldEntity != null) {
                heldEntity.isKinematic = false;     // Must not be kinematic so object is affected by collisions once launched
                Vector3 forceVec = holdTransform.transform.forward.normalized * launchFor;
                heldEntity.AddForce(forceVec, ForceMode.VelocityChange);
                heldEntity = null;
                return;
            }
            // Slightly push all nearby objects
            foreach (Rigidbody entity in entityList) {
                Vector3 entityPos = entity.GetComponent<Transform>().position;
                Vector3 forceVec = entityPos - forceOrigin.position;            // Calculate direction that object is pushed
                float scalingForce = 100 * Mathf.Exp(3 / forceVec.magnitude);   // More push force the closer object is to player
                forceVec.Normalize();                                           // Only care about direction so normalise vector
                entity.AddForce(forceVec * scalingForce);                       // Scale vector and push object
            }
        }

        // Fire2 (Left Click): Picks up object is not holding any, or drops currently held object
        if (Input.GetButtonDown("Fire2")) {
            // Drop object
            if(heldEntity != null) {
                heldEntity.isKinematic = false; // Must not be kinematic so object is affected by collisions once dropped
                heldEntity = null;
                return;
            }
            // If there are nearby objects
            if (entityList.Count != 0) {
                float minDist = float.MaxValue;
                // Get the object closest to player
                foreach (Rigidbody entity in entityList) {
                    Vector3 entityPos = entity.GetComponent<Transform>().position;
                    Vector3 dispVec = entityPos - forceOrigin.position;
                    if (dispVec.magnitude < minDist) {
                        // Update closest object
                        heldEntity = entity;
                        minDist = dispVec.magnitude;
                    }
                }
                heldEntity.isKinematic = true;  // Must be kinematic so object is unaffected by collisions when held
            }
        }
    }

}
