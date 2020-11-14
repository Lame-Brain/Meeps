using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //STATIC
    public static GameManager GAME;
    public static UIController UIX;
    public static MouseCursorControl MOUSE;
    public static Room THIS_ROOM;
    public static SaveSlot FILE;

    //INFOBOX
    public GameObject CanvasMain;
    public Text l1Txt, l2Txt, l3Txt, l4Txt;
    public List<string> lines2Output = new List<string>();
    private string line1, line2, line3, line4;
    private int infoboxUpdateSpeed = 1, nextInfoboxUpdate, timesWithoutUpdate = 0;

    //Map Navigation
    public int mazeX, mazeY;

    //Room Drawing
    public GameObject wallPrefab;
    public GameObject[] tilePrefab, tileMaskPrefab;

    //MOBS
    public GameObject meep, goob, wiiz;

    //Mouse Interface
    public Vector2 mousePos;
    public Vector2 mousePosRaw;
    public bool mPosValidated = false;

    //Buildings
    public string buildMode = "none";
    public Transform mark;
    public GameObject hutPF;

    //Misc
    public GameObject poof;    

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
        UIX = CanvasMain.GetComponent<UIController>();
        MOUSE = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseCursorControl>();
        FILE = SaveAndLoad.saveSlot[0] = new SaveSlot(); //<---For Debug purposes. In release, this will be set by main menu;
    }

    void Start() 
    {
        InitializeGame(); // <------------------------------------------------------------START
        Debug.Log(System.DateTime.Now.ToString());
    }

    void Update() // <------------------------------------------------------------UPDATE
    {
        //INFOBOX
        if (Time.time >= nextInfoboxUpdate)
        {
            nextInfoboxUpdate = Mathf.FloorToInt(Time.time) + infoboxUpdateSpeed;
            InfoBoxLogic();
        }

        //Building
        if (buildMode == "Hut" && mPosValidated)
        {
            mPosValidated = false;
            Instantiate(mark, mousePos, Quaternion.identity);
            GameObject hut = Instantiate(hutPF, mousePos, Quaternion.identity); hut.transform.localScale = new Vector3(1, 0, 0);
            hut.GetComponent<Building>().InitBuilding();
            Job newJob = new Job(); newJob.name = "Build"; newJob.targetObj = hut;  newJob.targetCoord = hut.transform.position; newJob.targetRoom = new Vector2Int(mazeX, mazeY);
            THIS_ROOM.Orders.Add(newJob);
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
            UIX.InfoBoxPanel.SetActive(true);
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
        if (timesWithoutUpdate > (infoboxUpdateSpeed * 2)) UIX.InfoBoxPanel.SetActive(false);
        if (UIX.InfoBoxPanel.activeInHierarchy && line1 == "" && line2 == "" && line3 == "" && line4 == "") UIX.InfoBoxPanel.SetActive(false);
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
                if (m > 0 && m < 6) go.GetComponent<SpriteRenderer>().color = new Color(Map.MAP.room[mazeX, mazeY].r / 255, Map.MAP.room[mazeX, mazeY].g / 255, Map.MAP.room[mazeX, mazeY].b / 255, 255);
            }
        }
        //Mobs
        foreach (GameObject mob in GameManager.FILE.MasterMobList)
        {
            if (mob.tag == "Goob") if(mob.GetComponent<GOOB>().room == Map.MAP.room[mazeX, mazeY].room) { mob.SetActive(true); } else { mob.SetActive(false); }
            if (mob.tag == "Meep") if(mob.GetComponent<Meep>().room == Map.MAP.room[mazeX, mazeY].room) { mob.SetActive(true); } else { mob.SetActive(false); }
            if (mob.tag == "WIIZ") if(mob.GetComponent<WIIZ>().room == Map.MAP.room[mazeX, mazeY].room) { mob.SetActive(true); } else { mob.SetActive(false); }

            if (mob.transform.position.x == 0 && mob.transform.position.y == 0)
            {
                if (mob.tag == "Goob") mob.GetComponent<GOOB>().Spawn(1, 1); //<----Need to figure out where to spawn Goobs
                if (mob.tag == "Meep") mob.GetComponent<Meep>().Spawn(Random.Range(1, Map.MAP.room[mazeX, mazeY].width), Random.Range(1, Map.MAP.room[mazeX, mazeY].height));
                if (mob.tag == "Wiiz") mob.GetComponent<WIIZ>().Spawn(1, 1);//<----Need to figure out where to spawn Wiizs
            }
        }
    }
    public Vector2 GetRandomRoomCoords()
    {        
        return new Vector2(Random.Range(1, Map.MAP.room[mazeX, mazeY].width), Random.Range(1, Map.MAP.room[mazeX, mazeY].height));
    }

    public void BuildHutButton()
    {
        MOUSE.EnterValidateMode();
        buildMode = "Hut";
        mPosValidated = false;
    }

    //**********************************************************************************************************************
    //******************************************** THIS IS WHERE IT STARTS *************************************************
    //**********************************************************************************************************************
    public void InitializeGame()
    {
        Map maze = new Map(9, 9);
        mazeX = Random.Range(0, 8); mazeY = Random.Range(0, 8);
        THIS_ROOM = Map.MAP.room[mazeX, mazeY];
        THIS_ROOM.Clear_Orders();
        FILE.AddMeep(); FILE.AddMeep(); FILE.AddMeep();
        DrawRoom();        

        //for (int a = 3; a > 0; a--) Output("" + a);
        //Output("the end", "shake");
        //Instantiate(meep, new Vector3(Random.Range(1, Map.MAP.room[mazeX, mazeY].width), Random.Range(1, Map.MAP.room[mazeX, mazeY].height), 0), Quaternion.identity);
        //Instantiate(meep, new Vector3(Random.Range(1, Map.MAP.room[mazeX, mazeY].width), Random.Range(1, Map.MAP.room[mazeX, mazeY].height), 0), Quaternion.identity);
        //Instantiate(meep, new Vector3(Random.Range(1, Map.MAP.room[mazeX, mazeY].width), Random.Range(1, Map.MAP.room[mazeX, mazeY].height), 0), Quaternion.identity);

    }
}
