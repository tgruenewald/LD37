using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

	DialogueParser parser;
	storyManager storyManager;
	public gameManager gameManager;
	//public mapTracker mapTracker;
	public roomScript roomScript;
	public Button[] button;

	public string dialogue, characterName;
	public int lineNum;
	string[] options;
	string[] checks;
	//public bool inChoice;
	List <Button> buttons = new List <Button>();

	public Text dialogueBox;
	public Text nameBox;
	public GameObject choiceBox;

	bool firstScreen = true;
	public bool inCheck = false;
	bool debugging = false;
	public bool gameOver = false;

	private string[] visitors = new string[] {"penguin", "mouse", "nurse", "police"};
	private string[] giraffes = new string[] {"giraffe", "weregiraffe"};

	// Use this for initialization
	void Start () {
		dialogue = "";
		characterName = "";
		//inChoice = false;
		parser = GameObject.Find ("Dialogue Parser").GetComponent<DialogueParser> ();
		storyManager = GameObject.Find ("gameManager").GetComponent<storyManager> ();

		lineNum = 0;
	}
	IEnumerator wait_a_bit(){
		yield return new WaitForSeconds (1f);
	}
	public void run_all_lines() {
		debugging = true;
		while (!gameOver) {
			Debug.Log ("running all lines");
			if (gameManager.delayedLineCommand) {
				Debug.Log ("releasing delayed line command");
				gameManager.runLineCommand (gameManager.commandModifier);
				gameManager.delayedLineCommand = false;

			} else {
				Debug.Log ("updating dialogue after click");
				UpdateDialogue (true);
			}	
			if (lineNum > 140) {
				Debug.Log ("at line 200");
				break;
			}
			Debug.Log ("waiting");
			StartCoroutine (wait_a_bit());
			Debug.Log ("done waiting");
		}
		Debug.Log ("compile success");
	}
	
	// Update is called once per frame
	void Update () {
		if (debugging)
			return;
		if ((Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0)) && !gameManager.inChoice && gameManager.inStory && !inCheck)
		{
			if (gameManager.delayedLineCommand)
			{
				Debug.Log ("releasing delayed line command");
				gameManager.runLineCommand (gameManager.commandModifier);
				gameManager.delayedLineCommand = false;

			}
			else
			{
				Debug.Log ("updating dialogue after click");
				UpdateDialogue (true);
			}
		}

	}

	private void check_for_window_visitor() {
		Image giraffeImage = GameObject.Find ("giraffe").GetComponent<Image> ();
		bool showSomeone = false;
		foreach (var visitor in giraffes) {
			bool showVisitor = false;
			parser.Flags.TryGetValue (visitor+".appears", out showVisitor);
			if (showVisitor) { 
				Debug.Log (visitor + " appears");

				giraffeImage.sprite = Resources.Load<Sprite> ("Sprites/" + visitor);
				giraffeImage.enabled = true;
				showSomeone = true;
				break;
			}

		}
		if (!showSomeone) {
			giraffeImage.enabled = false;
		}		
	}

	private void check_for_visitor() {
		Image visitorImage = GameObject.Find ("visitor").GetComponent<Image> ();
		bool showSomeone = false;
		foreach (var visitor in visitors) {
			bool showVisitor = false;
			parser.Flags.TryGetValue (visitor+".appears", out showVisitor);
			if (showVisitor) { 
				Debug.Log (visitor + " appears");
				visitorImage.sprite = Resources.Load<Sprite> ("Sprites/" + visitor);
				visitorImage.enabled = true;
				showSomeone = true;
				break;
			}

		}
		if (!showSomeone) {
			visitorImage.enabled = false;
		}


	}

	public void ShowDialogue(){
		//ResetImages ();
		ParseLine ();

		// check flags
		check_for_visitor();
		check_for_window_visitor ();
	}

	void ResetImages(){
		if (characterName != ""){
			GameObject character = GameObject.Find (characterName);
			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer> ();
			currSprite.sprite = null;
		}
	}//Reset Images

	void ParseLine(){
		Debug.Log ("Parsing line: " + (lineNum + 1));
		//Determine which list
		if (gameManager.gainingSkill)
		{

			parser.activeList = parser.skillLines;
			if (parser.GetKey(lineNum) != "" && !firstScreen)
			{
				gameOver = true;
				gameManager.ExitAdventure ();
				firstScreen = true;
				return;
			}
			firstScreen = false;
		}
		else
		{
			parser.activeList = parser.lines;
		}

		Debug.Log ("we are at line " + lineNum + " with this content: " + parser.GetContent (lineNum));
		if (parser.GetContent(lineNum) == "Check")
		{
			Debug.Log ("In check");
			inCheck = true;
			checks = parser.GetOptions (lineNum);
			Debug.Log ("LINE NUM: " + lineNum);
			for (int i = 0; i< options.Length; i++)
			{
				Debug.Log ("LINE NUM: " + lineNum + "and i=" + i);
				string entireCheck = checks [i];
				Debug.Log ("Entire check: " + entireCheck);

				if (entireCheck.Contains("~"))//if there is a check
				{
					string allChecks = entireCheck.Split ('~') [0];
					string lineCommand = entireCheck.Split ('~') [1];

					if (entireCheck.Contains("*"))					//if there are multiple checks
					{
						bool pickThisLine = true;
						string[] statcheck = (allChecks.Split ('*'));
						for (int j = 0; j < statcheck.Length; j++)
						{
							if (!storyManager.checkStat (statcheck [j]))
							{
								pickThisLine = false;

							}

						}

						if (pickThisLine)
						{
							gameManager.runLineCommand (lineCommand);
							inCheck = false;
							lineNum--;
							return;
						}
					}//if multiple checks
					else
					{
						Debug.Log (allChecks + " is: " + storyManager.checkStat (allChecks));
						if (storyManager.checkStat(allChecks))
						{
							gameManager.runLineCommand(lineCommand);
							lineNum--;
							inCheck = false;
							return;
						}

					}

				}//if there is a single check

				else 
				{
					gameManager.runLineCommand (entireCheck);
					lineNum--;
					inCheck = false;
					return;
				}
			}
		}
		else if (parser.GetKey (lineNum) != "Choice" && parser.GetKey (lineNum) != "endChoice"){
			gameManager.inChoice = false;
			characterName = parser.GetSpeaker (lineNum);
			var text = parser.GetContent (lineNum);
			Debug.Log ("In not choice");
			//if dialogue contains commands
			if (text == "over" && !gameManager.inChoice)
			{
				Debug.Log ("over called in DM");
				gameManager.ExitAdventure ();
				gameManager.gainingSkill = false;
			}
			dialogue = text;
			if (dialogue.Contains("~"))
			{
				Debug.Log ("Commands: " + dialogue.Split ('~') [1]);

				gameManager.parseOption (dialogue.Split ('~') [1]);
				dialogue = dialogue.Split ('~') [0];

			}


		//	DisplayImages ();
		}
		else {
			Debug.Log ("In else so assume choice: " + lineNum);
			gameManager.inChoice = true;
			characterName = "";
			dialogue = "";
			options = parser.GetOptions (lineNum);
			CreateButtons ();
		}
	}

	void DisplayImages(){
		if (characterName != "")
		{
			//GameObject character = GameObject.Find (characterName);
			//SetSpritePositions(character);
//			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
			//currSprite.sprite = character.GetComponent<Character>().charactersPoses[pose];
		}
	}

	void CreateButtons(){
		if (options.Length > 4)
			Debug.Log ("ERROR: MORE THAN FOUR OPTIONS");
		int b = 0;
		for (int i = 0; i < options.Length; i++)
		{
			ChoiceButton cb = button[b].GetComponent<ChoiceButton> ();
			string buttonText = options [i].Split ('~') [0];
			//Debug.Log (buttonText);


			if (buttonText.Contains("*"))
			{
				bool showButton = true;
				string allChecks = buttonText.Split ('~') [0];


				//string statCheck = allChecks.Split ('*') [0];

				string[] statCheck = allChecks.Split('*');

				for (int j = 0; j < statCheck.Length; j++)
				{
					if (storyManager.checkStat(statCheck[j]))
					{
						//Debug.Log (statCheck[j] + "true");
					} else 
					{
						//Debug.Log(statCheck[j] + "false");	
						showButton = false;
					}

				}

				if (showButton)
				{
					roomScript.choiceText [b].text = options[i].Split ('~') [1];
					roomScript.choiceText [b].enabled = true;
					cb.option = options [i].Split ('~') [2];
					b++;
				}//if button passes all statchecks

					
			}
			else
			{
				roomScript.choiceText [b].text = options [i].Split ('~') [0]; 
				roomScript.choiceText [b].enabled = true;
				//Debug.Log (options [i]);
				cb.SetOption(options [i].Split ('~') [1]);
				b++;
			}

			//Debug.Log (cb.option);
			//Debug.Log ("Button created");
		}

	}//CreateButtons

	void UpdateUI(){
		//Debug.Log ("updating UI: " + dialogue);
		if (!gameManager.inChoice){
			ClearButtons();
			//Debug.Log ("Buttons cleared");
		}
		if (dialogue != "over")
			dialogueBox.text = dialogue;
		else
			dialogueBox.text = "The next day";
		//gameManager.animateStory (dialogue);
		nameBox.text = characterName;
	}//UpdateUI

	void ClearButtons(){
		for (int i = 0; i < buttons.Count; i++){
			Button b = buttons[i];
			buttons.Remove(b);
			Destroy(b.gameObject);
		}
	}
	public void UpdateDialogue(bool lineIncrease)
	{
		ShowDialogue ();
		if (lineIncrease)
			lineNum++;
		if (gameManager.inStory)
		{
			UpdateUI();
		}
	}	
}
