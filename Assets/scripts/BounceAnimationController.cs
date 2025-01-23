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

public class BounceAnimationController : MonoBehaviour
{
    private Animator animator;
    private static readonly int BounceTrigger = Animator.StringToHash("Bounce");
    PlayerScores board;
    void Start()
    {
        animator = GetComponent<Animator>();
        board = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<PlayerScores>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //default value = 0
            float score = 0f;
            if (gameObject.tag == "0.2") {
                score = 0.2f;
            } else {
                score = float.Parse(gameObject.tag);
            }
            
            
            board.AddScore(collision.gameObject.GetComponent<tiktokID>().id, score, collision.gameObject.GetComponent<tiktokID>().name, collision.gameObject.GetComponent<tiktokID>().sprite);
            animator.SetTrigger(BounceTrigger);
            GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
        }
        //We should be emitting an event that contains enough information for another class to handle every collision event (tik tok ids, values, etc.)
        //In that class, we will have a dictionary in the class (the class is only instantiated once, not 16 times like this class)
        //
    }
}
