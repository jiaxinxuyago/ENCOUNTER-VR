using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallingSphereBehaviorSync : Bolt.EntityBehaviour<ICallingSphereState>
{
    private Vector3 scaleFactor;
    private float scaleSpeed;
    private Material myMaterial;
    private Color emissionColor;
    private float emissionIntensity = 1;

    void Start()
    {
        scaleSpeed = 0.1f;
        myMaterial = GetComponent<MeshRenderer>().material;
        myMaterial.EnableKeyword("_EMISSION");
        emissionColor = myMaterial.GetColor("_EmissionColor");
    }

    public override void Attached()
    {
        state.SetTransforms(state.CallingSphereTransform, gameObject.transform);
    }

        // Update is called once per frame
    void Update()
    {
        SphereGrowth();
    }

    void SphereGrowth()
    {
        //sphere expansion
        scaleFactor = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
        transform.localScale += scaleFactor;
        scaleSpeed += 0.005f;
        emissionIntensity -= 0.025f;

        float factor = Mathf.Pow(2, emissionIntensity);
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(emissionColor.r * factor, emissionColor.g * factor, emissionColor.b * factor));

        //sphere death
        if (transform.localScale.y > 160.0f)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }
}
