using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room: MonoBehaviour
{
    public new string name;
    public int width, height; //width and height of room
    public int[,] tile, mask; //tile shade and if there is mask over the tile.    
    public Vector2Int room;
    public List<int> mob_index = new List<int>(); //references MOBs in the room
    public List<Building> buildingList = new List<Building>();
    public List<Job> Orders = new List<Job>();

    public float r, g, b; //color of tiles
    //************************************************
    private int lowerBound = 150, upperBound = 500;//* Change this to define bounds of color generation
    //************************************************

    public Room(int xInput, int yInput, string nameInput, int roomLocX, int roomLocY)
    {
        name = nameInput;
        if (xInput < 3) xInput = 3; if (yInput < 3) yInput = 3; // Room cannot be less than 3x3
        width = xInput; height = yInput;
        tile = new int[width+1, height+1]; //initalize tile array
        mask = new int[width + 1, height + 1]; //initalize mask array
        for (int y = 1; y < height; y++) // this sets the floors
        {
            for(int x = 1; x < width; x++)
            {
                int r = Random.Range(0, 8), m = Random.Range(0, 100);
                tile[x, y] = r; //Set base tile
                r = Random.Range(0, 11);
                mask[x, y] = 0; //Mask defaults to 0
                if (m < 8 && m > 0) mask[x, y] = m; //Set mask
            }
        }
        //this sets the walls
        for (int y = 0; y < height; y++) 
        {
            tile[0, y] = 99;
            tile[width, y] = 99;
        }
        for (int x = 0; x < width; x++)
        {
            tile[x, 0] = 99;
            tile[x, height] = 99;
        }
        tile[width, height] = 99;
        //Determine Color
        bool done = false; int i = 0;
        while (!done)
        {
            r = Random.Range(0, 255);
            g = Random.Range(0, 255);
            b = Random.Range(0, 255);
            if (r + g + b > lowerBound && r + g + b < upperBound) done = true;
            i++;
            if(i > 100) { r = 150f; g = 150f; b = 150f; done = true; } // escape clause
        }
        room = new Vector2Int(roomLocX, roomLocY);
    }

    public void Clear_Orders() { Orders.Clear(); }
}
