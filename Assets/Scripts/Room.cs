using System.Collections;
using System.Collections.Generic;

public class Room
{
    public int width, height;
    public int[,] tile;
    public float r, g, b;
    //************************************************
    private int lowerBound = 150, upperBound = 500;//*
    //************************************************

    public Room(int xInput, int yInput)
    {
        if (xInput < 3) xInput = 3; if (yInput < 3) yInput = 3; // Room cannot be less than 3x3
        width = xInput; height = yInput;
        tile = new int[width+1, height+1]; //initalize tile array
        for(int y = 1; y < height-1; y++) // this sets the floors
        {
            for(int x = 1; x < width-1; x++)
            {
                int r = UnityEngine.Random.Range(0, 23); //8 + 8 + 7
                if (r > 7) r -= 8; //This should bias it toward blank tiles of different shades.
                tile[x, y] = r;
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
