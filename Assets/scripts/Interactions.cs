using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interactions : MonoBehaviour {

	public Image textbg;
	public Image textBorder;
	public Image talkingHead;

	public Sprite radioImage;
	public Sprite pcImage;

	public Text textWithImage;
	public Text storytext;
	public bool dialogueComplete = false;
	public bool inDialogue = false;
	public bool printingText = false;

	public string dialogueTag = "";

	private float fadeInTime = 0.025F;
	private bool keyPressed = false;

	public int textStage = 0;
	// Use this for initialization
	private static Interactions interactions = null; 

	void Start () {
		//showText ("Here is some text.");
	}
	void Awake() {
		DontDestroyOnLoad (gameObject);
		if (interactions == null) {
			interactions = this;
		} else {
			DestroyObject(gameObject);
		}
	}

	// Update is called once per frame
	public void Update(){
		if (Input.GetButtonDown ("Fire1")||Input.GetButtonDown("Jump")) {
			//Debug.Log ("clicked");

			if (printingText)
			{
				printingText = false;
			}
			else if (dialogueComplete&&!printingText) {

				//Debug.Log ("hiding text");
				hideText ();
				inDialogue = false;
				dialogueComplete = false;

			} 			
			else if(inDialogue&&!printingText)
			{
				showText(dialogueTag);
			}
		}//if click
	}//if Update


	public void showText(string tag){
		dialogueTag = tag;
		//Debug.Log ("showing text: " + dialoguetext);
		inDialogue = true;

		if(textManager (tag)==true){
			talkingHead.enabled = true;
		}
		textBorder.enabled = true;
		textbg.enabled = true;

		//StartCoroutine (animate (textManager(tag)));

	}

	public void hideText (){
		textBorder.enabled = false;
		talkingHead.enabled = false;
		textWithImage.enabled = false;
		textbg.enabled = false;
		storytext.enabled = false;
	}

	public IEnumerator animate(string strComplete, string speaker){
		Text text;
		if (speaker == "radio"){
			text = textWithImage;
			talkingHead.sprite = radioImage;
		}
		else if (speaker == "pc"){
			text = textWithImage;
			talkingHead.sprite = pcImage;
		}
		else{
			text = storytext;
		}

		printingText = true;
		int i = 0;
		text.text = "";
		text.enabled = true;
		while( i < strComplete.Length && printingText){
			text.text += strComplete[i++];
			yield return new WaitForSeconds(fadeInTime);
		}
		text.text = strComplete;
		printingText = false;


	}

	public bool textManager(string tag)
	{
		textStage++;
		switch (tag) {

		case "lady":
			switch (textStage) {
			case 1:
				StartCoroutine (animate ("Captain Arugua?", "pc"));
				return false;
			case 2:
				StartCoroutine (animate ("Finally.", "radio"));
				return false;
			case 3:
				StartCoroutine (animate ("What's going on? I was just attacked by a microwave!", "pc"));
				dialogueComplete = true;
				textStage = 0;
				return false;		
			}
			return false;
//			StartCoroutine (animate ("I am a lady talking to you.", ""));
//			dialogueComplete = true;
//			textStage = 0;
//			return false;

		case "radio1":
			StartCoroutine (animate ("This is Captain Arugua. Can anyone hear me? Over.", "radio"));
			dialogueComplete = true;
			textStage = 0;
			return true;

		case "radio2":
			StartCoroutine (animate ("Someone pick up this radio. Over.", "radio"));
			dialogueComplete = true;
			textStage = 0;
			return true;

		case "radio3":
			switch (textStage) {
				case 1:
				StartCoroutine (animate ("Captain Arugua?", "pc"));
					return true;
				case 2:
				StartCoroutine (animate ("Finally.", "radio"));
					return true;
				case 3:
						StartCoroutine (animate ("What's going on? I was just attacked by a microwave!", "pc"));
					return true;

				case 4:
				StartCoroutine (animate ("Something's gone wrong with the tech. Not just at HQ, but throughout the city. Over.", "radio"));
					return true;

				case 5:
				StartCoroutine (animate ("I hope it's not aliens again. Or Chthulu.", "pc"));
					return true;

				case 6:
				StartCoroutine (animate ("Whatever it is, we've triangulated the source of the problem at the National Aquarium. Over.", "radio"));
					return true;

				case 7:
				StartCoroutine (animate ("I'm on my way.", "pc"));
					return true;

				case 8:
				StartCoroutine (animate ("Get to the rooftop. We're sending in a helicopter to airlift you to the drop point. Over.", "radio"));
					return true;

				case 9:
				StartCoroutine (animate ("Wait, was I supposed to have been saying \"Over\" this whole time?", "pc"));
					return true;

			case 10:
				StartCoroutine (animate ("*distinct sound of facepalm* Just move it.", "radio"));
				dialogueComplete = true;
				textStage = 0;
					return true;
				default:
					return true;


			}//radio3 switch

		case "radio6":
			switch(textStage)
			{
			case 1:
				StartCoroutine (animate ("Where's everyone else?", "pc"));
				return true;
			case 2:
				StartCoroutine (animate ("They're trapped in the elevator.", "radio"));
				return true;
			case 3:
				StartCoroutine (animate ("Great. I'll wait for them. What's the ETA on the helicopter?", "pc"));
				return true;
			case 4: 
				StartCoroutine (animate ("Should be arriving ... soon. Ish.", "radio"));
				return true;
			case 5:
				StartCoroutine (animate ("Did you just say \"soonish\"?", "pc"));
				return true;
			case 6:
				StartCoroutine (animate ("I said soon. Over.", "radio"));
				return true;
			case 7:
				StartCoroutine (animate ("Right ...", "pc"));
				dialogueComplete = true;
				textStage = 0;
				return true;
			default: 
				return true;
			}//radio6
		case "radio11":
			switch (textStage)
			{
			case 1: 
				StartCoroutine (animate ("So my GPS isn't working ...", "pc"));
				return true;
			case 2:
				StartCoroutine (animate ("And?", "radio"));
				return true;
			case 3:
				StartCoroutine (animate ("How do I get to the aquarium?", "pc"));
				return true;
			case 4:
				StartCoroutine (animate ("You've never been?", "radio"));
				return true;
			case 5:
				StartCoroutine (animate ("Sure. Once. On a field trip.", "pc"));
				return true;
			case 6:
				StartCoroutine (animate ("It's on the corner of Portland and Denver.", "radio"));
				return true;
			case 7:
				StartCoroutine (animate ("... and where are those streets?", "pc"));
				return true;
			case 8:
				StartCoroutine (animate ("You don't know *static* unbelievable! *static* If you went down the fire escape, you're on Portland right now.", "radio"));
				return true;
			case 9: 
				StartCoroutine (animate ("So ... Denver is ... which way?", "pc"));
				return true;
			case 10:
				StartCoroutine (animate ("*static*", "radio"));
				return true;
			case 11:
				StartCoroutine (animate ("Hello? Over?", "pc"));
				return true;
			case 12:
				StartCoroutine (animate ("*static*", "radio"));
				return true;
			case 13:
				StartCoroutine (animate ("Looks like I'm on my own.", "pc"));
				dialogueComplete = true;
				textStage = 0;
				return true;
			default: 
				return true;
			}//radio11

		default:
			StartCoroutine (animate ("textManager did not find string", ""));
			return false;

		}//tagswitch
	}//textManager

/*	public IEnumerator WaitInput(string text, Text textbox) {
		keyPressed = false;
		while(!keyPressed)
		{
			//Debug.Log ("waiting for input in coroutine");
			if ((Input.GetButtonDown ("Fire1")||Input.GetButtonDown("Jump"))&& !printingText) {
				keyPressed = true;
				break;
			}
			yield return 0;
		}
		StartCoroutine (animate (text,textbox));
		dialogueComplete = true;
		inDialogue = false;
	}*/
}

