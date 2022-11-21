using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class YaPlayer
{
    public string Name => _name;
    public string ImageURL => _imageUrl;
    public string UniqueID => _uniqueID;
    public bool IsAutorized => _autorized;

    private string _name = null;
    private string _imageUrl = null;
    private string _uniqueID = null;
    private bool _autorized = false;
    private Dictionary<string, string> _stats = new Dictionary<string, string>();

    public YaPlayer(string name, string imageUrl, string uniqueID)
    {
        _name = name;
        _imageUrl = imageUrl;
        _uniqueID = uniqueID;
    }

    public YaPlayer()
    {
        _name = "Guest";
        _imageUrl = "https://cdn.tutsplus.com/mac/authors/jacob-penderworth/user-black.png";
        _uniqueID = "-1";
    }

    public void MarkAutorized()
    {
        _autorized = true;
    }

    public string GetStat(string key)
    {
        if(_stats.ContainsKey(key))
        {
            return _stats[key];
        }
        return "ERR";
    }

    public void SetStat(string key,string value)
    {
        if (_stats.ContainsKey(key))
        {
            _stats[key] = value;
        }
        else
        {
            _stats.Add(key, value);
        }
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetImgUrl(string url)
    {
        _imageUrl = url;
    }

    public void SetUID(string UID)
    {
        _uniqueID = UID;
    }
}
