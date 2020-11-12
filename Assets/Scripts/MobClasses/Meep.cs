using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meep : MOB
{
    private bool grown;
    private Vector2 target;
    private int chosenState, randomness;   
    
    // Initialize Meep
    void Start()
    {
        grown = false;
        //create a random name
        int max = Random.Range(5, 11);
        for(int length = 1; length < max; length++)
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
        //Find Self
        self = this.GetComponentInParent<Transform>();


        //spawn in Meep
        self.localScale = new Vector2(0, 0);
        Instantiate(GameManager.GAME.poof, self.position, Quaternion.identity);
        Camera.main.GetComponent<CameraController>().Camera2Target(self.position.x, self.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Grow Meep at beginning of life
        if (!grown) self.localScale = new Vector2(self.localScale.x + .01f, self.localScale.y + .01f);
        if (!grown && self.localScale.x > 1) { self.localScale = new Vector2(1, 1); grown = true; }

        //Animaton Special: Blink
        if(!sleeping) randomness = Random.Range(0, 201); if (randomness == 50) self.GetComponent<Animator>().SetBool("Meep_Blink", true);


        //Animation update        
        if(!walking && !sleeping) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 0); //Idle if nothing else.
        if (walking) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 1);
        if (sleeping) self.GetComponent<Animator>().SetInteger("Meep_Animation_State", 3);

        if (state == 0 || taskList.Count == 0) //Pick new state
        {
            chosenState = 1; //<--test this one
            if (fatigue > energy) chosenState = 101; //Force Meep to sleep


            if(chosenState == 1) //Wander
            {
                state = 1;
                target = GameManager.GAME.GetRandomRoomCoords();
                Job task = new Job();
                task.name = "Wandering"; task.target = target;
                taskList.Add(task);
            }

            if(chosenState == 101) //Sleep
            {
                state = 101;
                Job task = new Job();
                task.name = "Sleeping"; task.time = Random.Range(3, 13);
                taskList.Insert(0, task);
            }
        }
    }

    void FixedUpdate()
    {
        if (taskList.Count > 0 && taskList[0].name == "Wandering")
        {
            moveX = 0; moveY = 0;
            if (self.position.x > (taskList[0].target.x + .5)) moveX = -(speed / 2);
            if (self.position.x < (taskList[0].target.x - .5)) moveX = (speed / 2);
            if (self.position.y > (taskList[0].target.y + .5)) moveY = -(speed / 2);
            if (self.position.y < (taskList[0].target.y - .5)) moveY = (speed / 2);
            self.position = new Vector2(self.position.x + moveX, self.position.y + moveY);
            if (moveX == 0 && moveY == 0) { taskList.RemoveAt(0); walking = false; }
            if (moveX != 0 || moveY != 0)
            {
                walking = true;
                fatigue += .5f;
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
