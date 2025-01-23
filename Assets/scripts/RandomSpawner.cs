using System;
using System.Collections.Generic;
using TikTokLiveUnity;
using UnityEngine;
using System.IO;
using System.Collections;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveUnity.Utils;
using TMPro;
using UnityEngine.UI;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefab;

    

    async void Start()
    {
        Dictionary<long, int> giftValues = CreateDict();
        // enter a LIVE tiktok creator's username. This will connect the API to the tiktok creator
        var userName = "tiktokliveplinko";
        

        TikTokLiveManager.Instance.OnLike += (liveClient, likeEvent) => 
        {
            
            
            if (likeEvent.Count >= 10) {
                SpawnObject(likeEvent.Sender.AvatarThumbnail, likeEvent.Sender.Id, likeEvent.Sender.NickName);
                
                Debug.Log($"Thank you for like! {likeEvent.Count}");
            }
                
                
                
            
        };

        TikTokLiveManager.Instance.OnGift += (liveClient, giftEvent) => 
        {
            long giftID = giftEvent.Gift.Id;
            if (giftValues.TryGetValue(giftID, out int spawnCount))
            {
                for(int i = 0; i < spawnCount; i++) {
                    SpawnObject(giftEvent.Sender.AvatarThumbnail, giftEvent.Sender.Id, giftEvent.Sender.NickName);
                }
                Debug.Log($"Thank you for {giftEvent.Amount} Gift! {giftEvent.Gift.Name} from {giftEvent.Sender.NickName}");
            }
            else
            {
                Debug.LogWarning($"Unknown gift ID: {giftID}");
            }
        };
        await TikTokLiveManager.Instance.ConnectToStream(userName);
        
    }

    // private void RequestImage(Image img, Picture picture)
    //     {
    //         Dispatcher.RunOnMainThread(() =>
    //         {
    //             TikTokLiveManager.Instance.RequestSprite(picture, spr =>
    //             {
    //                 if (img != null && img.gameObject != null && img.gameObject.activeInHierarchy){
    //                     img.sprite = spr;
    //                     Canvas.ForceUpdateCanvases();
    //                 }
    //             });
    //         });
    //     }

    private void SpawnObject(Picture thumbnail, long tId, string tName)
    {
        Vector3 randomSpawnPosition = new Vector3(UnityEngine.Random.Range(-0.3f,0.3f), 4.5f, 0.1f);
        GameObject instance = Instantiate(prefab, randomSpawnPosition, Quaternion.identity);
        instance.GetComponent<tiktokID>().id = tId;
        instance.GetComponent<tiktokID>().name = tName;
        GameObject canvas = instance.transform.GetChild(0).gameObject;
        GameObject mask = canvas.transform.GetChild(0).gameObject;
        GameObject image = mask.transform.GetChild(0).gameObject;
        Image img = image.GetComponent<Image>();

        Dispatcher.RunOnMainThread(() =>
            {
                TikTokLiveManager.Instance.RequestSprite(thumbnail, spr =>
                {
                    if (img != null && img.gameObject != null && img.gameObject.activeInHierarchy){
                        img.sprite = spr;
                        instance.GetComponent<tiktokID>().sprite = img.sprite;
                        Canvas.ForceUpdateCanvases();
                    }
                });
            });
    }

    

    private Dictionary<long,int> CreateDict() 
    {
        Dictionary<long,int> dict = new Dictionary<long, int>();
        try
        {
            
            string filePath = Path.Combine(Application.streamingAssetsPath, "tiktokCodes.txt");

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] substring = line.Split(" ");
                    if (substring.Length == 2 && long.TryParse(substring[0], out long key) && int.TryParse(substring[1], out int value))
                    {
                        dict[key] = value;
                        Debug.Log($"Created Successfully Key {key} : Value {value}");
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid line in tiktokCodes.txt: {line}");
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
        }
        return dict;
    }

    
}