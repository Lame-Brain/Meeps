using System.Collections;
using System.Collections.Generic;

public class Map
{
    public static Map MAP;
    public int width, height;
    public Room[,] room;

    public Map(int inputX, int inputY)
    {
        if (MAP != null) UnityEngine.Debug.Log("!!!ERROR!!! MAP ALREADY EXISTS!!!");
        if (MAP == null)
        { 
            MAP = this;
            width = inputX; height = inputY;
            NewMap();
        }
    }

    public void NewMap()
    {
        room = new Room[width, height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                room[x, y] = new Room(UnityEngine.Random.Range(8,16), UnityEngine.Random.Range(8,16), "Room"+x.ToString()+y.ToString());
            }
        }
    }
}
