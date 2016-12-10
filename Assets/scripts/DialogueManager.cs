﻿using UnityEngine;
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

	// Use this for initialization
	void Start () {
		dialogue = "";
		characterName = "";
		//inChoice = false;
		parser = GameObject.Find ("Dialogue Parser").GetComponent<DialogueParser> ();
		storyManager = GameObject.Find ("gameManager").GetComponent<storyManager> ();

		lineNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") && !gameManager.inChoice && gameManager.inStory && !inCheck)
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



	public void ShowDialogue(){
		//ResetImages ();
		ParseLine ();
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
		else if (parser.GetKey (lineNum) != "Choice"){
			gameManager.inChoice = false;
			characterName = parser.GetSpeaker (lineNum);
			dialogue = parser.GetContent (lineNum);

			//if dialogue contains commands
			if (dialogue.Contains("~"))
			{
				Debug.Log ("Commands: " + dialogue.Split ('~') [1]);

				gameManager.parseOption (dialogue.Split ('~') [1]);
				dialogue = dialogue.Split ('~') [0];

			}

			else if (dialogue == "over" && !gameManager.inChoice)
			{
				gameManager.ExitAdventure ();
				gameManager.gainingSkill = false;
			}
		//	DisplayImages ();
		}
		else {

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
		dialogueBox.text = dialogue;
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
