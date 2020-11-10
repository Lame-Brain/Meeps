using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //STATIC
    public static GameManager GAME;

    //INFOBOX
    public GameObject InfoBoxPanel;
    public Text l1Txt, l2Txt, l3Txt, l4Txt;
    public List<string> lines2Output = new List<string>();
    private string line1, line2, line3, line4;
    private int infoboxUpdateSpeed = 1, nextInfoboxUpdate, timesWithoutUpdate = 0;

    //Map Navigation
    public int mazeX, mazeY;

    //Room Drawing
    public GameObject wallPrefab;
    public GameObject[] tilePrefab, tileMaskPrefab;
    
    //*******************************************************************************************************************************************************************************
    //                                    UNITY METHODS

    void Awake()// <------------------------------------------------------------AWAKE
    {
        if (GAME == null)
        {
            GAME = this;
            DontDestroyOnLoad(GAME);
        }
        else Destroy(this);
    }

    void Start() // <------------------------------------------------------------START
    {
        for (int a = 3; a > 0; a--) Output("" + a);
        Output("the end", "shake");
        Map maze = new Map(9,9);
        mazeX = Random.Range(0, 8); mazeY = Random.Range(0, 8);
        DrawRoom();
    }

    void Update() // <------------------------------------------------------------UPDATE
    {
        //INFOBOX
        if (Time.time >= nextInfoboxUpdate)
        {
            nextInfoboxUpdate = Mathf.FloorToInt(Time.time) + infoboxUpdateSpeed;
            InfoBoxLogic();
        }
    }
    //*******************************************************************************************************************************************************************************
    //                                    CUSTOM METHODS

    //INFOBOX
    private void InfoBoxLogic()
    {
        if (lines2Output.Count > 0)
        {
            //setup infobox
            timesWithoutUpdate = 0;
            InfoBoxPanel.SetActive(true);
            //look for commands
            string input = lines2Output[0];
            if (input.Length > 0 && input.Substring(0, 1) == "#")
            {
                string fx = input.Substring(1, input.Length - 1);
                Debug.Log(fx);
                lines2Output.RemoveAt(0);
                input = lines2Output[0];
                //process commands
                if (fx == "shake") Camera.main.GetComponent<CameraController>().shakeTime = 150;
            }
            //output strings
            if (line3 != "") line4 = line3;
            if (line2 != "") line3 = line2;
            if (line1 != "") line2 = line1;
            line1 = input;
            lines2Output.RemoveAt(0);
        }
        else timesWithoutUpdate++;
        if (timesWithoutUpdate > (infoboxUpdateSpeed * 2)) InfoBoxPanel.SetActive(false);
        if (InfoBoxPanel.activeInHierarchy && line1 == "" && line2 == "" && line3 == "" && line4 == "") InfoBoxPanel.SetActive(false);
        l1Txt.text = line1; l2Txt.text = line2; l3Txt.text = line3; l4Txt.text = line4;
    }
    public void Output(string s) { lines2Output.Add(s); }
    public void Output(string s, string fx) { lines2Output.Add("#" + fx); lines2Output.Add(s); }

    //Draw Rooms
    public void DrawRoom()
    {
        Debug.Log("Room = " + Map.MAP.room[mazeX,mazeY].name);
        //Clear any existing room data
        GameObject[] tileObjs = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject foundObj in tileObjs) Destroy(foundObj);
        // Draw the room
        GameObject go = null; int t = 0, m = 0;
        for(int y = 0; y <= Map.MAP.room[mazeX, mazeY].height; y++)
        {
            for (int x = 0; x <= Map.MAP.room[mazeX, mazeY].width; x++)
            {
                t = Map.MAP.room[mazeX, mazeY].tile[x, y]; m = Map.MAP.room[mazeX, mazeY].mask[x, y];
                if (t == 99) go = Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                if (t < 99) go = Instantiate(tilePrefab[t], new Vector3(x, y, 0), Quaternion.identity);
                go.GetComponent<SpriteRenderer>().color = new Color(Map.MAP.room[mazeX, mazeY].r / 255, Map.MAP.room[mazeX, mazeY].g / 255, Map.MAP.room[mazeX, mazeY].b / 255, 255);
                go = Instantiate(tileMaskPrefab[m], new Vector3(x, y, 0), Quaternion.identity);
                go.GetComponent<SpriteRenderer>().color = new Color(Map.MAP.room[mazeX, mazeY].r / 255, Map.MAP.room[mazeX, mazeY].g / 255, Map.MAP.room[mazeX, mazeY].b / 255, 255);
            }
        }
    }
}
