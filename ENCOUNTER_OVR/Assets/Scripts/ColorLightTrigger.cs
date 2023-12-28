using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLightTrigger : MonoBehaviour
{
    //Declare color material to swap
    public Material matToHighlight_Blue;
    public Material matToHighlight_Pink;
    private Material matToDehighlight;

    //Get components
    private Material matNow;
    private MeshRenderer thisMeshRenderer;

    //Assign ID to pads
    public int Id;

    //Assign color to display
    private Vector3 rgb;

    // Start is called before the first frame update
    void Start()
    {
        //find original color
        thisMeshRenderer = GetComponent<MeshRenderer>();
        matToDehighlight = thisMeshRenderer.material;

        //define color now
        matNow = matToDehighlight;
    }

    // Update is called once per frame
    void Update()
    {
        thisMeshRenderer.material = matNow;

    }

    //highlighted when stooped on
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Client" || other.gameObject.tag == "Player_Server")
        {
            
            //display new color
            if (other.gameObject.tag == "Player_Client")
            {
                //highlight in pink
                highlight(matToHighlight_Pink);

                //send message to arduino event handler
                rgb = new Vector3(0, 0, 255);
                ArduinoEventHandler.currentEvent.colorHighlighted(0, Id, rgb);

            }
            else if (other.gameObject.tag == "Player_Server")
            {
                //highlight in blue
                highlight(matToHighlight_Blue);

                //send message to arduino event handler
                rgb = new Vector3(255, 0, 0);
                ArduinoEventHandler.currentEvent.colorHighlighted(0, Id, rgb);
            }
            
        }
    }

    //dehighlighted when stooped off
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player_Client" || other.gameObject.tag == "Player_Server")
        {
            
            //display new color
            deHighlight();

            //send message to arduino event handler
            rgb = new Vector3(0, 0, 0);
            ArduinoEventHandler.currentEvent.colorDehighlighted(0, Id, rgb);
        }
    }

    //swap material
    void highlight (Material thisMatToHighlight)
    {
        if (matNow == matToDehighlight)
        {
            matNow = thisMatToHighlight;
        }
    }

    void deHighlight()
    {
        if (matNow == matToHighlight_Blue || matNow == matToHighlight_Pink)
        {
            matNow = matToDehighlight;
        }
    }

}
