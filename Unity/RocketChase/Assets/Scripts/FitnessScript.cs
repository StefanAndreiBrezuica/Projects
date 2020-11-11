using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FitnessScript : MonoBehaviour
{

    public static List<float> fitnessValue = new List<float>();
    Text fitness;
    void Start()
    {
        fitness = GetComponent<Text>();
        
    }

    
    void Update()
    {
        fitness.text = "Fitness: " + fitnessValue.Sum()/50;
    }
}
