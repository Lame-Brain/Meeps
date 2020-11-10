using System.Collections;
using System.Collections.Generic;

public class Room
{
    public string name;
    public int width, height;
    public int[,] tile, mask;
    public float r, g, b;
    //************************************************
    private int lowerBound = 150, upperBound = 500;//*
    //************************************************

    public Room(int xInput, int yInput, string nameInput)
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
                int r = UnityEngine.Random.Range(0, 8), m = UnityEngine.Random.Range(0, 100);
                tile[x, y] = r; //Set base tile
                r = UnityEngine.Random.Range(0, 11);
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
            r = UnityEngine.Random.Range(0, 255);
            g = UnityEngine.Random.Range(0, 255);
            b = UnityEngine.Random.Range(0, 255);
            if (r + g + b > lowerBound && r + g + b < upperBound) done = true;
            i++;
            if(i > 100) { r = 150f; g = 150f; b = 150f; done = true; } // escape clause
        }
    }
}
