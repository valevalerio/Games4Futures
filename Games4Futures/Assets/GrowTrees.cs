using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowTrees : MonoBehaviour
{
    public Camera GOCamera;
    // modelli standard
    public List<GameObject> Trees;
    //modelli già istanziati
    public List<GameObject> TreesInstaciated;
    public float GrowSpeed=30;
    Camera cam;

    Ray ray;
    RaycastHit hitData;

    // Start is called before the first frame update
    void Start()
    {
        cam = GOCamera;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
    
        Vector3 pos = new Vector3(200, 200, 0);

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

                

                TreesInstaciated.Add(tree);

            }
        }
        List<int> AdultTrees=new List<int>();
        int j = 0;
        foreach (var t in TreesInstaciated)
        {
            Debug.Log(t.transform.localScale.x);
            if (t.transform.localScale.x <= 1f)
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


    }
}
