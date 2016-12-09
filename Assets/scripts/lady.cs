using UnityEngine;
using System.Collections;

public class lady : MonoBehaviour {
	private CursorMode cursorMode = CursorMode.Auto;
	public Renderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {
		Cursor.SetCursor (null, Vector2.zero, cursorMode);		
		GameState.cursor = "normal";
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;

	}

	void OnMouseEnter() {
		if (GameState.cursor != "normal")
			rend.material.color = Color.yellow;
	}

	void OnMouseExit() {
		if (GameState.cursor != "normal")
			rend.material.color = Color.white;
	}






}
