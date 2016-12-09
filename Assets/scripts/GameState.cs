using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class GameState
{
	public static string cursor = "normal";

    public static GameObject player = null;
	public static int playerHP = 100;
    public static GameObject introMusic = null;
    public static string currentLevel = "Scene1";
	public static string currentBattle = "";
	public static string weaponsFileName = null;
	public static string currentWeapon = null;
	public static string npcImageName = null;
	public static string enemyName = null;
	public static bool inBattle = false;
	public static int score = 0;
	public static bool gameOver = false;
	public static int npcHP = 0;

	private static GameObject pc = null;

	public static void makeInventoryButtonsInteractable(bool enable) {
		for (int i = 0; i< Player.MAX_INVENTORY; i++) {
			Button inventorySlot = GameObject.Find ("InventoryButton" + (i+1)).GetComponent<UnityEngine.UI.Button> ();
			inventorySlot.interactable = enable;
		}		
	}

	public static void SetPC(GameObject player){
		GameState.pc = player;
	}

	/*public static GameObject GetPC(){
		if(pc == null){
			pc = GameObject.FindGameObjectWithTag("Player");
			if(pc == null)
				return null;
		}

		return pc.GetComponent<player>();
	}*/
}


