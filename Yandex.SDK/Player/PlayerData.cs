using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData
{
    [DllImport("__Internal")]
    private static extern void SaveIncrementStat(string key,int val);
    [DllImport("__Internal")]
    private static extern void SaveData(string key, string val);
    [DllImport("__Internal")] 
    private static extern void SaveStat(string key, int val);
    [DllImport("__Internal")] 
    private static extern void SavePlayerData(string json);

    private Dictionary<string, string> Data = new Dictionary<string, string>();
    private Dictionary<string, int> Stats = new Dictionary<string, int>();

    public PlayerData(PlayerDataSave save = null)
    {
        if(save == null)
        {
            Debug.LogWarning("Save not defined. Created new data");
            return;
        }
        if(save.DataKeys.Count != save.DataValues.Count)
            Debug.LogError("Data keys and Data values count not match");
        if (save.StatsKeys.Count != save.StatsValues.Count)
            Debug.LogError("Stats keys and Stats values count not match");
        for(int i=0;i<save.DataKeys.Count;i++)
        {
            if (save.DataValues.Count < i)
                break;
            Data.Add(save.DataKeys[i], save.DataValues[i]);
        }
        for (int i = 0; i < save.StatsKeys.Count; i++)
        {
            if (save.StatsValues.Count < i)
                break;
            Stats.Add(save.StatsKeys[i], save.StatsValues[i]);
        }
        Debug.Log("PlayerData loaded");
        foreach(var key in Data.Keys.ToArray())
        {
            Debug.Log($"DATA: {key} - {Data[key]}");
        }
        foreach (var key in Stats.Keys.ToArray())
        {
            Debug.Log($"STATS: {key} - {Stats[key]}");
        }
    }

    public void SetStats(string key, int value)
    {
        try { 
        if(Stats.ContainsKey(key))
        {
            Stats[key] = value;
        }
        else
        {
            Stats.Add(key, value);
        }
        //SaveStat(key, value);
    }
        catch(Exception e)
        {
            Debug.LogError("Failed to set Data!");
            Debug.LogError("Data Dictionary: " + ((Data == null)?"null":Data.ToString()));
            Debug.LogError(e.Message);
        }
    }

    public void SetData(string key, string value)
    {
        try
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = value;
            }
            else
            {
                Data.Add(key, value);
            }
            //SaveData(key, value);
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to set Data!");
            Debug.LogError("Data Dictionary: " + ((Stats == null) ? "null" : Stats.ToString()));
            Debug.LogError(e.Message);
        }
    }

    public int GetStat(string key)
    {
        if (Stats.ContainsKey(key))
        {
            return Stats[key];
        }
        return -1;
    }

    public string GetData(string key)
    {
        if (Data.ContainsKey(key))
        {
            return Data[key];
        }
        return "null";
    }

    public int IncrementStat(string key,int value)
    {
        try
        {
            if (Stats.ContainsKey(key))
            {
                SaveIncrementStat(key, value);
                return Stats[key] += value;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to set Data!");
            Debug.LogError("Data Dictionary: " + ((Stats == null) ? "null" : Stats.ToString()));
            Debug.LogError(e.Message);
        }
        return -1;
    }

    public void SavePlayer()
    {
        PlayerDataSave save = new PlayerDataSave(Data,Stats);
        string json = JsonUtility.ToJson(save);
        SavePlayerData(json);
    }
}
