
using System.Collections.Generic;
using System;


public class NeuralNetwork : IComparable<NeuralNetwork>
{

    private int[] layers; //straturile
    private float[][] neurons; //Matricea de neuroni
    private float[][][] weights; //Matricea de greutati
    private float fitness; //Fitnessul retelei


    /// <summary>
    /// Initilizeaza neuroni din retea
    /// </summary>
    /// <param name="layers">straturile retelei neuronale</param>
    public NeuralNetwork(int[] layers)
    {
        //deep copy of layers of this network 
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }


        //genereaza matricele
        InitNeurons();
        InitWeights();
    }

    
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }


    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    /// <summary>
    /// Creaza matricea de neuroni
    /// </summary>
    private void InitNeurons()
    {
        //Initilizarea neuronilor
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++) //trece prin toate straturile
        {
            neuronsList.Add(new float[layers[i]]); //Adauga un strat la retea
        }

        neurons = neuronsList.ToArray(); //converteste lista intr-un array
    }


    /// <summary>
    /// Creeaza matricea de ponderi.
    /// </summary>
    private void InitWeights()
    {

        List<float[][]> weightsList = new List<float[][]>(); //Lista de ponderile care va fi convertita intr-un array tridimensional

        //trece prin toti neuroni care au ponderi
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); 

            int neuronsInPreviousLayer = layers[i - 1]; 

            //trece prin toti neuroni din strat
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //ponderile neuronilor

                //trece prin toti neuroni din stratul anterior si seteaza ponderile aleator intre -0.5 si 0.5 
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
                }

                layerWeightsList.Add(neuronWeights); 
            }

            weightsList.Add(layerWeightsList.ToArray()); 
        }

        weights = weightsList.ToArray(); 
    }


    /// <summary>
    /// Feed forward la reteaua neuronala
    /// </summary>
    /// <param name="inputs">Input-urile pe care le primeste reteaua</param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        //adauga inputurile
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //trece prin toti neuroni si calculeaza valorile prin feedforward
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //Suma tuturor neuronilor înmultite cu greutățile din stratul precedent                
                }

                neurons[i][j] = (float)Math.Tanh(value); //Funcția de activare
            }
        }

        return neurons[neurons.Length-1]; //returnează stratul de output
    }


    /// <summary>
    /// Supune greutatile din reteaua neuronala la mutatii
    /// </summary>
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //mutatie la valoarea neuronului
                    float randomNumber = UnityEngine.Random.Range(0f,100f);

                    if (randomNumber <= 2f)
                    { 
                      //schimba semnul gretutatii
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    {
                      //alege aleator o noua greutate
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { 
                      //creste cu 0 sau 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { 
                      //scade cu 0 sau 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void AddFitness(float fit)
    {
        fitness += fit;
        FitnessScript.fitnessValue.Add(fitness);
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public float GetFitness()
    {
        return fitness;
    }

    public NeuralNetwork Cross(NeuralNetwork nn)
    {
        NeuralNetwork newNN = new NeuralNetwork(layers);
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if (UnityEngine.Random.Range(0, 100) < 50)
                        newNN.weights[i][j][k] = nn.weights[i][j][k];
                    else
                        newNN.weights[i][j][k] = weights[i][j][k];
                }
            }
        }
        newNN.Mutate();
        return newNN;
    }
    /// <summary>
    /// Compara doua retele neuronale pentru sortarea dupa fitness
    /// </summary>
    /// <param name="other">Network to be compared to</param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) 
            return 1;
        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }



}
