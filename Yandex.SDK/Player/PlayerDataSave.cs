using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class PlayerDataSave
{
    public List<string> StatsKeys { get; set; }
    public List<int> StatsValues { get; set; }
    public List<string> DataKeys { get; set; }
    public List<string> DataValues { get; set; }

    public PlayerDataSave() { }

    public PlayerDataSave(Dictionary<string,string> Data,Dictionary<string,int> Stats)
    {
        DataKeys = new List<string>();
        DataValues = new List<string>();
        StatsKeys = new List<string>();
        StatsValues = new List<int>();
        foreach(var key in Data.Keys.ToArray())
        {
            DataKeys.Add(key);
            DataValues.Add(Data[key]);
        }
        foreach (var key in Stats.Keys.ToArray())
        {
            StatsKeys.Add(key);
            StatsValues.Add(Stats[key]);
        }
    }
}
