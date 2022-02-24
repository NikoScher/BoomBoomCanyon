using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerCamera;

    [SerializeField] GameObject endScreen;      // End screen canvas object

    // Collider used to detect player and audio played when in collider
    Collider winCave;
    AudioSource winMusic;

    // Timer used to calculate score at the end
    float timer = 0.0f;
    public int endScore = 0;
    public float maxPoints = 1000000;

    // Check if player has reached eng goal e.g, cave
    void OnTriggerEnter(Collider collider)
    {
        GameObject entity = collider.gameObject;
        if (entity.tag == "Player")
            Win();
    }

    // Start is called before the first frame update
    void Start()
    {
        winCave = GetComponent<BoxCollider>();
        winMusic = GetComponent<AudioSource>();
    }

    void Update() 
    {
        timer += Time.deltaTime;
    }

    void Win()
    {
        playerCamera.GetComponent<AudioSource>().Stop();    // Stop Background music
        winMusic.Play();
        
        // Calculate end score and display it
        maxPoints /= player.GetComponent<Respawn>().deaths + 1;
        maxPoints -= timer * 1000;
        maxPoints = Mathf.Max(maxPoints, 0.0f);
        endScore = (int)maxPoints;
        endScreen.SetActive(true);
    }
}
