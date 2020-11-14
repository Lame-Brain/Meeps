using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOB : MonoBehaviour
{
    public string mobName;
    public float speed, attackSpeed, attack, damage, weapon, defense, armor, health, wounds, energy, fatigue, buildSkill, workSkill, gatherSkill, exploreSkill, moveX, moveY;
    public int state;
    public Vector2Int room;
    public Transform self;
    public List<Job> taskList = new List<Job>();
    public bool walking = false, sleeping = false;

    public void Spawn(float x, float y)
    {
        self.position = new Vector2(x, y);
        self.localScale = new Vector2(0, 0);
        Instantiate(GameManager.GAME.poof, self.position, Quaternion.identity);
        Camera.main.GetComponent<CameraController>().Camera2Target(self.position.x, self.position.y);
        InvokeRepeating("GrowUp", .01f, .01f);
    }

    private void GrowUp()
    {
        self.localScale = new Vector2(self.localScale.x + .01f, self.localScale.y + .01f);
        if (self.localScale.x > 1)
        {
            self.localScale = new Vector2(1, 1);
            CancelInvoke("GrowUp");
        }
    }
}
