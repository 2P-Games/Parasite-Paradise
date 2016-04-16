public GameObject textBox;
    public Text text;
    //public PlayerAttackScript player;
    public int line = 0;
    public int endAtLine = 0; //the last line of text.  If the button is pressed here, move to another scene.
    
    public string[] ScriptLines = 
    {
		//(start up in village, Creator talks to you)

		"Creator: Ah my latest creation has awaken! How are you feeling buddy?."	//1

		,"Parasite: Gyah..."	//2

		,"Creator: I should probably fill you in on why I created you"	//2

		,"Creator: You see, people from our village have been abducted by the village across the island. I need you to go rescue them and get every single one back no matter the methods you have to use"	//3

		,"Parasite: *Chuckles parasitically*. Bliegh, Fyahhh, Yugioh."	//4

		,"Creator: Alright I'll take that as a yes. (Hmmm, I thought he'd be able to talk). Anyway remember, you have the ability to infect guards and take control of them, try to use this to your advantage. Got it?"	//5
        ,"Parasite: *Nods parasitically*. Trahhh, Mouough, Pikachu." //6

        ,"Creator: !?. You said a word didn't you?" //7

        ,"Parasite: No I didn't." //8

        ,"Creator: ...Ok... Just... don't get detected ok. Listen it's simple, you infect guards, you control guards, you save the villager." //9

        ,"Parasite: How do I infect?" //10

        ,"Creator: You press X" //11

        ,"Parasite: What's X?" //12

        ,"Creator: Err, I mean you use your tail" //13

        ,"Parasite: How do I save the people?." //14

        ,"Creator: Well that's up to the player." //15

        ,"Parasite: Player!?" //16

        ,"Creator: Uhhh." //17

        ,"Developer: Stop breaking the 4th wall and get to saving people." //18

        ,"Creator: He's right, listen. You have to keep in mind that you are very weak in health, so stealth is your friend. Each area has a person of influence get to him or her, and you'll be able to rescue the villager. Ok? Ok." //19

        ,"Parasite: Sure, but first..." //20

        ,"Creator: Yeah?" //21

        ,"Parasite: Can you teach me Thunder bolt?" //22

        ,"Creator: No! Just go do your mission. Jeez." //23

	};

    // Use this for initialization
    void Start()
    {
        endAtLine = ScriptLines.Length;	//23
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