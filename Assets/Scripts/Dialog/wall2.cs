using UnityEngine;
using UnityEngine.UI;

public class Wall2Dialog : MonoBehaviour {

    public GameObject textBox;
    public Text text;
    //public PlayerAttackScript player;
    public int line = 0;
    public int endAtLine = 0; //the last line of text.  If the button is pressed here, move to another scene.
    
    public string[] ScriptLines = 
    {
		//(Meet the guard again)

		"Guard: Hey! It's you!"	//1

		,"Parasite: Yup, rescued your sister by the way."	//2

		,"Guard: You did? Really?"	//3

		,"Parasite: Yup, here she gave me this locket so I can give it to you as some sort of good luck charm. She said to be careful"	//3

		,"Guard: Thank you so much. I'll take my leave now." //4

		,"Parasite: Now. To take care of final business behind these walls."	//5

	};

    // Use this for initialization
    void Start()
    {
        endAtLine = ScriptLines.Length;	//8
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ScriptLines[line];

        if (Input.GetKeyDown(KeyCode.A))
        {
            line++;
        }
        
        if (line > endAtLine)
        {
            //Finish and move on to scene.
            line--;
        }  
    }
}