using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidManager boidManager;

    public Vector3 vel = new Vector3();

    // Update is called once per frame
    void Update()
    {
        // Check if boid is close to target, if so then switch target
        Vector3 delta = boidManager.currTarget.position - transform.position;
        if(delta.magnitude < boidManager.switchDist) {
            boidManager.switchTarget = true;
        }

        int neighbours = 0;
        Vector3 avgPos = new Vector3(); // Rule 1: Cohesion
        Vector3 avgVel = new Vector3(); // Rule 2: Alignment
        Vector3 sepFor = new Vector3(); // Rule 3: Separation
        Vector3 goalVel = new Vector3();    // Extra rule so boids tend to some goal

        // Loop through every boid
        foreach (GameObject boid in boidManager.boidList) {
            if (boid != null) {                 // Check if they exist as some may be destroyed by projectiles
                if (boid != this.gameObject) {  // Ensure current boid doesn't compare itself to itself
                    delta = boid.transform.position - transform.position;   // Get the displacement of the current boid
                    if (delta.magnitude < boidManager.nbRad) {              // Check if current boid is within neibourghood
                        neighbours++;
                        avgPos += boid.transform.position;                  // Add up position, used for averge position later
                        avgVel += boid.GetComponent<Boid>().vel;            // Add up velocity, used for averge velocity later
                        if (delta.magnitude < boidManager.sepRad)           // Check if current boid is within separation radius
                            sepFor -= delta;
                    }
                }
            }
        }

        // Check if boid has any neighbours
        if (neighbours > 0) {
            avgPos = (avgPos / neighbours) - transform.position;    // Calculate average position
            avgVel = (avgVel / neighbours) - transform.position;    // Calculate average velocity
            goalVel = boidManager.currTarget.position - transform.position;     // Calculate vector point to goal
            vel += 0.05f*avgPos + 0.01f*avgVel + 0.5f*sepFor + 0.2f*goalVel;    // Weight all behaviours to give overall velocity
        }

        // Check if boid is over max velocity
        if (vel.magnitude > boidManager.maxVel) {
            vel.Normalize();            // Normalise to keep direction
            vel *= boidManager.maxVel;  // Scale back up to max velocity
        }

        // Move and rotate so boid is always pointing in direction it's heading
        transform.position += vel * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), 0.01f);
    }
}
