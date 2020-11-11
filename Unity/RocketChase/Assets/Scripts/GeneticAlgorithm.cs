using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GeneticAlgorithm
{

    public void Evolve(List<NeuralNetwork> brain)
    {
        
        int totalNumberOfBrains = brain.Count;
        Selection(brain);
        Crossover(brain,totalNumberOfBrains);
    }

    //Funcția de selecție
    private void Selection(List<NeuralNetwork> brain)
    {
        
        List<NeuralNetwork> brainCopy;
        brainCopy = new List<NeuralNetwork>(); //lista auxiliara de retele
        List<float> selectionChances; // lista de alocare a fitness-ului
        
        selectionChances = new List<float>();
        float fitnessSum = 0;

        for(int i=0;i<brain.Count;i++)
        {
            selectionChances.Add(brain[i].GetFitness()); //extragem fitness-ul fiecărui individ

            fitnessSum += selectionChances.Sum(); 
        }
        for(int i = 0; i < brain.Count; i++)
        {
            selectionChances[i] /= fitnessSum; //Calculam sansa de selectie
        }
        for(int i = 0; i < brain.Count; i++)
        {
            if (selectionChances[i] < UnityEngine.Random.Range(0f, 1f))
            {
                  brainCopy.Add(brain[i]); //adaugam indivizi selecati in lista auxiliara
            }
            
            
        }
        brain = brainCopy;
    }

    //Funcția de încrucișare
    private void Crossover(List<NeuralNetwork> brain,int count)
    {
        while (brain.Count < count)
        {
            //Incrucisam aleator genele si le supunem la mutatie
            brain.Add(brain[UnityEngine.Random.Range(0, brain.Count)].Cross(brain[UnityEngine.Random.Range(0, brain.Count)]));
        }
    }
}
