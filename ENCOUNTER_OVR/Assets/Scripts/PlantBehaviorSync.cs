using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlantBehaviorSync : Bolt.EntityBehaviour<IPlantState>
{
     //plant types
    public enum plantTypes {Flower, Grass, Mushroom, Others, ShortGrass, Ivy};
    public plantTypes myType;
    
    //growing animation parameters
    private float ultimateScale;
    private float ultimatePos;
    private Vector3 scaleFactor;
    private Vector3 growSpeed;

    //life span
    public float plantLifeSpan;
    private float timer;

    //light
    private GameObject myLight;
    private bool callingResponse;

    //emission
    private Color[] emissionColor = new Color[2];

    //partner close timer
    private float partnerCloseTimer = 0;
    public bool partnerClose;
    
    private void Start()
    {
        StartRandomization();

        //set timer
        plantLifeSpan = 60.0f;
        timer = 0.0f;

        //get light
        if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
        {
            if (transform.childCount > 0)
            {
                myLight = transform.GetChild(0).gameObject;
                myLight.GetComponent<Light>().range = 0;
                myLight.GetComponent<Light>().intensity = 0;
            }
        }

        //initialize emissive
        for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
        {
            emissionColor[i] = GetComponent<MeshRenderer>().materials[i].color;
        }
    }
 
    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.PlantTransform, gameObject.transform);

    }

    private void Update()
    {
        Growing();

          //timer add up by real time
            timer += Time.deltaTime;

            if (timer > plantLifeSpan)
            {
                if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
                {
                    if (transform.childCount > 0)
                    {
                         myLight.GetComponent<Light>().range -= 0.1f;
                    }
                }
            
                PlantDecay();
            }
            else
            {
                //player calling respond
                CallingRespond();
                //player close respond
                PlayerCloseRespond();
            }

        //toggle on close respond for debugging

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerCloseRespond_On();

        }else if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerCloseRespond_Off();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CallingSphere")
        {
            callingResponse = true;
        }
    }
    
    //player getting closer, light up plants
    public void PlayerCloseRespond_On()
    {
        partnerClose = true;
        plantLifeSpan = plantLifeSpan + 20.0f;
    }

    public void PlayerCloseRespond_Off()
    {
        partnerClose = false;
    }

    public void PlayerCloseRespond()
    {
        if (partnerClose)
        {
            //toggle timer on
            partnerCloseTimer += Time.deltaTime/3;
           
            if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
            {
                if (transform.childCount > 0)
                {
                    for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
                    {
                        Color lerpThisColor = Color.Lerp(Color.black, emissionColor[i], partnerCloseTimer);
                        GetComponent<MeshRenderer>().materials[i].SetColor("_EmissionColor", lerpThisColor);
                    }

                    if (myLight.GetComponent<Light>().range < 0.75f)
                    {
                        myLight.GetComponent<Light>().range = Mathf.Lerp(0.0f, 0.75f, partnerCloseTimer);
                    }

                    if (myLight.GetComponent<Light>().intensity < 1.0f)
                    {
                        myLight.GetComponent<Light>().intensity = Mathf.Lerp(0.0f, 1.0f, partnerCloseTimer);
                    }
                    
                }
            }
        }
        else
        {
            //toggle timer off
            if (partnerCloseTimer > 0.0f)
            {
                if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
                {
                    if (transform.childCount > 0)
                    {
                        for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
                        {
                            if (emissionColor[i] != Color.black)
                            {
                                Color lerpThisColor = Color.Lerp(emissionColor[i], Color.black, partnerCloseTimer);
                                GetComponent<MeshRenderer>().materials[i].SetColor("_EmissionColor", lerpThisColor);
                            }
                        }

                        if (myLight.GetComponent<Light>().range > 0.0f)
                        {
                            float currentLightRange = myLight.GetComponent<Light>().range;
                            myLight.GetComponent<Light>().range = Mathf.Lerp(currentLightRange, 0.0f, partnerCloseTimer);
                        }

                        if (myLight.GetComponent<Light>().intensity > 0.0f)
                        {
                            float currentLightIntensity = myLight.GetComponent<Light>().intensity;
                            myLight.GetComponent<Light>().intensity = Mathf.Lerp(currentLightIntensity, 0.0f, partnerCloseTimer);
                        }
                        //toggle timer off
                        else
                        {
                            partnerCloseTimer = 0.0f;
                        }
                    }
                }
            }
            
        }
    }


    //player calling each other, light up plants
    private void CallingRespond()
    {
        if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
        {
            if (transform.childCount > 0)
            {
                if (callingResponse)
                {
                    for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
                    {
                        Color lerpThisColor = Color.Lerp(Color.black, emissionColor[i], Mathf.PingPong(Time.time / 4, 0.75f));
                        GetComponent<MeshRenderer>().materials[i].SetColor("_EmissionColor", lerpThisColor);
                    }

                    myLight.GetComponent<Light>().range = Mathf.PingPong(Time.time / 4, 0.75f);
                    myLight.GetComponent<Light>().intensity = Mathf.PingPong(Time.time / 4, 2.0f);

                    if (myLight.GetComponent<Light>().range < 0.01f)
                    {
                        callingResponse = false;
                    }
                }
            }
        }
    }

    public void PlantDecay()
    {
        for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
        {
            Color thisMaterialColor = GetComponent<MeshRenderer>().materials[i].color;
            thisMaterialColor.a -= 0.005f;
            GetComponent<MeshRenderer>().materials[i].color = thisMaterialColor;

            Color thisMaterialEmissiveColor = GetComponent<MeshRenderer>().materials[i].GetColor("_EmissionColor");

            if (thisMaterialEmissiveColor != Color.black)
            {
                Color lerpThisColor = Color.Lerp(thisMaterialEmissiveColor, Color.black, timer);
                GetComponent<MeshRenderer>().materials[i].SetColor("_EmissionColor", lerpThisColor);
            }

            if (GetComponent<MeshRenderer>().materials[i].color.a < -1)
            {
                transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

                if (transform.localScale.y < 0.1f)
                {
                    PlantWither();
                    //BoltNetwork.Destroy(gameObject);
                }
            }
        }
    } 

    public void PlantWither()
    {
        GameObject serverPlayer;
        GameObject clientPlayer;
        serverPlayer = GameObject.FindWithTag("Player_Server");
        clientPlayer = GameObject.FindWithTag("Player_Client");

        if (serverPlayer != null)
        {
            serverPlayer.SendMessage("SelfDestroyCommand", gameObject);
        }

        if (clientPlayer != null)
        {
            clientPlayer.SendMessage("SelfDestroyCommand", gameObject);
        }

    }

    private void StartRandomization()
    {
        //rotation angles
        transform.eulerAngles = new Vector3(Random.Range(-5, 5), Random.Range(-180, 180), Random.Range(-5, 5));

        //Types and start scale and poses randomization
        if (myType == plantTypes.Flower || myType == plantTypes.Mushroom)
        {
            float randomScaleUnit = Random.Range(0.15f, 0.4f);
            transform.localScale = new Vector3(randomScaleUnit, randomScaleUnit, randomScaleUnit);
            ultimateScale = Random.Range(0.7f, 1.25f);
            float scaleRandomSeed = Random.Range(0.0015f, 0.003f);
            scaleFactor = new Vector3(scaleRandomSeed, scaleRandomSeed, scaleRandomSeed);
        }

        else if (myType == plantTypes.Grass || myType == plantTypes.Others)
        {
            float randomStartPos = Random.Range(-0.2f, -0.15f);
            transform.position += new Vector3(0, randomStartPos, 0);
            ultimatePos = Random.Range(-0.02f, 0.03f);
            float randomGrowSpeed = Random.Range(0.0008f, 0.0015f);
            growSpeed = new Vector3(0, randomGrowSpeed, 0);
        }

        else if (myType == plantTypes.ShortGrass)
        {
            float randomStartPos = Random.Range(-0.15f, -0.1f);
            transform.position += new Vector3(0, randomStartPos, 0);
            ultimatePos = Random.Range(-0.0002f, 0.0f);
            float randomGrowSpeed = Random.Range(0.0005f, 0.001f);
            growSpeed = new Vector3(0, randomGrowSpeed, 0);
        }

        else if (myType == plantTypes.Ivy)
        {
            float randomScaleUnit = Random.Range(0.15f, 0.4f);
            transform.localScale = new Vector3(randomScaleUnit, randomScaleUnit, randomScaleUnit);
            ultimateScale = Random.Range(0.2f, 0.4f);
            float scaleRandomSeed = Random.Range(0.00015f, 0.0003f);
            scaleFactor = new Vector3(scaleRandomSeed, scaleRandomSeed, scaleRandomSeed);
        }
    }

    private void Growing()
    {
        if (myType == plantTypes.Flower || myType == plantTypes.Mushroom || myType == plantTypes.Ivy)
        {
            if (transform.localScale.y < ultimateScale)
            {
                transform.localScale += scaleFactor;
            }
        }
        else if (myType == plantTypes.Grass || myType == plantTypes.Others || myType == plantTypes.ShortGrass)
        {
            if (transform.position.y < ultimatePos)
            {
                transform.position += growSpeed;
            }
        }
    }

}
