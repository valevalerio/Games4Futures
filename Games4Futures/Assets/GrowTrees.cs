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
    private float Radius=50f;
    public float FallingSpeed = 3f;
    public Camera GOCamera;
    // modelli standard
    public List<GameObject> Trees;
    //modelli già istanziati
    public List<GameObject> TreesInstaciated;

    public AudioClip OST;
    public AudioClip FallingFactory;

    public AudioClip FallingTreeSound;
    Camera cam;

    
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {
        cam = GOCamera;
        // IstanciatedFactories = new List<GameObject>();
        Co2_1.value = Co2_0.value = startingCo2Value;
        FactorySlider.value = IstanciatedFactories.Count;
        AudioSource ost = this.transform.parent.gameObject.AddComponent<AudioSource>();
        ost.loop = true;
        ost.PlayOneShot(OST);

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
                tree.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                tree.transform.parent = this.transform.parent;
                AudioSource treeAudio = tree.AddComponent<AudioSource>();
                
                treeAudio.PlayOneShot(FallingTreeSound);
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
        j = 0;
        List<int> stableFactories = new List<int>();
        foreach (var f in FallingFactories)
        {
            RaycastHit hit;

            if (Physics.Raycast(f.transform.position, Vector3.down, out hit)){

                if (f.transform.position.y > hit.point.y-.5f)
                {
                    f.transform.Translate(Vector3.down * Time.deltaTime * FallingSpeed);
                }
                else
                {
                    f.GetComponent<AudioSource>().PlayOneShot(FallingFactory);
                    stableFactories.Add(j);
                }
            }
            j += 1;
        }
        stableFactories.Reverse();
        Debug.Log(stableFactories);
        foreach(var f in stableFactories)
        {
            FallingFactories.RemoveAt(f);
        }
}

    public List<GameObject> Factories;
    public List<GameObject> IstanciatedFactories;
    public List<GameObject> FallingFactories;
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
            "for a total of"+ ((IstanciatedFactories.Count + FallingFactories.Count) * Co2produced * EnergyPerCo2Produced).ToString());
        if (EnergyDemand.value > (IstanciatedFactories.Count+ FallingFactories.Count) * Co2produced* EnergyPerCo2Produced)
        {
            
            GameObject newFact = GameObject.Instantiate(Factories[0]);
            float r2, x2, z2,theta2;
            r2 = Radius * Random.Range(0.2f, .999f);
            theta2 = 2 * Mathf.PI * Random.Range(0.2f, .999f);
            x2 = r2 * Mathf.Cos(theta2);
            z2 = r2 * Mathf.Sin(theta2);
            Vector3 xyFabricPosition = new Vector3(x2, 10, z2);
            newFact.transform.position = xyFabricPosition;
            RaycastHit hit;
            
            if (Physics.Raycast(xyFabricPosition,Vector3.down, out hit)){
                newFact.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
            newFact.transform.SetParent(this.transform.parent);
            newFact.AddComponent<AudioSource>().PlayOneShot(FallingFactory);
            

            FallingFactories.Add(newFact);
            //IstanciatedFactories.Add(newFact);
            FactorySlider.value += 1;
            Debug.Log("newFactory");

        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, Radius*Vector3.one);
    }

}
