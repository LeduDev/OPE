using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    private float currentTime = 0f;
    private float startingTime = 5f;

    [SerializeField]
    private Text timerTxt;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        timerTxt.text = currentTime.ToString("00");
        currentTime -= 1 * Time.deltaTime;
    
        if (currentTime <=0)
        {
            currentTime = 0;
        }
    }
}
