using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerURLImage : MonoBehaviour
{
    [SerializeField]
    private RawImage _image;

    private Texture2D _texture;

   

    public void LoadURLImage(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            _image.texture = (((DownloadHandlerTexture)request.downloadHandler).texture);
    }
}
