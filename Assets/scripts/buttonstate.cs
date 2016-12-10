using UnityEngine;
using System.Collections;

public class buttonstate : MonoBehaviour {
	public Vector2 hotSpot = Vector2.zero;
	private CursorMode cursorMode = CursorMode.Auto;
	// Use this for initialization
	void Start () {

        // TODO: use this for initialization
		//Debug.Log("loading inventorycell");
		GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.Load<Sprite>("sprites/inventorycell");
    }
    public void hover()
    {
        Debug.Log("hover");
        GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.Load<Sprite>("Sprites/broken_mug");
    }
    public void unhover()
    {
        Debug.Log("unhover");
        GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.Load<Sprite>("Sprites/mug");
    }

    public void click()
    {
		Debug.Log ("you clicked");
        //GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.Load<Sprite>("Sprites/sword");
		UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button> ();
		Debug.Log ("You selected the " + button.tag);
		GameState.currentWeapon = button.tag;
		if (button.tag == "hotdog") {
			GameState.playerHP = GameState.playerHP + 40;
			if (GameState.playerHP > 100) {
				GameState.playerHP = 100;
			}			
		}
			
		// these are consumable
		if (button.tag == "hotdog" || button.tag == "grenade" || button.tag == "spear" || button.tag == "rock") {
			button.tag = "available";
			button.image.sprite = Resources.Load<Sprite>("Sprites/inventorycell");

		}

		if (!GameState.inBattle) {
			switch (button.tag) {
			case "sword":
				Cursor.SetCursor (Resources.Load ("sword") as Texture2D, hotSpot, cursorMode);				
				GameState.cursor = "sword";
				break;
			}			
			button.tag = "available";
			button.image.sprite = Resources.Load<Sprite>("Sprites/inventorycell");			


		}


    }
    public void setButtonImage()
    {
        GetComponent<UnityEngine.UI.Button>().image.sprite = Resources.Load<Sprite>("Sprites/phone");
    }
	

}
