using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {

	DialogueManager dialogueManager;
	DialogueParser parser;
	public roomScript roomScript;
	public storyManager storyManager;
	public mapTracker mapTracker;
	public animateText animateText;



	public int week = 1;
	public int turn = 0;
	public int day = 1;

	string command = "";
	public string commandModifier = "";


	public Text weekText;
	public Text storyText;
	public Text shipStatText;

	public bool inStory = false;
	public bool inChoice = false;
	public bool expRound = false;
	public bool gainingSkill = false;
	public bool printingText = false;
	public bool delayedLineCommand = false;

	[System.Serializable]
	public class Player
	{
		public string name;
		public GameObject player;
		public Text statText;

		public int eng = 0;
		public int bot = 0;
		public int med = 0;
		public int cp = 0;

	}



	public Player[] player;

	public void Start()
	{
		dialogueManager = GameObject.Find ("Dialogue Manager").GetComponent<DialogueManager> ();
		parser = GameObject.Find ("Dialogue Parser").GetComponent<DialogueParser> ();

		parser.init ();
		roomScript.startStory (1);

		Text dayDisplayText = GameObject.Find ("DayDisplayText").GetComponent<Text> ();
		dayDisplayText.text = "Day " + day;
		dayDisplayText.CrossFadeAlpha(0, 4f, false);
	}

	public void Update(){
		expRound = true;
		/*if (Input.GetButtonDown ("Fire1")) {
			//Debug.Log ("clicked");
			if (printingText) {
				printingText = false;


			} else if (inStory && !inChoice) {
				nextScreen ();
			
			}
		}//if click*/
	}//if Update

	public void ExitAdventure(){
		
		Debug.Log ("loop back to beginning of nurse");
		dialogueManager.lineNum = parser.SearchStory("startday");
		day++;
		Text dayDisplayText = GameObject.Find ("DayDisplayText").GetComponent<Text> ();
		dayDisplayText.text = "Day " + day;
		dayDisplayText.CrossFadeAlpha(1.0f, 0f, true);
		dayDisplayText.CrossFadeAlpha(0, 4f, false);
//		AdvanceWeek ();
//		inStory = false;
//		expRound = false;
//		weekText.text = "Week: " + week;
//		mapTracker.hidePlanets ();
//		updateStats ();
//		roomScript.leaveRoom ();

	}

/*	void nextScreen(){
		if (expRound) {
			roomScript.gotoPlanet (mapTracker.location);
		}
		else if (gainingSkill)
		{
			gainSkill ();
		}

		else{
			//AdvanceWeek();
			roomScript.leaveRoom ();
		}

	}*/

	public void gainSkill(){
		if (expRound) {
			return;
		} 
		else {
			roomScript.showStory();
			inStory = true;
			gainingSkill = true;
			dialogueManager.UpdateDialogue (false);
			/*switch (roomScript.inRoom) {
			case "bot":
				storyText.text = storyManager.printStory ("gainskill.bot");
				//animateStory("gainskill.bot");
				player [turn].bot++;
				gainingSkill = false;
				break;
			case "med":
				if(animateStory ("gainskill.med")=="over")
				{
					player [turn].med++;
					gainingSkill = false;
					AdvanceTurn();
					roomScript.leaveRoom ();	
				}
						

				break;
			case "cp":
				storyText.text = "You gained 1 computer programming!"; 
				player [turn].cp++;
				gainingSkill = false;
				break;
			case "eng":
				storyText.text = "You gained 1 engineering!"; 
				gainingSkill = false;
				player [turn].eng++;
				break;
			default:
				Debug.Log ("Error: Not in room");
				break;
			}//switch*/

		}//else
	}


	public void AdvanceWeek(){
		Debug.Log ("Week advanced");

		week++;
		if (week % 4 == 0){
			expRound = true;
			weekText.text = "Week: EXPEDITION TIME!";
		}
		else{
			weekText.text = "Week: " + week;
		}

		parser.DisplayFlags ();
		//turn++;
	}
		
	public void updateStats()
	{
		shipStatText.text = "Food: " + storyManager.Stat ["food"] +
		"\nMorale: " + storyManager.Stat ["morale"] +
		"\nScrap Metal: " + storyManager.Stat ["metal"];
	}

	public void animateStory(string text)
	{
		StartCoroutine(animateText.animate(text));
	}

	public void hideChoices(){
		roomScript.hideChoices ();
	}

	public void parseOption(string option)
	{
		if (inStory)
		{
			int i = 0;
			string partOption = "";
			if (option.Contains (":")) {
				partOption = option.Split (':') [i];
			} else
				partOption = option;
			
			command = partOption.Split (',') [0];
			commandModifier = partOption.Split(',')[1];
			Debug.Log ("command is: " + command);
			Debug.Log ("command Modifier is: " + commandModifier);

			if (option.Contains(":"))
			{
				do {

					commandModifier = partOption.Split (',') [1];

					if (command == "SetFlag")
					{
						bool newFlagValue;
						if (partOption.Split(',')[2] == "T")
						{
							newFlagValue = true;
						}
						else
						{
							newFlagValue = false;
						}
							
						parser.Flags[commandModifier] = newFlagValue;

					}
					else{
						runOptionCommand(command,commandModifier);
					}

					
					i++;
					Debug.Log("i increased");
					partOption = option.Split (':') [i];
					command = partOption.Split (',') [0];
					Debug.Log (command);
					Debug.Log (commandModifier);

				} while (command != "line");
				commandModifier = partOption.Split (',') [1];

				if (inChoice)
				{
					runLineCommand (commandModifier);	
				}
			}
			else if (command == "line")
			{
				if (!inChoice)
				{
					delayedLineCommand = true;
					return;
				}
				else
				{
					runLineCommand (commandModifier);
					hideChoices ();
					inChoice = false;
					return;
				}

			}//else if
			else
			{
				runOptionCommand (command, commandModifier);
				return;
			}
			Debug.Log (command);
			Debug.Log (commandModifier);

			hideChoices ();
			inChoice = false;

		}//if gameManager.inStory
		else //normal rooms
		{
			if (option == "talkPlanets")
				Debug.Log("talked about planets");
			if (option == "gainSkill")
				gainSkill ();;
			if (option == "leaveRoom")
				roomScript.leaveRoom ();
		}
	}//parseOptions

	void runOptionCommand(string command, string commandModifier)
	{
		Debug.Log("command: " + command);
		Debug.Log("command modifier: " + commandModifier);

		if (command == "eng")
			player [turn].eng += int.Parse(commandModifier);
		else if (command == "bot")
			player [turn].bot += int.Parse(commandModifier);
		else if (command == "cp") 
			player [turn].cp += int.Parse(commandModifier);
		else if (command == "med")
			player [turn].med += int.Parse(commandModifier);
		else 
			storyManager.changeStat (command, int.Parse(commandModifier));
	}

	public void runLineCommand(string lineCommand)
	{
		int previouslineNum = dialogueManager.lineNum;
		if (lineCommand == "over")
		{
			Debug.Log ("over in runLineCommand");
			ExitAdventure ();
		}		
		else if (lineCommand == "next")
		{
			Debug.Log ("updating dialogue for going to next line");
			dialogueManager.UpdateDialogue (true);
		}
		else
		{
			dialogueManager.lineNum = parser.SearchStory(lineCommand);
			Debug.Log ("updating dialogue after finding specific line by key code");
			dialogueManager.UpdateDialogue (true);
		}

		//Debug.Log ("after running line " + previouslineNum + ", we are at: " + (dialogueManager.lineNum + 1));
	}//runLineCommand

}
