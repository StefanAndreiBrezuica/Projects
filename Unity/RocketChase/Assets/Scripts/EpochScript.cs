using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpochScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static int epochValue=0;
    Text epoch;
    void Start()
    {
        epoch = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        epoch.text = "Epoch: " + epochValue;
    }
}
