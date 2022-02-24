using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] Transform target;  // Target pose for boids
    public Transform currTarget;
    public bool switchTarget = false;   // Used so boids can switch between target pose and manager pose
    public float switchDist = 3.0f;     // At least 1 boid must get within this distance for target to switch

    public float nbRad = 20.0f;         // Neighbourhood radius
    public float sepRad = 2.0f;         // Separation radius

    public float maxVel = 5.0f;         // Max velocity boid can travel

    public float boundRad = 10.0f;      // Boids must spawn within this distance of the X-Z plane of boid manager
    public int numBoids = 15;           // Number of boids that manager spawns
    
    public GameObject[] boidList;
    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        currTarget = target;

        // Instatiate all the boids and ensure they have a manager
        boidList = new GameObject[numBoids];
        for (int i = 0; i < numBoids; i++) {
            Vector3 randomPos = new Vector3(Random.Range(-boundRad, boundRad), Random.Range(-1.0f, 1.0f), Random.Range(-boundRad, boundRad));
            Vector3 startPos = transform.position + randomPos;

            boidList[i] = (GameObject)Instantiate(prefab, startPos, Random.rotation);
            boidList[i].GetComponent<Boid>().boidManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Any boid can toggle switchTarget
        if(switchTarget) {
            if (currTarget == target)
                currTarget = transform;
            else
                currTarget = target;
            switchTarget = false;
        }
    }

}