using UnityEngine;
using System.Collections;

public class InventoryDisplay : MonoBehaviour {
	private static InventoryDisplay inventoryDisplay = null; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Awake() {
		DontDestroyOnLoad (gameObject);
		if (inventoryDisplay == null) {
			inventoryDisplay = this;
		} else {
			DestroyObject(gameObject);
		}
	}
}
