using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIUpdater : BasicObject {

    new void Start () {
        base.Start();
        HealthUI = GameObject.Find("Health UI");
        StealthOverlay = GameObject.Find("Stealth Overlay");
	}

    // updates health UI on screen to given number
    public static void UpdateHealthText(int currentHealth)
    {
        HealthUI.GetComponent<Text>().text = "Health: " + currentHealth;
    }

    // change overlay color based on given detection
    public static void UpdateStealthOverlay(Enemy.BehaviorState behaviorState)
    {
        switch (behaviorState) {
            case Enemy.BehaviorState.Alerted:
            case Enemy.BehaviorState.Restless:
        StealthOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                break;
            case Enemy.BehaviorState.Passive:
            default:
        StealthOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 90);
                break;
    }
    }

    /** References to UI objects for one-time, fast access **/
    /** Set references in editor **/

    private static GameObject HealthUI;
    private static GameObject StealthOverlay;
}
