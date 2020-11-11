using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject RocketPrefab;
    public GameObject MotherShip;

    private GeneticAlgorithm evolution=new GeneticAlgorithm();
    private bool isTraning = false;
    private int populationSize = 50;
    private int generationNumber = 0;
    private int[] layers = new int[] { 1, 10, 10, 1 }; //1 input si 1 output
    private List<NeuralNetwork> brain;
    private bool leftMouseDown = false;
    private List<Controller> RocketList = null;


    void Timer()
    {
        isTraning = false;
    }


	void Update ()
    {
        if (isTraning == false)
        {
            if (generationNumber == 0)
            {
                InitRocketNeuralNetworks();
            }
            else
            {
                brain.Sort();
                for (int i = 0; i < populationSize / 2; i++)
                {
                    brain[i] = new NeuralNetwork(brain[i+(populationSize / 2)]);
                    brain[i].Mutate();

                    brain[i + (populationSize / 2)] = new NeuralNetwork(brain[i + (populationSize / 2)]); 
                }

                for (int i = 0; i < populationSize; i++)
                {
                    brain[i].SetFitness(0f);
                }
            }

            generationNumber++;
            EpochScript.epochValue = generationNumber;
            isTraning = true;
            Invoke("Timer",15f);
            evolution.Evolve(brain);
            CreateRocketBodies();
        }


        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        if(leftMouseDown == true)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MotherShip.transform.position = mousePosition;
        }
    }


    private void CreateRocketBodies()
    {
        if (RocketList != null)
        {
            for (int i = 0; i < RocketList.Count; i++)
            {
                GameObject.Destroy(RocketList[i].gameObject);
            }

        }

        RocketList = new List<Controller>();

        for (int i = 0; i < populationSize; i++)
        {
            Controller control = ((GameObject)Instantiate(RocketPrefab, new Vector3(UnityEngine.Random.Range(-10f,10f), UnityEngine.Random.Range(-10f, 10f), 0),RocketPrefab.transform.rotation)).GetComponent<Controller>();
            control.Init(brain[i],MotherShip.transform);
            RocketList.Add(control);
        }
        
    }

    void InitRocketNeuralNetworks()
    {
        
        if (populationSize % 2 != 0)
        {
            populationSize = 20; 
        }

        brain = new List<NeuralNetwork>();
        

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            brain.Add(net);
        }
    }
}
