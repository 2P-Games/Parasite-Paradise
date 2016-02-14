using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapManager : MonoBehaviour {

    // generic function for opening a scene
	public void OpenLevelByString(string levelName)
    {
        // main menu
        if(levelName.Equals("mainmenu", System.StringComparison.InvariantCultureIgnoreCase))
        {
            SceneManager.LoadScene("MainMenu");
        }
        // test scene
        else if(levelName.Equals("testscene", System.StringComparison.InvariantCultureIgnoreCase))
        {
            SceneManager.LoadScene("TestScene");
        }
    }

    // close the game
    public void CloseGame()
    {
        Application.Quit();
    }
}
