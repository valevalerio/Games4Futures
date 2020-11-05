using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactorySpowner : MonoBehaviour
{
    public List<GameObject> Factories;
    private List<GameObject> IstanciatedFactories;
    public float Co2IncreasingRate = 0.2f;
    public float TreeCo2Capacity = 2;
    public Slider NonGEnergy;
    public Slider Co2Perc;
    public Slider EnergyDemand;
    private float noiseNonG;
    // Start is called before the first frame update
    void Start()
    {
        IstanciatedFactories = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        noiseNonG += Time.deltaTime;
        EnergyDemand.value += 1+Mathf.Cos(noiseNonG);
        Co2Perc.value += Mathf.Cos(noiseNonG + 3) + IstanciatedFactories.Count*Co2IncreasingRate;
        
    }
}
