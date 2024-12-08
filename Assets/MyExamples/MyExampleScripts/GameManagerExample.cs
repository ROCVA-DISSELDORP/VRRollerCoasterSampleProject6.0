using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEample : MonoBehaviour
{
    public int playerScore = 0;
    public float timeRunning = 0;
    public string playerName = "Player 1";
    public bool isPaused = false;

    public Vector3 playerStartPosition = new Vector3(0, 0, 0);
    public Vector3 playerScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("The Game Is Starting!!!");

        isPaused = false;

        if (playerStartPosition != Vector3.zero)
        {
            GameObject.Find("VRPlayer").transform.position = playerStartPosition;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("The Game Is Running!!!");

        timeRunning = Time.time;

        if(Input.GetKey(KeyCode.P))
        {
            if (isPaused == true)
            {
                isPaused = false;
                Time.timeScale = 0;
            }
            else if (isPaused == false)
            {
                isPaused = true;
                Time.timeScale = 1;
            }
        }

    }

    public void AddScore()
    {
        // playerScore = playerScore + 100;
        playerScore += 100;
    }
    public void AddScore(int score)
    {
        playerScore += score;
    }
}
