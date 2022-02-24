using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerGUI : MonoBehaviour
{
    // Must always be player game object! Not any of its children!
    [SerializeField] GameObject player;

    // Need player controller script for displaying stamina
    PlayerController pc;

    Text stamina;

    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerController>();
        stamina = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float sprintDelta = pc.currSprintTime / pc.maxSprintTime;
        int sprintPercent = (int) (sprintDelta * 100);
        
        // Display calculated stamina and Lerp between green and red to make it look swanky
        stamina.text = "Stamina: " + sprintPercent + "%";
        stamina.color = Color.Lerp(Color.red, Color.green, sprintDelta);
    }
}
