using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    Text introText;

    float timer = 0.0f;
    public float timeOut = 5.0f;    // Time that start screen stays on screen for

    // Start is called before the first frame update
    void Start()
    {
        introText = GetComponent<Text>();
    }

    void Update()
    {
        // Destroy start screen object after time reaches timeOut
        timer += Time.deltaTime;
        if (timer > timeOut)
            Destroy(gameObject);
    }
}
