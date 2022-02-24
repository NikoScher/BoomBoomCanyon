using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public GameObject endManager;

    Text score;

    // When canvas is enabled (after reaching end cave), show score
    void OnEnable()
    {
        score = GetComponent<Text>();
        score.text = "Score: " + endManager.GetComponent<EndManager>().endScore;
    }
}
