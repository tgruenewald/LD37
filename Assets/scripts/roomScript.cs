using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class roomScript : MonoBehaviour {

	public Image blackbg;
	public Image illustration;

	public Text[] choiceText;
	public Button[] button;

	public Text storyText;
	public Image characterArt;
	public Text characterName;

	public gameManager gameManager;
	public storyManager storyManager;
	public DialogueManager DialogueManager;
	public DialogueParser parser;

	public string inRoom = "";

	// Use this for initialization
	void Start () {
		Debug.Log ("getting room background");
		illustration = GameObject.Find ("illustration").GetComponent<Image> ();	
		illustration.sprite = Resources.Load<Sprite>("Sprites/medievalStructure_07");
		illustration.enabled = true;
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	public void gotoDeck(){
		blackbg.enabled = true;
		choiceText [2].text = "Leave the room.";
		ChoiceButton cb = button [2].GetComponent<ChoiceButton> ();
		cb.option = "leaveRoom";
		choiceText [2].enabled = true;
		if(gameManager.expRound == true){
			
		}
			

	}
		
	public void showStory(){
		for (int i = 0; i < choiceText.Length; i++){
			choiceText[i].enabled = false;
		}
	
		storyText.enabled = true;
	}

	public void showChoice(int numOfChoices){
		storyText.enabled = false;
		for (int i = 0; i < numOfChoices; i++){
			choiceText[i].enabled = true;
		}

	}
	public void hideChoices(){
		for (int i = 0; i < choiceText.Length; i++){
			choiceText[i].enabled = false;
		}

	}

	//sometimes choice2
	public void gotoRoom (string room)
	{
		ChoiceButton cb;
		inRoom = room;
		blackbg.enabled = true;
		if (gameManager.expRound == true) {
			choiceText [0].text = "Talk bout dem planets.";
			choiceText [1].text = "No time to increase skills. Expedite!";
			choiceText [2].text = "Leave the room.";
			cb = button [2].GetComponent<ChoiceButton> ();
			cb.option = "leaveRoom";
		} 
		else {
			choiceText [0].text = "Talk bout dem planets.";
			choiceText [1].text = "Gain a skill.";
			cb = button [1].GetComponent<ChoiceButton> ();
			cb.option = "gainSkill";
			choiceText [2].text = "Leave the room.";
			cb = button [2].GetComponent<ChoiceButton> ();
			cb.option = "leaveRoom";
		}
		showChoice (3);
		illustration.enabled = true;

	}

	public void leaveRoom()
	{
		blackbg.enabled = false;
		hideChoices();
		gameManager.inStory = false;
		storyText.enabled = false;
		characterArt.enabled = false;
		characterName.enabled = false;
		//illustration.enabled = false;
		inRoom = "";
	}

	public void enableCharacterArt(){
		characterArt.enabled = true;
		characterName.enabled = true;
	}

	public void startStory(int destination)
	{
		
//		if (gameManager.expRound) {
//			if (System.Array.IndexOf (mapTracker.planet [mapTracker.location].accessiblePlanets, destination) != -1 || destination == mapTracker.location) {
//				hideChoices();
//				if (mapTracker.planet [destination].visited == false) {
//					mapTracker.travelNewPlanet (destination);

					gameManager.inStory = true;

					blackbg.enabled = true;
					illustration.enabled = true;
					enableCharacterArt ();
					storyText.enabled = true;

					//string text = "";
					if (destination == 1) {
						DialogueManager.lineNum = parser.SearchStory("storyStart");


						//text = gameManager.animateStory ("adventure.default");
					} else if (destination == 2) {
						//parser.activeList = parser.roomLines;
						//DialogueManager.lineNum = parser.SearchStory ("englab.ngo");
						//text = gameManager.animateStory ("adventure.tree");
					} else if (destination == 3) {
					//	text = gameManager.animateStory ("adventure.war");
					} else if (destination == 4) {
					//	text = gameManager.animateStory ("adventure.ghost");
					}
					Debug.Log ("updating dialogue at start of adventure");
					DialogueManager.UpdateDialogue(true); 
						
//				}//if planet visit is incomplete
//
//			}//if planet is accessible??
//		}//if in expedition mode

	}//gotoPlanet
		

	public void choice(int choice)
	{
		if (gameManager.inChoice)
		{
			storyManager.pickedChoice = choice;
			gameManager.inChoice = false;
			storyText.enabled = true;
		}
		else if (choice == 2)
		{
			gameManager.gainSkill ();
		}
		else
		{
			storyText.enabled = false;
			leaveRoom ();
		}
	}

}
