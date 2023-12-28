using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlantBehaviorSync : Bolt.EntityBehaviour<IPlantState>
{
    private GameObject spawnPoint;
    public GameObject chargerToSpawn;
    private bool hasSeed = false;
    private bool isTaken = false;

    //growing animation parameters
    private float ultimateScale;
    private float ultimatePos;
    private Vector3 scaleFactor;
    private Vector3 growSpeed;

    private void Awake()
    {
        //find spawn pos
        spawnPoint = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        //rotation angles
        transform.eulerAngles = new Vector3(Random.Range(-5, 5), Random.Range(-180, 180), Random.Range(-5, 5));

        float randomScaleUnit = Random.Range(0.15f, 0.4f)*0.03f;
        transform.localScale = new Vector3(randomScaleUnit, randomScaleUnit, randomScaleUnit);
        ultimateScale = Random.Range(0.7f, 1.25f)*0.03f;
        float scaleRandomSeed = Random.Range(0.0015f, 0.003f);
        scaleFactor = new Vector3(scaleRandomSeed, scaleRandomSeed, scaleRandomSeed);

    }

    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.PlantTransform, gameObject.transform);
        state.SetTransforms(state.SpawnPosTransform, spawnPoint.transform);
    }

    
    void Update()
    {
        if (transform.localScale.y < ultimateScale)
        {
            transform.localScale += scaleFactor;
        }

        if (!hasSeed)
        {
            BoltNetwork.Instantiate(chargerToSpawn, spawnPoint.transform.position, Quaternion.identity);
            hasSeed = true;
        }
    }
}
