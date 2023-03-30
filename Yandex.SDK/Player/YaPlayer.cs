using System;
using UnityEngine;

[Serializable]
public class YaPlayer
{
    public string UID => uID;
    public string Name => name;
    public bool IsAuth => isAuth;
    public string SmallPhotoURL => smallPhoto;
    public string MediumPhotoURL => mediumPhoto;
    public string LargePhotoURL => largePhoto;

    [SerializeField] private string uID;
    [SerializeField] private string name;
    [SerializeField] private bool isAuth;
    [SerializeField] private string smallPhoto;
    [SerializeField] private string mediumPhoto;
    [SerializeField] private string largePhoto;
}
