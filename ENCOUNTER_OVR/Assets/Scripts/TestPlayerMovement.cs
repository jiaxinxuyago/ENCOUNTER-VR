using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{

    private float moveSpeed = 0;

    //SERIAL DATA
    public int matrixX;
    public int matrixY;
    //Assign color to display on arduino
    private Vector3 rgb;
    public bool isCallingPartner;

    //initialize calling sphere
    [SerializeField] private GameObject callingSphere;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gameObject.transform.position += new Vector3(0.0f, 0.0f, moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.transform.position += new Vector3(0.0f, 0.0f, -moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.position += new Vector3(moveSpeed, 0.0f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
        }

        callingPartner();
        isCallingPartner = Input.GetKeyDown(KeyCode.Return);
    }

    public void callingPartner()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BoltNetwork.Instantiate(callingSphere, transform.position, Quaternion.identity);
        }
    }
}
