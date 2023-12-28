using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenInGame : MonoBehaviour
{

    //Find Mesh Renderer from game object
    private MeshRenderer meshToTurnOff;

    // Start is called before the first frame update
    void Start()
    {
        meshToTurnOff = GetComponent<MeshRenderer>();
        meshToTurnOff.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
