using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	public MenuCam menuCam;
	public GameObject StartGameButton;
	public GameObject QuitGameButton;
	public GameObject ReturnToMenuButton;
	private UIScreen currentScreen;

	// Use this for initialization
	void Start () {
		currentScreen = UIScreen.MainMenu;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentScreen == UIScreen.MainMenu) {
			StartGameButton.SetActive (true);
			QuitGameButton.SetActive (true);
			ReturnToMenuButton.SetActive (false);
		} else if (currentScreen == UIScreen.Options) {
			StartGameButton.SetActive (false);
			QuitGameButton.SetActive (false);
			ReturnToMenuButton.SetActive (true);
		}
	
	}

	public void OptionsButtonClicked(){
		if (menuCam) {
			menuCam.MenuScreen ();
			currentScreen = UIScreen.MainMenu;
		}
		
	}
	public void MenuButtonClicked(){
		if (menuCam) 
		{
			menuCam.MenuScreen();
			currentScreen = UIScreen.MainMenu;
		}
	}
	public void NewGameButtonClicked()
	{
		Application.LoadLevel ("Scene2");
	}
	public enum UIScreen
	{
		MainMenu, Options
	}
}
