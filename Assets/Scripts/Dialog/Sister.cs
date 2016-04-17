using UnityEngine;
using UnityEngine.UI;

public class SisterDialog : MonoBehaviour {
	public GameObject textBox;
    public Text text;
    //public PlayerAttackScript player;
    public int line = 0;
    public int endAtLine = 0; //the last line of text.  If the button is pressed here, move to another scene.
    
    public string[] ScriptLines = 
    {
		//(Guard's sister is locked and you saved her)

		"Parasite: You ok?"	//1

		,"Girl: Yeah... Thanks by the way."	//2

		,"Parasite: Yeah, your brother told me to rescue you so I can pass the wall."	//3

		,"Girl: My brother? Is he safe, can I see him?"	//3

		,"Parasite: See him at the village. I'm gonna go tell him I saved you so he can let me through." //4

		,"Girl: Wait. Here show him this locket. That way he'll believe you. Just tell him to bring it back. That way I'll know he'll try to be careful."	//5

        ,"Parasite: Ok, alright. You be careful as well." //6

        ,"Girl: You be safe too" //7

        ,"Parasite: Safe? No I don't do safe." //8

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