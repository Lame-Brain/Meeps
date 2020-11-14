using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlot : MonoBehaviour
{
    public string gameName, savedTime;
    public Map map;
    public List<GameObject> MasterMobList = new List<GameObject>();

    public void AddMeep()
    {
        GameObject meep = Instantiate(GameManager.GAME.meep, new Vector3(0, 0, 0), Quaternion.identity);
        meep.GetComponent<Meep>().InitMeep();
        MasterMobList.Add(meep);
        meep.SetActive(false);
    }
    public void AddGoob()
    {
        GameObject goob = Instantiate(GameManager.GAME.goob, new Vector3(0, 0, 0), Quaternion.identity);
        MasterMobList.Add(goob);
        goob.SetActive(false);
    }
    public void AddWiiz()
    {
        GameObject wiiz = Instantiate(GameManager.GAME.wiiz, new Vector3(0, 0, 0), Quaternion.identity);
        MasterMobList.Add(wiiz);
        wiiz.SetActive(false);
    }
}
