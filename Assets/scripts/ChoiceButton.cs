using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

	public string option;

	gameManager gameManager;
	DialogueParser parser;
	DialogueManager dialogueManager;
	storyManager storyManager;
	roomScript roomScript;
	AudioSource audioHighlight;
	AudioSource audioSelect;

	// Use this for initialization
	void Start () {
		//dialogueManager = GameObject.Find ("Dialogue Manager").GetComponent<DialogueManager> ();
		//parser = GameObject.Find ("Dialogue Parser").GetComponent<DialogueParser> ();
		gameManager = GameObject.Find ("gameManager").GetComponent<gameManager> ();
		//storyManager = GameObject.Find ("gameManager").GetComponent<storyManager> ();
		//roomScript = GameObject.Find ("Rooms").GetComponent<roomScript> ();
		audioSelect = GameObject.Find ("ui_select").GetComponent<AudioSource>();
		audioHighlight = GameObject.Find ("ui_highlight").GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText (string newText){

		this.GetComponentInChildren<Text>().text = newText;
	}

	public void SetOption(string newOption){
		this.option = newOption;
		Debug.Log("setting option: " + this.option);
	}

	public void ParseOptions(){
		Debug.Log ("parsing option: " + option);

		gameManager.parseOption (option);

	}//parseOptions
	public void OnPointerEnter( PointerEventData ped ) {
		Debug.Log ("=============================hover");
		audioHighlight.Play ();

	}

	public void OnPointerDown( PointerEventData ped ) {
		Debug.Log ("=============================CLICK");
		audioSelect.Play ();
	}    
}
