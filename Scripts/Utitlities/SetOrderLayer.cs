using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrderLayer : MonoBehaviour
{

    public MeshRenderer mesh;
    public int order;
    // Start is called before the first frame update
    void Start()
    {
        mesh.sortingOrder = order;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
