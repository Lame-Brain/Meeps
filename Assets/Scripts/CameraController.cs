using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cam;
    private bool isCamera;
    private float camX, camY, movX, movY, movZ, zoom;

    [HideInInspector]
    public int shakeTime = 0;
    public float speed, zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Camera>()) { cam = this.transform; isCamera = true; }
        if (!this.GetComponent<Camera>()) isCamera = false;
        zoom = cam.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        cam.position = new Vector3(camX, camY, -10);
        if (isCamera)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow)) movY = speed; if (Input.GetKeyUp(KeyCode.UpArrow)) movY = 0;
            if (Input.GetKeyDown(KeyCode.DownArrow)) movY = -speed; if (Input.GetKeyUp(KeyCode.DownArrow)) movY = 0;
            if (Input.GetKeyDown(KeyCode.RightArrow)) movX = speed; if (Input.GetKeyUp(KeyCode.RightArrow)) movX = 0;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) movX = -speed; if (Input.GetKeyUp(KeyCode.LeftArrow)) movX = 0;
            if (Input.GetKey(KeyCode.PageUp)) zoom += zoomSpeed; if (Input.GetKey(KeyCode.PageDown)) zoom -= zoomSpeed;
            camX += movX; camY += movY;
            if (zoom < 1) zoom = 1; if (zoom > 10) zoom = 10;
            cam.GetComponent<Camera>().orthographicSize = zoom;

            if (shakeTime > 0)
            {
                //Debug.Log("shake time = " + shakeTime);
                shakeTime--;
                cam.position = new Vector3(camX + Random.Range(-.1f, .1f), camY + Random.Range(-.1f, .1f), -10);
            }
        }        
    }

    public void Camera2Target(float x, float y)
    {
        camX = x; camY = y;
        movX = 0; movY = 0;
    }
}
