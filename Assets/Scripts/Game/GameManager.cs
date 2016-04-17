using UnityEngine;
using System.Collections;

public class GameManager : BasicObject
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        /** Game Pausing and Unpausing **/
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
            } else
            {
                Time.timeScale = 1;
                isPaused = false;
            }

            UIUpdater.TogglePauseOverlay();
        }
    }


    // is the game paused
    private bool isPaused = false;

}
