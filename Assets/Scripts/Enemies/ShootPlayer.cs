using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    [SerializeField] GameObject proj;    // Game object to be fired (in this case - cannonball)
    [SerializeField] Transform projPose; // Pose to fire object from, orietation matters!

    [SerializeField] GameObject target;  // Game object for cannon to target (in this case - player)
    [SerializeField] Material mat;       // Material of targeting laser
    LineRenderer lr;                     // Line renderer for targeting laser

    AudioSource bang;       // Sound efffect to be played when cannon fires
    ParticleSystem ps;      // Particles produced when cannon fires

    Vector3 lockOri;        // Vector stroing last know look vector where cannon has seen player
    Vector3 tempOri;        // Vector storing current look vector
    bool lockedOn = false;
    float maxWT = 5.0f;     // Time that cannon will wait until a new wonder pose is made
    float currWT = 0.0f;    // Timer for wonder pose

    public float range = 30.0f;     // Cannon range
    public float targSpeed = 0.01f; // Cannon targeting speed

    public float maxRT = 5.0f;      // Cannon reload time
    float currRT = 0.0f;            // Timer for reloading

    // Start is called before the first frame update
    void Start()
    {
        bang = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
        lockOri = transform.localEulerAngles;
        tempOri = lockOri;

        // Targeting laser should be thin and red
        lr = GetComponent<LineRenderer>();
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth  = 0.1f;
        lr.material = mat;
    }

    // Update is called once per frame 
    void Update()
    {
        // Reset targeting laser
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
        
        // Check player is in some radius around cannon
        // Make sure to use ignore raycast mask so that raycast is not stopped by particles, GUI elements etc.
        RaycastHit hit;
        Vector3 targetVec = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, targetVec, out hit, Mathf.Infinity, ~LayerMask.GetMask("Ignore Raycast"))) {
            GameObject entity = hit.collider.gameObject;
            if (entity.tag == "Player" && hit.distance <= range) {
                lockOri = transform.localEulerAngles;   // Update last know look vector that points to player
                if (lockedOn)                           // If locked on, then always point at player
                    transform.rotation = Quaternion.LookRotation(targetVec);
                else                                    // If not locked on then slowly rotate to player
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetVec), targSpeed);
            }
            // Else, player not found so goto wander behaviour
            else {
                if (currWT >= maxWT) {
                    // Create new random wander orientation
                    tempOri = lockOri + new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0);
                    currWT = 0.0f;
                }
                // Rotate to wander orientation
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(tempOri.x, tempOri.y, tempOri.z), 0.0005f);
            }
        }

        // Check player is in front of cannon barrel
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, ~LayerMask.GetMask("Ignore Raycast"))) {
            GameObject entity = hit.collider.gameObject;
            if (entity.tag == "Player") {
                // If in front of cannon barrel then lock on and display targeting laser
                lockedOn = true; 
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, target.transform.position + (0.5f * Vector3.down));
                // Check if cannon is reloaded
                if (currRT >= maxRT) {
                    bang.Play();    // Play cannon fire audio
                    Instantiate(proj, projPose.position, projPose.rotation);    // Create cannonball
                    currRT = 0.0f;  // Reset reload timer
                    ps.Play();      // Make smoke particles
                }
            }
            else {
                // If player not in front of barrel then no lock on
                lockedOn = false;
            }
        }

        // Use delta time to add to reload timer
        if (currRT < maxRT)
            currRT += Time.deltaTime;
        // Use delta time to add to wander timer
        if (currWT < maxWT)
            currWT += Time.deltaTime;
    }

}