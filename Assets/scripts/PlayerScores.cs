using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TikTokLiveUnity;
using System.IO;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveUnity.Utils;
using TMPro;

public class PlayerScores : MonoBehaviour
{

    public Dictionary<long, float> playerScores;
    float firstScore;
    float secondScore;
    float thirdScore;
    float fourthScore;

    Sprite firstPic;
    Sprite secondPic;
    Sprite thirdPic;
    Sprite fourthPic;

    string firstName;
    string secondName;
    string thirdName;
    string fourthName;

    long firstId;
    long secondId;
    long thirdId;
    long fourthId;

    // Start is called before the first frame update
    void Start()
    {
        playerScores = new Dictionary<long, float>();
            firstScore = 0f;
            secondScore = 0f;
            thirdScore = 0f;
    }

    // public void AddScore(long id, float scoreToAdd, string name, Sprite sprite) {
    //     if (!playerScores.ContainsKey(id)) {
    //         playerScores[id] = scoreToAdd;
    //     }
    //     else {
    //         playerScores[id] += scoreToAdd;
    //         }

    //     if (playerScores[id] > firstScore) {

    //         if (firstName == secondName) {
    //             secondScore = firstScore;
    //             thirdScore = secondScore;
                
            
            

    //             thirdPic = secondPic;
    //             secondPic = firstPic;
                

    //             thirdName = secondName;
    //             secondName = firstName;
                
    //         }
    //         firstName = name;
    //         firstPic = sprite;
    //         firstScore = playerScores[id];

    //         gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = firstPic;
    //         gameObject.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = secondPic;
    //         gameObject.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = thirdPic;
    //         gameObject.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "FIRST PLACE: " + firstName + " " + firstScore.ToString("N2") + " PTS";
    //         //move first to second
    //     }else if(playerScores[id] > secondScore){
            
    //         if(thirdName == secondName) {
    //             thirdScore = secondScore;
                

            
            
    //             thirdPic = secondPic;
                
    //             thirdName = secondName;
                

    //         }
    //         secondScore = playerScores[id];
    //         secondName = name;
    //         secondPic = sprite;

    //         gameObject.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = secondPic;
    //         gameObject.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = thirdPic;
    //         gameObject.transform.GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "SECOND PLACE: " + secondName + " " + secondScore.ToString("N2") + " PTS";
    //         //move second to third
    //     }else if(playerScores[id] > thirdScore){
    //         thirdScore = playerScores[id];
    //         thirdPic = sprite;
    //         thirdName = name;

    //         gameObject.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = thirdPic;
    //         gameObject.transform.GetChild(3).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "THIRD PLACE: " + thirdName + " " + thirdScore.ToString("N2") + " PTS";
    //         //nothing else besides updating third text
    //     }

        
           
    // }
    public void AddScore(long id, float scoreToAdd, string name, Sprite sprite)
{
    if (!playerScores.ContainsKey(id))
    {
        playerScores[id] = scoreToAdd;
    }
    else
    {
        playerScores[id] += scoreToAdd;
    }

    float score = playerScores[id];

    // Remove the player from their current position if they're already on the leaderboard
    RemovePlayerFromLeaderboard(id);

    fourthScore = score;
    fourthName = name;
    fourthId = id;
    fourthPic = sprite;
    

    // Update leaderboard
    if (score > firstScore)
    {
        thirdScore = secondScore;
        thirdName = secondName;
        thirdPic = secondPic;
        thirdId = secondId;

        secondScore = firstScore;
        secondName = firstName;
        secondPic = firstPic;
        secondId = firstId;

        firstScore = score;
        firstName = name;
        firstPic = sprite;
        firstId = id;
    }
    else if (score > secondScore)
    {
        thirdScore = secondScore;
        thirdName = secondName;
        thirdPic = secondPic;
        thirdId = secondId;

        secondScore = score;
        secondName = name;
        secondPic = sprite;
        secondId = id;
    }
    else if (score > thirdScore)
    {
        thirdScore = score;
        thirdName = name;
        thirdPic = sprite;
        thirdId = id;
    }

    // Update UI
    UpdateLeaderboardUI();
}

private void RemovePlayerFromLeaderboard(long id)
{
    if (id == firstId)
    {
        firstScore = secondScore;
        firstName = secondName;
        firstPic = secondPic;
        firstId = secondId;

        secondScore = thirdScore;
        secondName = thirdName;
        secondPic = thirdPic;
        secondId = thirdId;

        ResetThirdPlace();
    }
    else if (id == secondId)
    {
        secondScore = thirdScore;
        secondName = thirdName;
        secondPic = thirdPic;
        secondId = thirdId;

        ResetThirdPlace();
    }
    else if (id == thirdId)
    {
        ResetThirdPlace();
    }
}

private void ResetThirdPlace()
{
    thirdScore = float.MinValue;
    thirdName = "";
    thirdPic = null;
    thirdId = 0;
}

private void UpdateLeaderboardUI()
{
    UpdatePlaceUI(1, firstName, firstScore, firstPic);
    UpdatePlaceUI(2, secondName, secondScore, secondPic);
    UpdatePlaceUI(3, thirdName, thirdScore, thirdPic);
    UpdatePlaceUI(4, fourthName, fourthScore, fourthPic);
}

private void UpdatePlaceUI(int place, string name, float score, Sprite pic)
{
    string tName = "" + name;
    if (tName.Length > 10) {tName = tName.Substring(0, 9);}
    Transform placeTransform = gameObject.transform.GetChild(place);
    placeTransform.GetChild(1).GetComponent<Image>().sprite = pic;
    placeTransform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = 
        $"{GetPlaceName(place)}: {tName} {score.ToString("N2")} PTS";
}

private string GetPlaceName(int place)
{
    switch (place)
    {
        case 1: return "FIRST PLACE";
        case 2: return "SECOND PLACE";
        case 3: return "THIRD PLACE";
        case 4: return "Recent Score";
        default: return "UNKNOWN PLACE";
    }
}



    
    
}
