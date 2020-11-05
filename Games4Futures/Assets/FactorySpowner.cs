using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactorySpowner : MonoBehaviour
{
    public List<GameObject> Factories;
    public List<GameObject> IstanciatedFactories;
    public Slider NonGEnergy;
    float noiseNonG;
    public Slider Co2Perc;
    public Slider EnergyDemand;
    // Start is called before the first frame update
    void Start()
    {
        IstanciatedFactories = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        noiseNonG += Time.deltaTime;
        NonGEnergy.value += 1+Mathf.Cos(noiseNonG);
        // Co2Perc = Mathf.Cos(noiseNonG+3)
    }
}
