using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalDialog : MonoBehaviour {
	
	public GameObject textBox;
    public Text text;
    //public PlayerAttackScript player;
    public int line = 0;
    public int endAtLine = 0; //the last line of text.  If the button is pressed here, move to another scene.
    
    public string[] ScriptLines = 
    {
		//(You beat the game))

		"Parasite: I think that's all of them."	//1

		,"Villager: Thank you so much. We no longer have to worry about being enslaved anymore"	//2

		,"Parasite: Well my work here is done."	//3

		,"Villager: Wait you're not coming back to the village?"	//3

		,"Parasite: No I think I'll just roam around. You know get to know the world, make friends, infect people, discover new things... You know... Just explore around." //4

		,"Villager: What's that about infecting people?"	//5

        ,"Parasite: Nothing, don't worry about it. (Maybe I should infect him, whatever it's not like I'll see him anymore). Good job player. Now you can continue with your life. Or play this without ever getting detected."

        ,"Villager: What are you talking about?"

        , "Parasite: Nothing. Roll credits."

	};

    // Use this for initialization
    void Start()
    {
        endAtLine = ScriptLines.Length;	//5
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