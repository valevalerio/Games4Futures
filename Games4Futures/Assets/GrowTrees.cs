using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrowTrees : MonoBehaviour
{
    public float maxScale = 2;
    public float GrowSpeed = 30;
    public float startingCo2Value = 2;
    public float Co2value = 0;
    
    public Camera GOCamera;
    // modelli standard
    public List<GameObject> Trees;
    //modelli già istanziati
    public List<GameObject> TreesInstaciated;
  
    
    Camera cam;

    Ray ray;
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {
        cam = GOCamera;
        // IstanciatedFactories = new List<GameObject>();
        Co2_1.value = Co2_0.value = startingCo2Value;
        FactorySlider.value = Factories.Count;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        this.transform.parent.transform.Rotate(Vector3.up*Time.deltaTime*2);

        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = cam.ScreenPointToRay(mousePos);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            RaycastHit hit;
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit))
            {

                int treeIdx = Random.Range(0, Trees.Count);
                Vector3 TreePos = hit.point;
                GameObject tree = GameObject.Instantiate(Trees[treeIdx]);
                tree.transform.localScale = .1f * Vector3.one;
                tree.transform.position = TreePos;
                tree.transform.parent = this.transform.parent;
                
                

                TreesInstaciated.Add(tree);

            }
        }
        List<int> AdultTrees=new List<int>();
        int j = 0;
        foreach (var t in TreesInstaciated)
        {
            
            if (t.transform.localScale.x <= maxScale)
            {
                t.transform.localScale = t.transform.localScale + (Vector3.one * Time.deltaTime * GrowSpeed);
            }
            else {
                AdultTrees.Add(j);
            }
            j += 1;
        }
        AdultTrees.Reverse();
        foreach (var k in AdultTrees)
            TreesInstaciated.RemoveAt(k);

        balanceSystem();
    }

    public List<GameObject> Factories;
    public List<GameObject> IstanciatedFactories;
    public float Co2produced = 2f;
    public float TreeCo2Capacity = 2;
    public Slider FactorySlider;
    public Slider Co2_0;
    public float noisePerc = 0.1f;
    public Slider Co2_1;

    public Slider EnergyDemand;
    public float noiseOnDemand=0.1f;
    public float EnergyPerCo2Produced = 10f;
    // Update is called once per frame
    void balanceSystem()
    {

        EnergyDemand.value += 1*Time.deltaTime + Time.deltaTime * (noiseOnDemand * EnergyDemand.maxValue) * Mathf.Sin(Time.timeSinceLevelLoad);
        Co2_0.value += ( IstanciatedFactories.Count * Co2produced+
                                    -TreesInstaciated.Count*TreeCo2Capacity);
        //Time.deltaTime*(noisePerc*Co2_0.maxValue)*Mathf.Cos(Time.timeSinceLevelLoad) 
        Co2_1.value = Co2_0.value;
        Co2value = Co2_1.value;
        Debug.Log(EnergyDemand.value.ToString()+"Energy, there are"
            +IstanciatedFactories.Count.ToString()+"REsources,each produces"+ Co2produced * EnergyPerCo2Produced + "energy"+
            "for a total of"+ (IstanciatedFactories.Count * Co2produced * EnergyPerCo2Produced).ToString());
        if (EnergyDemand.value > IstanciatedFactories.Count * Co2produced* EnergyPerCo2Produced)
        {
            
            GameObject newFact = GameObject.Instantiate(IstanciatedFactories[0], this.transform);
            newFact.transform.position=new Vector3(Random.Range(-this.transform.lossyScale.x/2, this.transform.lossyScale.x/ 2),
                                                       0,
                                                    Random.Range(-this.transform.lossyScale.z / 2, this.transform.lossyScale.z / 2));
            IstanciatedFactories.Add(newFact);
            FactorySlider.value += 1;

        }

    }

}
