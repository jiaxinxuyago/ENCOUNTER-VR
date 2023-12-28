using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeedsGenerationSync : Bolt.EntityBehaviour<ISeedState>
{
    //Determine if it is special or not
    public bool isSpecial;

    private Scene myScene;

    public int slotID;
    public int plantID;

    private GameObject slot;

    public bool occupied = false;
    
    private MeshRenderer myMesh;

    //initiate generation array
    [SerializeField] private GameObject[] Flowers;
    [SerializeField] private GameObject[] Grass;
    [SerializeField] private GameObject[] Mushrooms;
    [SerializeField] private GameObject[] Others;
    [SerializeField] private GameObject[] Special;

    private GameObject plantToSpawn;
    private GameObject plantPossessed;

    //GENERATION
    private slotGenerationSync.randomTypes slotCommand;

    private void Start()
    {
        //Find my scene
        myScene = gameObject.scene;

        //Find Slot
        slot = gameObject.transform.parent.gameObject;
        slotGenerationSync parentScript = gameObject.transform.parent.GetComponent<slotGenerationSync>();

        //Set up plant ID
        for (int i = 0; i < slot.transform.childCount; i ++)
        {
            if(slot.transform.GetChild(i).gameObject == gameObject)
            {
                plantID = i;
            }
        }

        //Do not display
        myMesh = GetComponent<MeshRenderer>();
        myMesh.enabled = false;


        //Receive information from slot
        slotID = parentScript.mySlotID;
        slotCommand = parentScript.generationCommand;

        //Generation
        if(!isSpecial)
        {
            GeneratePlantToSpawn(slotCommand);
        }
        else
        {
            int randomIndex = Random.Range(0, 3);
            plantToSpawn = Special[randomIndex];
        }

        plantPossessed = gameObject;
    }

    private void Update()
    {
        if (plantPossessed == null)
        {
            plantPossessed = gameObject;
           
            occupied = false;

            transform.parent.gameObject.SendMessage("RemoveOneSeedOccupied");
        }
    }

    public override void Attached()
    {
        base.Attached();

        //sync transform state
        state.SetTransforms(state.SeedTransform, gameObject.transform);
        state.IsOccupied = occupied;
    }

    public void GrowSeed()
    {
        if(occupied == false)
        {
            BoltNetwork.Instantiate(plantToSpawn, gameObject.transform.position, Quaternion.identity);
            transform.parent.gameObject.SendMessage("AddOneSeedOccupied");
        }

        occupied = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plant")
        {
            //move to base scene
            SceneManager.MoveGameObjectToScene(other.gameObject, myScene);

            plantPossessed = other.gameObject;
        }
    }

    private void GeneratePlantToSpawn(slotGenerationSync.randomTypes thisCommand)
    {
       
        if (thisCommand == slotGenerationSync.randomTypes.JustGrass)
        {
            //generate grass
            GenerateGrass();
        }
        else if (thisCommand == slotGenerationSync.randomTypes.HasMoreFlower)
        {
            //generate flowers
            GenerateFlowers();
        }
        else if (thisCommand == slotGenerationSync.randomTypes.HasMoreMushrooms)
        {
            //generate mushrooms
            GenerateMushrooms();
        }
        else if (thisCommand == slotGenerationSync.randomTypes.HasMoreOthers)
        {
            //generate others
            GenerateOthers();
        }
        else if (thisCommand == slotGenerationSync.randomTypes.TotalRandom)
        {
            //generate random
            GenerateRandom();
        }
    }
    private void GenerateThreeMainTypes(int firstThreshold, int secondThreshold)
    {
        //initialize random seeds
        int randomSeed = Random.Range(0, 30);
        //grass generation
        if (randomSeed < firstThreshold)
        {
            plantToSpawn = Grass[Random.Range(0, Grass.Length)];
        }
        else
        {
            if (randomSeed < secondThreshold)
            {
                plantToSpawn = Flowers[Random.Range(0, Flowers.Length)];
            }
            else
            {
                plantToSpawn = Mushrooms[Random.Range(0, Mushrooms.Length)];
            }
        }
    }

    private void GenerateGrass()
    {
        GenerateThreeMainTypes(27, 29);
    }

    private void GenerateFlowers()
    {
        GenerateThreeMainTypes(20, 29);
    }

    private void GenerateMushrooms()
    {
        GenerateThreeMainTypes(19, 21);
    }
    private void GenerateOthers()
    {
        //initialize random seeds
        int randomSeed = Random.Range(0, 30);
        //grass generation
        if (randomSeed < 25)
        {
            plantToSpawn = Grass[Random.Range(0, Grass.Length)];
        }
        else
        {
            randomSeed = Random.Range(0, 10);
            if (randomSeed < 7)
            {
                plantToSpawn = Others[Random.Range(0, Others.Length)];
            }
            else
            {
                plantToSpawn = Others[3];
            }  
        }
    }

    private void GenerateRandom()
    {
        //initialize random seeds
        int randomSeed = Random.Range(0, 30);
        //grass generation
        if (randomSeed < 20)
        {
            plantToSpawn = Grass[Random.Range(0, Grass.Length)];
        }
        else
        {
            randomSeed = Random.Range(0, 15);
            if (randomSeed < 7)
            {
                plantToSpawn = Flowers[Random.Range(0, Flowers.Length)];
            }
            else
            {
                if (randomSeed < 10)
                {
                    plantToSpawn = Mushrooms[Random.Range(0, Mushrooms.Length)];
                }
                else
                {
                    randomSeed = Random.Range(0, 10);
                    if (randomSeed < 7)
                    {
                        plantToSpawn = Others[Random.Range(0, Others.Length)];
                    }
                    else
                    {
                        plantToSpawn = Others[3];
                    }
                }
            }
        }
    }
}
