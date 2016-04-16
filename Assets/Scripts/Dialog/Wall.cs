using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wall1Dialog : MonoBehaviour {	
	public GameObject textBox;
    public Text text;
    //public PlayerAttackScript player;
    public int line = 0;
    public int endAtLine = 0; //the last line of text.  If the button is pressed here, move to another scene.
    
    public string[] ScriptLines = 
    {
		//(see's someone guarding the wall, but has the colors of you village)

		"Parasite: Hey, wait a sec... Are you from the village from the east?"	//1

		,"Guard: Yeah... How'd you know?"	//2

		,"Parasite: I can tell... Why are you guarding the wall of this village?"	//3

		,"Guard: I don't want to. I am forced because of my sister they've captured and kept her at the tower. I have to work for them or else they'll kill her."	//3

		,"(Press f to pay respects)" //4

		,"Guard: She is not dead! I know she's alive"	//5

        ,"Parasite: Ok, listen I'll save her don't worry. But if I do just let me past this wall, ok?" //6

        ,"Guard: Yeah, of course. Either way I'll go back to my village. It's been a while." //7

        ,"Parasite: Alright then I'll get going." //8

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