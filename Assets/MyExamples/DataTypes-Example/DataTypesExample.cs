using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTypesExample : MonoBehaviour
{
    public bool isPaused = false;
    public bool gameover = false;
    public int score = 0;
    public int health = 100;
    public int age = 18;
    public float speed = 1;
    public float playTime = 0;
    public string playerName = "Player 1";
    public string firstName = "Tim";
    public string lastName = "Coster";
    public string[] playerNames = new string[5];

    public Vector3 spawnPosition = new Vector3 (0, 0, 0);
    public Vector2 spawnPosition2D = new Vector2(0,0);

    public Color playerColor = Color.white;

    public AnimationCurve vehicleAccelerationCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
