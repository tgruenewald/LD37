using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class readFrom : MonoBehaviour {

	public Text text;

	public TextAsset textFile;
	string [] storyLines;

	void Start()
	{
		if (textFile != null)
		{
			storyLines = textFile.text.Split ('\n');
		}

		for (int i = 0; i<storyLines.Length; i++)
		{
			Debug.Log (storyLines [i]);
		}

	}

	/*
	void Start () {

		StartCoroutine(Load ("Assets\\Story\\DFIUStory.txt"));
	}

	IEnumerator waitForClick()
	{
		while (!Input.GetButtonDown ("Fire1")) {
			
			yield return null;
		}
			
	}

	public IEnumerator Load(string fileName)
	{
			string line;
			StreamReader theReader = new StreamReader(fileName,Encoding.Default);
			do{
				line = theReader.ReadLine();
				if(line != null)
				{
					string[] entries;
				entries.Add(line);
				}


			} while (line!= null);

			theReader.Close();

			Debug.Log (entries [3]);
	}
*/
}
