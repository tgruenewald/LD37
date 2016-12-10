using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

public class DialogueParser : MonoBehaviour {

	//TextAsset textfile;

	public struct DialogueLine{
		public string key;
		public string content;
		public string speaker;
		public string[] options;

		public DialogueLine(string Key, string Content, string Speaker){
			key = Key;
			content = Content;
			speaker = Speaker;
			options = new string[0];
		}
	}

	public List<DialogueLine> lines;
	public List<DialogueLine> skillLines;
	public List<DialogueLine> roomLines;

	public List<DialogueLine> activeList;

	public Dictionary<string, bool> Flags = new Dictionary<string, bool> ();


	void Start () {
		
//		string file = "Assets/TextAssets/ludum37.txt";
//		lines = new List<DialogueLine> ();
//
//		LoadDialogue (file, lines);
//
//		activeList = lines;
//
//		PrintAllContent(lines);
//		Debug.Log (FlagCheck ("giraffe.sold"));


	}

	public void init() {
		string file = "Assets/TextAssets/ludum37.txt";
		lines = new List<DialogueLine> ();

		LoadDialogue (file, lines);

		activeList = lines;

		PrintAllContent(lines);
		Debug.Log (FlagCheck ("giraffe.sold"));		
	}

	void Update () {
	
	}

	void LoadDialogue(string textfile, List<DialogueLine> list){
		string line;
		StreamReader r = new StreamReader (textfile);

		using (r){
			int l = 1;
			do{
				line = r.ReadLine();
				if (line!= null){
					string[] lineData = line.Split ('|');

					//if it's a roomline
					if (lineData[0].Contains("."))
					{
						if (lineData[0].Split('.')[0] == "englab")
						{
							list = roomLines;
						}
					}

					if (list == roomLines)
					{
						Debug.Log("room line: " + lineData[1]);
					}

					//normal parsing
					if (lineData[0] == "Choice")
					{
						DialogueLine lineEntry = new DialogueLine(lineData[0], "", "");
						lineEntry.options = new string[lineData.Length-1];
						for (int i = 1; i < lineData.Length; i++)
						{
							CreateDictionary(lineData[i]);
							lineEntry.options [i-1] = lineData[i];
						}
						list.Add(lineEntry);
					}
//					else if (lineData[1] == "Check")
//					{
//						DialogueLine lineEntry = new DialogueLine(lineData[0],lineData[1],"");
//						lineEntry.options = new string[lineData.Length-1];
//						for (int i = 2; i < lineData.Length; i++)
//						{
//							lineEntry.options[i-2] = lineData[i];
//						}
//						list.Add(lineEntry);
//					}
					else{
						if (lineData.Length != (3))
						{
							Debug.Log("error cannot parse at  line: " + l + ", which is: " + line);
						}
						else
						{
							CreateDictionary(lineData[1]);
							DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], lineData[2]);
							list.Add(lineEntry);
						}
					}

					if (lineData[1] == "over")
					{
						activeList = lines;
					}



				}
				l++;
			} while (line!= null);
		} 
		r.Close();
	}

	public void CreateDictionary (string line)
	{
		if (line.Contains("SetFlag"))
		{
			//Debug.Log ("found line with SetFlag: " + line);
			string newFlag = "";
			string commands = line.Split('~')[1];
			if (commands.Contains(":"))
			{
				string[] command = commands.Split(':');
				for (int i = 0; i < command.Length; i++)
				{
					//Debug.Log ("here's one isolated command: " + command [i].Split (',') [0]);
					if (command[i].Split(',')[0] == "SetFlag")
					{
						newFlag = command[i].Split(',')[1];
						if (FlagExists (newFlag))
							return;
						else
							Flags.Add(newFlag, false);
						//Debug.Log("adding flag as false: " +  newFlag);
					}
					else
					{
						//Debug.Log("flag not found in: " + command[i]);
					}
				}
			}
			else
			{
				newFlag = commands.Split(',')[1];
				if (FlagExists (newFlag))
					return;
				else
					Flags.Add(newFlag, false);
				//Debug.Log("adding this only flag as false: " +  newFlag);
			}
		}
	}//Create Dictionary

	public int SearchStory(string code)
	{
		for (int i = 0; i<activeList.Count;i++)
		{
			if (activeList[i].key == code)
			{
				return i;
			}
		}
		Debug.Log ("searched text asset for key " + code + ", key not found");
		return -1;

	}

	public string GetKey(int lineNumber){
		if (lineNumber <activeList.Count){
			return activeList [lineNumber].key;
		}
		return "";
	}

	public string GetSpeaker (int lineNumber){
		if (lineNumber < activeList.Count){
			return activeList[lineNumber].speaker;
		}
		return "";
	}

	public string GetContent(int lineNumber){
		if (lineNumber < activeList.Count){
			return activeList [lineNumber].content;
		}
		return "";
	}

	public string[] GetOptions (int lineNumber){
		if (lineNumber < activeList.Count){
			return activeList [lineNumber].options;
		}
		return new string[0];
	}

	public bool FlagExists(string flagString)
	{
		foreach (KeyValuePair<string, bool> temp in Flags) 
		{
			if (temp.Key==flagString)
				return true;
			else
				return false;
		}
		return false;
	}//flagexists

	public bool FlagCheck(string statcheck)
	{
		string flag = statcheck.Split ('=') [0];
		string checkString = statcheck.Split ('=') [1];
		bool check;

		if (checkString == "T")
			check = true;
		else if (checkString == "F")
			check = false;
		else {
			check = false;
			Debug.Log ("Error: " + statcheck + " does not contain true or false");
		}

		if (Flags [flag] == check) {
			return true;
		} else {
			return false;
		}
			
	}//Flag Check

	public void DisplayFlags()
	{
		foreach (KeyValuePair<string, bool> temp in Flags) 
		{
			Debug.Log (temp.Key + ": " + temp.Value);
		}
	}//Display Flags	

	public void PrintAllContent(List<DialogueLine> list)
	{
		for (int i = 0; i < list.Count;i++)
		{
			Debug.Log(list[i].content);
		}
	}//PrintAllContent



}
