using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cam;
    private bool isCamera;
    private float camX, camY;

    public int shakeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Camera>()) { cam = this.transform; isCamera = true; }
        if (!this.GetComponent<Camera>()) isCamera = false;
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = new Vector3(camX, camY, -10);
        if (isCamera)
        {





            if(shakeTime > 0)
            {
                //Debug.Log("shake time = " + shakeTime);
                shakeTime--;
                cam.position = new Vector3(camX + Random.Range(-.1f, .1f), camY + Random.Range(-.1f, .1f), -10);
            }
        }        
    }
}
