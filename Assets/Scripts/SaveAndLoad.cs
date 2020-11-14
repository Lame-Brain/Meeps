using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{
    public static SaveSlot[] saveSlot = new SaveSlot[4];
    public static int SelectedSlot;

    public static void Save(int i)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/savegame" + i + ".mep")) File.Delete(Application.persistentDataPath + "/savegame" + i + ".mep");
        FileStream file = File.Create(Application.persistentDataPath + "/savegame"+i+".mep");
        bf.Serialize(file, saveSlot[SelectedSlot]);
        file.Close();
    }

    public static void Load(int i)
    {
        if(File.Exists(Application.persistentDataPath + "/savegame" + i + ".mep"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savegame" + i + ".mep", FileMode.Open);
            saveSlot[i] = ((SaveSlot)bf.Deserialize(file));
        }
    }
}
