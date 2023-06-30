using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;

namespace Logzep.YandexSDK.Utils
{
    [RequireComponent(typeof(Image))]
    public class AvatarLoader: MonoBehaviour
    {
        [Header("Avatar settings")]
        [SerializeField] private Image UIImage;
        [Tooltip("Should keep default picture if avatar is not loaded")]
        [SerializeField] private bool UseDefaultPicture;
        [SerializeField] private Sprite DefaultPicture;

        private UnityEngine.Texture? texture;

        private void Start()
        {
            if(UIImage == null)
            {
                UIImage = GetComponent<Image>();
                if (UIImage == null)
                    throw new Exception(gameObject.name+": AvatarLoader not found Image component!");
            }
            if (UseDefaultPicture)
            {
                UIImage.sprite = DefaultPicture;
            }
            else
            {
                UIImage.color = new Color(255f, 255f, 255f, 0f);
            }
        }

        public void Load(string avatarURL)
        {
            StartCoroutine(LoadAvatar(avatarURL));
        }

        private IEnumerator LoadAvatar(string avatarURL)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(avatarURL);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                if(UseDefaultPicture)
                    UIImage.sprite = DefaultPicture;
                else
                    UIImage.color = new Color(255f, 255f, 255f, 0f);
            }
            else
            {
                texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                UIImage.sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                UIImage.color = new Color(255f, 255f, 255f, 255f);
            }
        }


    }
}
