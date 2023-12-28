using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    Material mat;
    float counter = 0.0f;

    private void Start() {
        mat = GetComponent<Renderer>().material;
    }

    private void Update() {
        //mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2 + 0.5f);
        mat.SetFloat("_DissolveAmount", counter);
        counter += 0.001f;
    }
}