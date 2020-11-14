using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Meep : MOB
{
    private Vector2 target;
    private int chosenState, randomness;
    //public bool OVERRIDE = false; //<<<<-----------DEBUG
    

    public bool building = false;

    // Update is called once per frame
    void Update()
    {
        //Animaton Special: Blink
        if(!sleeping) randomness = Random.Range(0, 201); if (randomness == 50) self.GetComponent<Animator>().SetBool("Meep_Blink", true);


        //Animation update        
        if(!walking && !sleeping) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 0); //Idle if nothing else.
        if (walking) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 1);
        if (sleeping) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 3);
        if (building) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 5);

        //------------------------------------------------------
        if (taskList.Count == 0) //Pick new state|
        {

            chosenState = Random.Range(1,10);
            if (fatigue > energy) chosenState = 101; //Force Meep to sleep
            //if (!OVERRIDE) chosenState = 1; //<--test this one
            //if (!OVERRIDE && fatigue > energy) chosenState = 101; //Force Meep to sleep
            //if (OVERRIDE) { chosenState = 9; Debug.Log("OVERRIDE"); OVERRIDE = false; }


            if (chosenState == 9) //grab job from Room Order List
            {
                //if (GameManager.THIS_ROOM.Orders.Count == 0) Debug.Log("ERROR! NO ORDERS!");
                if (GameManager.THIS_ROOM.Orders.Count > 0)
                {
                    Job task = new Job(); task.name = "Walking"; task.targetCoord = GameManager.THIS_ROOM.Orders[0].targetCoord; taskList.Add(task);
                    task = new Job(); task.name = "Build"; task.targetObj = GameManager.THIS_ROOM.Orders[0].targetObj; taskList.Add(task);
                    GameObject[] foundmarks = GameObject.FindGameObjectsWithTag("Mark");
                    foreach (GameObject go in foundmarks)
                    {
                        if(go.transform.position == GameManager.THIS_ROOM.Orders[0].targetObj.transform.position) Destroy(go);
                    }
                    GameManager.THIS_ROOM.Orders.RemoveAt(0);
                }
            }
            if (chosenState < 8) //Wander
            {
                target = GameManager.GAME.GetRandomRoomCoords();
                Job task = new Job();
                task.name = "Wandering"; task.targetCoord = target;
                taskList.Add(task);
            }

            if(chosenState == 101) //Sleep
            {
                Job task = new Job();
                task.name = "Sleeping"; task.time = Random.Range(3, 13);
                taskList.Insert(0, task);
            }
        }
    }

    void FixedUpdate()
    {
        if (taskList.Count > 0 && taskList[0].name == "Walking")
        {
            moveX = 0; moveY = 0;
            if (self.position.x > (taskList[0].targetCoord.x + .5)) moveX = -speed;
            if (self.position.x < (taskList[0].targetCoord.x - .5)) moveX = speed;
            if (self.position.y > (taskList[0].targetCoord.y + .5)) moveY = -speed;
            if (self.position.y < (taskList[0].targetCoord.y - .5)) moveY = speed;
            self.position = new Vector2(self.position.x + moveX, self.position.y + moveY);
            if (moveX == 0 && moveY == 0) {
                taskList.RemoveAt(0); walking = false;
            }
            if (moveX != 0 || moveY != 0)
            {
                walking = true;
                fatigue += .5f;
            }
        }
        if (taskList.Count > 0 && taskList[0].name == "Build")
        {
            moveX = 0; moveY = 0;
            building = true;
            taskList[0].targetObj.GetComponent<Building>().built += .1f;
            float nys = taskList[0].targetObj.transform.localScale.y + .01f;
            fatigue += .5f;
            if(nys >= 1) { taskList[0].targetObj.GetComponent<Building>().built = 100; nys = 1; building = false; }
            taskList[0].targetObj.transform.localScale = new Vector3(1, nys, 1);
            if (!building)
            {
                taskList.RemoveAt(0); //I did this because I cannot remove the task before I finish calling it.
            }
        }
            if (taskList.Count > 0 && taskList[0].name == "Wandering")
        {
            moveX = 0; moveY = 0;
            if (self.position.x > (taskList[0].targetCoord.x + .5)) moveX = -(speed / 2);
            if (self.position.x < (taskList[0].targetCoord.x - .5)) moveX = (speed / 2);
            if (self.position.y > (taskList[0].targetCoord.y + .5)) moveY = -(speed / 2);
            if (self.position.y < (taskList[0].targetCoord.y - .5)) moveY = (speed / 2);
            self.position = new Vector2(self.position.x + moveX, self.position.y + moveY);
            if (moveX == 0 && moveY == 0) { taskList.RemoveAt(0); walking = false; }
            if (moveX != 0 || moveY != 0)
            {
                walking = true;
                fatigue += .2f;
            }
        }
        if(taskList.Count > 0 && taskList[0].name == "Sleeping")
        {
            sleeping = true; walking = false; moveX = 0; moveY = 0;
            taskList[0].time -= .5f;                
            fatigue -= .5f;
            if (fatigue < 0) fatigue = 0;
            if(fatigue == 0 || taskList[0].time < 0)
            {
                sleeping = false;
                taskList.RemoveAt(0);
            }
        }
        
    }

    // Initialize Meep
    public void InitMeep()
    {
        //create a random name
        int max = Random.Range(5, 11);
        for (int length = 1; length < max; length++)
        {
            mobName += (char)('A' + Random.Range(0, 26));
        }
        GameManager.GAME.Output("Hello, my name is " + mobName + "!");
        //randomize stats
        speed = Random.Range(.02f, .05f);
        attackSpeed = Random.Range(1f, 2f);
        attack = Random.Range(0f, 1f);
        damage = Random.Range(1f, 2f);
        weapon = 0;
        defense = Random.Range(1f, 2f);
        armor = 0;
        health = Random.Range(10f, 20f);
        wounds = 0;
        energy = Random.Range(20f, 30f);
        fatigue = 0;
        buildSkill = Random.Range(5f, 10f);
        workSkill = Random.Range(5f, 10f);
        gatherSkill = Random.Range(5f, 10f);
        exploreSkill = Random.Range(5f, 10f);
        state = 0;
        //find Room
        room = new Vector2Int(GameManager.GAME.mazeX, GameManager.GAME.mazeY);
        //Find Self
        self = this.GetComponentInParent<Transform>();
        self.name = mobName;
    }
}
/*
 * Things a Meep can do:
 * 0 = look for new thing to do
 * 1 = wander to random x,y target
 * 2 = target, approach, and admire random structure
 * 3 = target, apporach, and admire random meep * 
 * 4 = grab a job from the list
 * 5 + fatigue points = sleep
 * next + wounds points = seek healer hut or sleep * 
 * if fatigue is greater than energy = sleep
 * if wounds is greater than health = die
 */
