using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private const string savePath = "/playerstats.dat"; 
    
    public static void SavePlayerStats(PlayerStats playerStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + savePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerStatsData data = new PlayerStatsData(playerStats);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerStatsData LoadPlayerStats()
    {
        string path = Application.persistentDataPath + savePath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStatsData data = formatter.Deserialize(stream) as PlayerStatsData;
            stream.Close();
            
            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
