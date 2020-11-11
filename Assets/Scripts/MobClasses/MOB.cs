using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOB : MonoBehaviour
{
    public string mobName;
    public float speed, attackSpeed, attack, damage, weapon, defense, armor, health, wounds, energy, fatigue, buildSkill, workSkill, gatherSkill, exploreSkill, moveX, moveY;
    public int state;
    public Transform self;
    public List<Job> taskList = new List<Job>();
    public bool walking = false, sleeping = false;
}
