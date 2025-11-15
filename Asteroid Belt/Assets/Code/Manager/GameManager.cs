using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Countdown,  // like starting a race, 3, 2, 1 GO!
        Playing,    // enable PlayerController, cursor locked outside of UI to game
        Paused,     // unlock Cursor, view UI, freeze time &|| input
        GameOver    // show results, add money, disable shooting and spawning
    }

    //[Header("References")]
    //PlayerController player;
    //Camera mainCam;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
