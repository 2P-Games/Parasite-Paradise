using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIUpdater : BasicObject {

    new void Start () {
        base.Start();
        HealthUI = GameObject.Find("Health UI");
	}

    public static void UpdateHealthText(int currentHealth)
    {
        HealthUI.GetComponent<Text>().text = "Health: " + currentHealth;
    }

    /** References to UI objects for one-time, fast access **/
    /** Set references in editor **/

    private static GameObject HealthUI;

}
