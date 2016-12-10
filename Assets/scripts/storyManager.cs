using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class storyManager : MonoBehaviour {

	public gameManager gameManager;
	public roomScript roomScript;
	DialogueParser parser;

	int textStage = 0;
	int flavorText = 0;
	public int pickedChoice = 0;

	public Dictionary<string, int> Stat = new Dictionary<string, int> ();

	public bool tookGhostNote = false;
	public bool ghostActive = false;

	void Start(){
		parser = GameObject.Find ("Dialogue Parser").GetComponent<DialogueParser> ();

		Stat.Add ("food", 10);
		Stat.Add ("morale", 10);
		Stat.Add ("metal", 10);

		Stat.Add ("cap", 0);
		Stat.Add ("zed", 0);
		Stat.Add ("xola", 0);
		Stat.Add ("yas", 0);
		Stat.Add ("wong", 0);
		Stat.Add ("ngo", 0);

	}

	public void changeStat(string stat, int change)
	{
		Stat [stat] += change;
		gameManager.updateStats ();
	}

	public bool checkStat(string statCheck)
	{
		char oper;
		char oper2;
		string stat;
		int check;

		//if stat check is a flag
		if (statCheck.Contains("."))
		{
			return parser.FlagCheck (statCheck);
		}
		//if the stat check is not a flag
		else
		{
			if (statCheck.Contains(">="))
			{
				oper = '>';
				oper2 = '=';
				stat = statCheck.Split (oper) [0];
				check = int.Parse(statCheck.Split (oper2) [1]);

				int comparison;
				if (stat == "bot" || stat == "med" || stat == "cp" || stat == "eng")
					comparison = getPlayerStat (stat);
				else
					comparison = Stat [stat];

				if (comparison >= check)
					return true;
				else
					return false;
			}
			else if (statCheck.Contains("<="))
			{
				oper = '<';
				oper2 = '=';
				stat = statCheck.Split (oper) [0];

				int comparison;
				if (stat == "bot" || stat == "med" || stat == "cp" || stat == "eng")
					comparison = getPlayerStat (stat);
				else
					comparison = Stat [stat];

				check = int.Parse(statCheck.Split (oper2) [1]);
				if (comparison <= check)
					return true;
				else
					return false;
			}
			else
			{
				if (statCheck.Contains("="))
				{
					oper = '=';
					oper2 = oper;
					stat = statCheck.Split (oper) [0];
					check = int.Parse(statCheck.Split (oper2) [1]);

					int comparison;
					if (stat == "bot" || stat == "med" || stat == "cp" || stat == "eng")
						comparison = getPlayerStat (stat);
					else
						comparison = Stat [stat];

					if (comparison == check)
						return true;
					else
						return false;
				}
				else if (statCheck.Contains(">"))
				{
					oper = '>';
					oper2 = oper;
					stat = statCheck.Split (oper) [0];
					check = int.Parse(statCheck.Split (oper2) [1]);

					int comparison;
					if (stat == "bot" || stat == "med" || stat == "cp" || stat == "eng")
						comparison = getPlayerStat (stat);
					else
						comparison = Stat [stat];

					if (comparison > check)
						return true;
					else
						return false;
				}
				else if (statCheck.Contains("<"))
				{

					oper = '<';
					oper2 = oper;
					stat = statCheck.Split (oper) [0];
					check = int.Parse(statCheck.Split (oper2) [1]);

					int comparison;
					if (stat == "bot" || stat == "med" || stat == "cp" || stat == "eng")
						comparison = getPlayerStat (stat);
					else
						comparison = Stat [stat];

					if (comparison < check)
						return true;
					else
						return false;
				}
				else
				{
					Debug.Log ("Error: No operand recognized for stat check");
					return false;
				}

			}//if it's not <= or >=
		}//else if not a flag

	}

	int getPlayerStat(string stat)
	{
		if (stat == "eng")
			return gameManager.player[gameManager.turn].eng;
		if (stat == "bot")
			return gameManager.player [gameManager.turn].bot;
		if (stat == "cp")
			return gameManager.player [gameManager.turn].cp;
		if (stat == "med")
			return gameManager.player [gameManager.turn].med;
		else
			Debug.Log ("error: not a player stat");
			return 0;
		
	}

	public string printStory(string code)
	{
		gameManager.Player player = gameManager.player[gameManager.turn];
		Debug.Log (code);
		int playerSkill = 0;
		switch (code) 
		{
		case "adventure.default":
				textStage++;
			switch (textStage){
				case 1: 
					return "You're on an adventure!";
				case 2:
					return "Still on an adventure!";
				case 3:
					return "Last stage in the adventure!";
				default:
					textStage = 0;
					return "over";
			}

		case "adventure.water":
				textStage++;
				switch (textStage) {
				case 1:
					changeStat("food", 1);
					return "At Wong's insistence, you land on the surface of water near a patch of purple algae. After some preliminary testing, Wong pronounces it edible, and you cart" +
						" as much as you can carry back onto the shuttle. Gain 1 Food.";
				case 2: 
					return "Captain Clarke eats so much of the algae that he retires to his room, feeling sick. The crew begins to discuss whether to continue skimming the ocean surface or to " +
						"dive into the ocean to see what might be found. Xola says a deep-ocean venture will be risky without the proper equipment, and Wong wants to continue to explore" +
						" on the surface. However, Ngozi says her sensors have picked up what might be sentient life below, and it is the Federation's responsibility to investigate. What do you advocate?";
				case 3: 
					roomScript.choiceText [0].text = "Skim the surface.";
					roomScript.choiceText [1].text = "Dive deep.";
					goChoice();
					return "";
				case 4:
					if (pickedChoice == 1)
					{
						changeStat("ngo",-1);
						return "The crew sets the shuttle to skim just above the surface of the water. You soon come across a strange lilac cloud hovering over the water. " +
							"The shuttle skims closer to the cloud, and it moves to surround the shuttle. You hear a pleasant, soft whirring, and the shuttle's radars all begin to malfunction.";
						
					} 
					else
					{
						changeStat("ngo",1);
						textStage = 3;
						pickedChoice = 1;
						//IMPLEMENT BELOW-THE-SEA LATER??
						return "The shuttle begins the dive below the surface. However, after a few minutes, Xola announces that the shuttle is unprepared for this level of water pressure and " +
							"directs that the shuttle return to to the ruface. Ngozi is disappointed, but safety first.";
					}
				case 5:	
					roomScript.choiceText [0].text = "Comment on the beauty of the pink cloud.";
					roomScript.choiceText [1].text = "Try to fix the radars.";
					goChoice();
					return "";
				case 6:
					if (pickedChoice == 1)
					{
						changeStat("morale", 1);
						textStage = 100;
						return "Indeed, it is very pretty! Everyone cheers up at this remark, and Ngozi even begins snapping selfies of herself by the observation window. Gain 1 morale. The pink cloud" +
								" eventually floats away, and the radars return to normal. You scout out the surface a bit more, then return to the space station to plan the next expedition.";
					}
					else
					{
						if (player.med >= 2)
						{
							return "Using your medical skill, you discover that the pleasant whirring sound is coming from a cloud of microorganisms, and their erratic movements indicate that they are whirring " +
								"because of pain. Many of them are gathered on the side of the shuttle with shade, as though to avoid the sun.";
						}
						else if (player.bot >= 2)
						{
						
							changeStat("food", 3);
							textStage = 100;
							return "After gathering up a sample of the cloud, you find that your shuttle is surrounded by microorganisms that are responsible for the algae growth. You take some with you " +
								"back to the space station to increase food production. Gain 3 Food.";
						}
						else if (player.eng >= 1)
						{
							changeStat("morale", 1);
							textStage = 100;
							return "After a bit of tinkering, you find a way to alter the radar so it only detects large-density objects. Everyone is impressed. Gain 1 morale.";	
						}
						else
						{
							changeStat("xola", 1);
							textStage = 100;
							return "You try your best to repair the radar, but the thingamajig refuses to function! Eventually the cloud dissipates, and you return to the space station, feeling " +
								"rather silly. Xola tells you she appreciated that you tried.";
						}
					}
				case 7:
					roomScript.choiceText [0].text = "Burn the microorganisms.";
					roomScript.choiceText [1].text = "Continue to provide shade.";
					goChoice();
					return "";
				case 8:
					if (pickedChoice == 1)
					{
						changeStat("yas", 3);
						changeStat ("morale", -1);
						textStage = 100;
						return "You enable thrusters all over the shuttle: Starboard! Port! Aft! Bow! The cloud blackens and falls away. Dr. Karadja grins like a demon. Everyone else looks away " +
							"uncomfortably as the shuttle returns to the space station. Lose 1 morale.";
					}
					else
					{
						textStage = 100;
						return "After gliding for a while, the microorganisms become attached to the shuttle, turning the exterior of the shuttle pink. They mess up the radars from time to time, but otherwise " +
							"there is no cause for alarm. Your shuttle returns to the space station with what looks like a mighty new paint upgrade. Turns out these microorganisms somehow can survive " +
							"deep space.";
					}
				default: 
					textStage = 0;
					return "over";
				}//switch

			case "adventure.tree":
				textStage++;
				switch (textStage) {
				case 1:
					return "You enter the planet’s orbit a little quicker than you intended, and a piece melts off " +
						"your thrusters before you land. Your replicator should be able to fix the breakage, but it" +
						" looks like you’ll need to scavenge for some more metal while you’re here.";
				case 2: 
					return "You, Yas, and Wong step out of the ship and look around. The area is covered by a " +
						"dense forest of metal trees, their silver branches dangling with what appear to be the" +
						" organs of animals. Vine-like intestines hang from one tree, while another grows " +
						"smooth, plump livers. The organs give off the fragrant odor of ripened fruit.";
				case 3:
					return "Wong looks a little green, but he starts expounding on the ineffability of nature." +
						" Yas closely examines a tree full of kidneys, and pronounces all of them perfect.";
				case 4: 
					return "You walk a little further and come to a clearing in the woods. A withered silver tree " +
						"stands at the center of the clearing. Its gnarled branches are bare of any organs.";
				case 5:
					roomScript.choiceText [0].text = "Talk to the tree.";
					roomScript.choiceText [1].text = "Climb the tree.";
					goChoice ();
					return "";
				case 6:
					if (pickedChoice  == 1)
					{
						textStage = 10;
						return "Feeling faintly ridiculous, you begin speaking to the tree." +
						"\nCadet: Your branchiness, we are explorers from the Democratic Federation of the " +
						"Intergalactic Union, and we come in peace." +
						"\nOld Tree: Howdy, folks. What can I do you for?";
							
					}
					else
					{
						textStage = 15;
						return "You think it’s about time you put your Star Scouts training to good use. After all, you" +
						" didn’t just earn your badges by selling boxes of Minty Moons! You scamper up the tree, " +
						"and it begins to creak alarmingly. No matter! You plant your feet firmly and continue on" +
						" up.";
					}
				case 11: 
					return "Cadet: Well, to get to the root of the problem, we’re stranded on your planet, " +
					"and we need metal to repair our thrusters. Could we trouble you for a stray branch" +
					" or two?" +
					"\nOld Tree: Beggin’ yer pardon, but I can’t help. Y’see, the rot’s got into my trunk," +
					" and it’s near hollowed me out. Givin’ up one of my branches would all but kill me. ";
				case 12:
					textStage = 20;
					return"Back in the day, I used to grow the most beauteous red hearts for miles around." +
						" Now I’m jest old and lonesome. Oh, but when I was a sapling…"+
						"\nYou quickly make up an excuse, and escape before the tree starts telling" +
						" you any more of its life story.";
				case 16:
					return "At the top of the tree, you take in the view. The forest spreads out around you as far " +
					"as your eyes can see. Besides the trees and the organs, there aren’t any other signs of " +
					"life.";
				case 17:
					return "As you start climbing down, your foot catches on a thin branch, and you hear it snap. " +
					"Apparently, that’s the last straw. The tree begins to speak with a metallic rasp." +
					"\nOld Tree: ENOUGH! Y’all had better leave, now. And NEVER COME BACK!";
				case 18:
					textStage = 20;
					return "Cadet: Whoopsie daisy."+
						"\nThe tree starts to shake and sends you tumbling into the dirt. You wisely decide to " +
						"escape the clearing as fast as you can.";
				case 21: 
					return "You, Yas and Wong regroup by your ship to discuss your situation in a sane and rational manner." +
					"\nYas: Do you realize what a treasure trove we’ve landed on? Kidneys, lungs, spleens, all there for the taking.";
				case 22:

					return "Wong: We took an oath to extend the hand of friendship to all new species we encounter. These trees could share wisdom and secrets " +
					"that far surpass our knowledge, if only we talk to them.";
				case 23:
					return "Yas: You washed-up idiot, these trees are dangerous, and we need the metal. Somehow, I don’t think that they’ll be giving it up " +
					"willingly. And if we only harvested a few of them, think of the lives that could be saved. The experiments that could be performed…";
				case 24:
					return 	"Wong: With you, it’s always about your experiments, isn’t it? Nothing else matters. Well, I’ll defend the nature, if nobody else will!"+
							"\nYas: Wonderful. Well, I call majority vote. Cadet, what’s your decision?";
				case 25:
					roomScript.choiceText [0].text = "Side with Yas and harvest the trees.";
					roomScript.choiceText [1].text = "Side with Wong and talk to the trees.";
					goChoice();
					return "";
				case 26:
					if (pickedChoice == 1) {
						textStage = 30;
						return "The doctor gives you an approving smile. Wong glowers at both of you and stalks back into the ship to wait." +
						"\nYou and Yas look at each other." +
						"\nYas: This will have to be quick. If that old tree was any indication, this forest will not take kindly to strangers." +
							"\nCadet: Got it, Doc.";
						

					} else 
					{
						textStage = 40;
						return "You address the forest, telling the trees about your situation. You humbly request a stray branch or two, so that you can repair your ship." +
						"\nBranches rustle and chime in the wind. Slowly, the branches begin to bend towards the old, barren tree. ";
					}

				case 31:
					return "Experimentally, you pick a fresh stomach from one of the trees. The tree appears to stretch its limbs, and settles back into its original position. " +
					"You look over at Yas and see that she has already gathered a pulsating stomach, three pancreases, and a brain stem.";
				case 32:
					
					return "Remembering that you still need metal, you snap a small branch off of a healthy young tree. It flails its branches " +
					"in shock and whips you in the face. The other trees nearby begin to stir." +
					"\nYas: Get back to the ship. Go, go, go!";
				case 33:
					changeStat ("yas", 2);
					changeStat ("wong", -1);
					changeStat ("metal", 1);
					changeStat ("food", 1);
					changeStat ("morale", -1);
					textStage = 100;	
					return "You don’t need to be told twice. You and Yas race towards the ship. You manage to jam the branch into the replicator and" +
								" fix the thruster just in time to prevent your ship from being overtaken by grasping tree limbs."+
								"\nAs you shoot off into the sky, you think of the fine dinner you’ll be having tonight. Sauteed liver with onions, " +
							"and perhaps some boiled tongue on the side.";
				case 41:
					return "You glance with trepidation at Yasmine and Lee. The voice of the forest rumbles through you."+
						"\nThe Forest: TAKE THAT ONE, IF YOU WISH."+
						"\nOld Tree: What, little old me?"+
						"\nYas and Wong start arguing again. You realize that it’ll be up to you again to make this decision.";
				case 42:
						roomScript.choiceText [0].text = "Cut down the old tree.";
						roomScript.choiceText [1].text = "Uh, no thanks!";
						goChoice();
						return "";
				case 43:
					if (pickedChoice == 1)
					{
						
						textStage = 50;
						return "You walk up to the tree with your laser."+
							"\nOld Tree: Y’all just make it quick, y’hear?"+
							"\nYou slice off two long branches. The old tree writhes in agony and makes a sound like gears grinding " +
						"together.";

					}
					else
					{
						
						textStage = 60;
						return "You edge towards the ship, signaling urgently to Yas and Wong with your eyes." +
						"\nCadet: Thanks, but that’s an offer I’ll have to refuse." +
						"\nOld Tree: Wow, really?" +
						"\nThe soil beneath your feet begins to rumble. An enormous oak full of teeth howls with rage. ";
							
					}
				case 51:
					return	"The sound follows you the whole way back to the ship. The old tree’s metal turns out to be a perfect " +
					"alloy for repairing the thrusters, and you even have more metal left over afterwards.";

				case 52:
					changeStat("wong", -2);
					changeStat("yas", 1);
					changeStat("metal", 2);
					changeStat("morale", -1);
					textStage = 100;
					return "Yas approves of your levelheaded approach to the situation, but Wong refuses to talk to you and Yas " +
						"for the next week. For many nights to come, your dreams are haunted by the sound of grinding gears.";

				case 61:
					changeStat ("wong", 1);
					changeStat ("metal", 1);
					changeStat ("morale", 2);
					textStage = 100;
					return "You snap off a couple of its branches as you race back to the ship."+
						"\nYou manage to jam the branch into the replicator and fix the thruster just in time to prevent your ship " +
						"from being overtaken by grasping tree limbs. As you shoot off into the sky, you hear a lone voice call out to you from down below."+
						"\nOld Tree: Y’all come back now!";
				default:
					textStage = 0;
					return "over";
				}//switch

			case "adventure.war":
				textStage++;
				switch (textStage) {
					case 1:
						//temporary metals??
						changeStat ("metal", 5);
						return "Your shuttle lands just outside of the canyon. With only a few people, everyone has to work " +
							"together to haul the equipment needed. It’s back breaking work and the craters make maneuvering" +
							" dangerous, but by the end of your first day the crew has collected over 50 samples.";
					case 2:
						changeStat("metal",-5);
						return "The crew gathers their samples and equipment, but before they can get back to the shuttle, " +
							"a net falls down over Wong, Ngozi, and Xola. They are whisked away." +
							" The ship has landed right in the middle of a conflict zone between two of the planet’s " +
							"warring groups. The rest of the crew just barely reaches the shuttle and escapes as a hostile " +
							"group of inhabitants gives chase.";
					case 3:
						return "Back on the ship, the Captain makes contact with Federation, reporting the missing crew " +
							"members, but help won’t arrive for weeks because of ... political complications. As usual. " +
							"The captain calls a meeting with the remaining crew to make a difficult choice. Do you wait " +
							"for the Federation to arrive? Or do you vote to take the crew on a rogue mission, completely against guidelines?";
					case 4:
						roomScript.choiceText [0].text = "Wait on the Federation.";
						roomScript.choiceText [1].text = "Rescue the crew.";
						goChoice();
						return "";
					case 5:
						if(pickedChoice == 1)
						{
							textStage = 10;
							return "For the safety of the rest of the crew and the Federation as an organization, you decide " +
								"to let the Federation handle it.";
						}
						else 
						{
							textStage = 20;
							return "With the few remaining crew members, you draw up a plan to rescue your crewmates. " +
							"At night, you hide the shuttle far from the captor’s camp. With finesse and luck, you’re " +
							"able to find their holding cell.";
												
						}
					case 11:
						return "It takes weeks for the Federation to come to the table. The planet’s politics " +
						"are complicated and getting all members to even meet, much less negotiate is difficult. " +
						"There seems to be little hope for compromise, much less a rescue. While negotiations stall," +
						" all missions are suspended and all the ship can do is hope that the captured crew members" +
						" are still alive.";
					case 12:
						return	"The ship has never felt as empty to you as it does now. After a month," +
							" the Federation finally brings news that a successful rescue operation is underway. After" +
							" 17 hours, your crewmates return, thinner and worn out from their experience.";
					case 13:
						changeStat ("morale", -3);
						textStage = 100;	
						return "Later, your crew members find out that you were the vote that persuaded the ship from " +
							"launching a rescue mission. While they say they understand your reasons, relations are now " +
							"cold between you. Morale drops.";

					case 21:
						//flavor text difference between two choices at least?
						roomScript.choiceText [0].text = "Smash the lock with a rock.";
						roomScript.choiceText [1].text = "Pick the lock.";
						goChoice ();
						if (player.eng < 4)
							roomScript.choiceText [1].enabled = false;
						return "";
					case 22: 
						return "Just as you free the crew, a guard spots your actions. In a rather poor excuse for a " +
							"jail break (space hasn’t left you at your physical peak), you manage to escape your " +
							"pursuers after falling into a crevice. Fortunately, you are able to crawl out with whatever dignity you have left" +
							" and navigate back to the shuttle where the rest of the crew had gathered.";
					case 23:
						changeStat ("morale", 3);
						changeStat ("wong", 3);
						changeStat ("ngo", 3);
						changeStat ("xola", 3);
						textStage = 100;
						return "Later, your captured crew members learn that you were the deciding vote for the rescue" +
							" mission and your words inspired everyone to action. They understand that you took a big " +
							"risk and are thankful. Morale drastically increases and your relations with each of the" +
							" rescued crew members increases.";
								
					default:
						textStage = 0;
						return "over";
					}//switch

			case "adventure.ghost":
				textStage++;
				switch (textStage) {
				case 1:
					return "The abandoned ship is dark, the power supply long having run out. Your suits are the only light " +
					"source as you and your crew look around for survivors, but you don’t find anyone.";
				case 2:
					return "The only evidence that one was on the ship are some hastily written notes you find on the main AI" +
						" terminal. You can’t read the dialect.";
				case 3:
					roomScript.choiceText [0].text = "Take the note with you.";
					roomScript.choiceText [1].text = "Leave the note.";
					goChoice();
					return "";
				case 4:
					if (pickedChoice == 1){
						tookGhostNote = true;
						return "You take the notes with you. Maybe Ngozi can take a look at them once you get back to the ship.";
					}
					else {
						return "You leave the note. What do you need scrap paper for?";
					}
				case 5:
					//Does the ghost mission end if you do not take the note??
					if (!tookGhostNote)	
						textStage = 100;
					return "After searching the ship from top to bottom, you can’t find anything that tells you what happened to the" +
							" crew. To everyone’s relief, no bodies are left behind. The Captain says that the crew must have been" +
							" rescued by another ship, but never turned off the distress signal. He turns off the signal and " +
							"orders the crew to return to base.";

				case 6:
					changeStat ("morale", -3);
						return "A few days later after encountering the abandoned ship, some crew members have begun reporting " +
							"signs of illness and fatigue. Dr. Kharadja says the cause is overwork and a common virus making its way" +
							" through the crew. A little rest and simple medication should let the illness pass. Morale drops.";
				case 7:
					return "You figure enough time has passed and Ngozi should be able to tell you something about the " +
						"note. In the computer room you find Ngozi at the computer wearing a blanket and looking rather" +
						" unwell. She says the dialect is from a minor planet that is part of the Federation." +
						" The translation is rather difficult, so Ngozi isn’t sure if she has it right." +
						" The note reads SLEEPS IN WATER.";
				case 8:
					return "Seeing your confused expression, Ngozi apologizes and says the translation is probably off " +
						"since she’s been feeling so tired lately. She says you can throw the note away or show it to " +
						"whoever you think could do something with it.";
				
				case 9:
					roomScript.choiceText [0].text = "Throw the note away.";
					roomScript.choiceText [1].text = "Show the note to the rest of the crew.";
					goChoice();
					return "";
				case 10:
					if (pickedChoice == 1)
					{
						//does morale return to normal? any remarks about people getting beter??
						changeStat("morale", 3);
						textStage = 100;
						return "You throw the note away.";
					}
					else 
					{
						return "You show the note to the crew members. No one really knows what to make of it.";
					}
				case 11:
					changeStat ("morale", -3);
					return "Morale continues to drop as half the crew starts feeling the symptoms. Xola wants your help " +
						"filling orders since she’s not feeling well. You report to the engineering room.";
				case 12:
					return "Xola: Thanks for coming down here. Wong’s complaining about the plants not getting enough water. " +
						"Would you mind dealing with him? I don’t have it in me deal with him right now.";
				case 13: 
					return "You meet Wong in the greenhouse. Some of the plants have turned white for no explainable reason. Wong " +
						"is perplexed by it and shows them to you.";
				case 14:
					return "Wong: It’s all the plants from this pipeline. Those over there are connected to a different water " +
							"source. Do you think you can do anything?";
				case 15: 
					roomScript.choiceText [0].text = "Shut off the water supply.";
					roomScript.choiceText [1].text = "Don't mess with the water supply.";
					goChoice();
					return "";
				case 16:
				if (pickedChoice == 1)
				{
					textStage = 20;
					return "You shut off the water supply. The plants are pretty much dead anyways so what’s the harm?";
				}
				else
				{
					textStage = 30;
					ghostActive = true; //implement decreasing morale for rest of game??
					return "You elect not to touch the pipes. Hey, the academy didn’t teach you anything about " +
						"plumbing and you don’t intend to practice now on your only water supply.";
				}
				case 21:
					textStage = 100;
					changeStat ("morale", 6);
					return "The symptoms the space station has been complaining about start to disappear and morale improves. Wong quarantines " +
						"the white plants and studies their changes. Once she feels better, Xola finally investigates the pipes, shutting " +
						"down the entire water supply for the space station. Inside the pipes, she finds a growing white moss that eerily begins to " +
						"form in the shape of a human hand. She removes the entire segment of pipe and orders a quarantine on the water. " +
						"The space station sets course to the nearest freshwater supply as the space station head back to headquarters.";
				case 31:
					return "The space station's functions break down one by one as crew members become incapacitated and are unable to keep up " +
						"maintenance. The fuel supply runs dangerously low until the station is adrift. All reserve power is being used to " +
						"maintain life support features.";
				case 32: 
					textStage = 100;
					return "Alone you stay at the ship’s computer, the screen dark from the loss of power. As best you can, you scribble " +
						"notes on all the paper you can find. In detail, you record everything about what is happening on the ship. " +
						"You’ve set up a distress beacon but know that the message won’t be received by home base until two months later because of the " +
						"delay. The ship’s reserve power can only maintain life support feature for only a few more days. You write and then wait.";
				default:
					textStage = 0;
					return "over";
				}//switch



			case "gainskill.med":
				playerSkill = gameManager.player [gameManager.turn].med;
				Debug.Log (playerSkill);
				if (playerSkill < 3) {
					if (flavorText != 1 && flavorText != 2) {
						Debug.Log ("changing flavor text");
						flavorText = Random.Range (1, 3);
					}//if first time, pick a flavor
					textStage++;
					if (flavorText == 1) {
						switch (textStage) {
						case 1: 
							return "You enter the Medical Bay for your monthly check-up to find Ngozi gasping while holding" +
							"\nher throat. Dr. Karadja is nowhere in sight. ";
						case 2: 
							return "Quick! You perform the Heimlich maneuver! Ngozi spits out a chicken bone.";
						case 3:
							return "Dr. Karadja appears from behind a curtain and congratulates you on passing your first medical exam.";
						case 4:
							return "You gain 1 Medical.";
						default:
							textStage = 0;
							flavorText = 0;
							return "over";
						}//switch
					}//option 1
					if (flavorText == 2) {
						switch (textStage) {
						case 1:
							return "Dr. Karadja teaches you how to draw blood. You spend the entire week practicing on yourself.";
						case 2: 
							return "Your right arm becomes bruised and bloody, but your left arm looks good.";
						case 3: 
							return "Guess you're not ambidextrous, after all.";
						case 4:
							return "You gain 1 Medical.";
						default: 
							flavorText = 0;
							textStage = 0;
							return "over";
						}
					}//option 2
					else
						return "Error: no flavorText for medical";
				}//beginner skill
				else if (playerSkill < 6) {
					if (flavorText != 1 && flavorText != 2) {
						Debug.Log ("changing flavor text");
						flavorText = Random.Range (1, 3);
					}// if first time, pick flavor
					textStage++;
					if (flavorText == 1) {
						switch (textStage) {
						case 1: 
							return "You shadow Dr. Karadja and record patient visits. As your knowledge of medical shorthand" +
							"\ngrows, your handwriting deterioriates from a neat cursive to the ugly scribbles of a toddler.";
						case 2:
							return "You gain 1 Medical.";
						default:
							textStage = 0;
							flavorText = 0;
							return "over";
						}//switch
					}//option 1
					if (flavorText == 2) {
						switch (textStage) {
						case 1:
							return "Xola comes in, complaining of a stiff knee. Dr. Karadja informs Xola that amputation may be necessary," +
							"\nand takes out the saw.";
						case 2:
							return "You laughingly assure Xola that Dr. Karadja is only joking.";
						case 3: 
							return "Great bedside manner!";
						case 4:
							return "You gain 1 Medical.";
						default: 
							flavorText = 0;
							textStage = 0;
							return "over";
						}
					}//option 2
					else
						return "Error: no flavorText for medical";
				}//intermediate skill
				else if (playerSkill < 9) {
					if (flavorText != 1 && flavorText != 2) {
						Debug.Log ("changing flavor text");
						flavorText = Random.Range (1, 3);
					}//if first time, choose flavor
					textStage++;
					if (flavorText == 1) {
						switch (textStage) {
						case 1: 
							return "You accidentally freeze off your middle finger while emptying the garbage.";
						case 2: 
							return "No fear: just Space BandAid that baby up.";
						case 3:
							return "You gain 1 Medical.";
						default:
							textStage = 0;
							flavorText = 0;
							return "over";
						}//switch
					}//option one
					if (flavorText == 2) {
						switch (textStage) {
						case 1:
							return "Zed comes in for a root canal. You skillfully shoot the anesthesia into Zed's arm," +
							"\nrendering them unconscious.";
						case 2: 
							return "Dr. Karadja says that you only needed to numb Zed's gums, but she applauds your decision nonetheless.";
						case 3: 
							return "You gain 1 Medical.";
						default: 
							flavorText = 0;
							textStage = 0;
							return "over";
						}
					}//option 2
					else
						return "Error: no flavorText for medical";
				}//expert skill
				else if (playerSkill > 8) {
					if (flavorText != 1 && flavorText != 2) {
						Debug.Log ("changing flavor text");
						flavorText = Random.Range (1, 3);
					}//if no flavor, choose flavor
					textStage++;
					if (flavorText == 1) {
						switch (textStage) {
						case 1: 
							return "The Captain comes in, complaining of nightmares and insomnia. You give him the perfect cure by cryogenically" +
							"\nfreezing him for a wonderful night of dreamless sleep.";
						case 2: 
							return "You even unfreeze him in the morning.";
						case 3:
							return "You gain 1 Medical.";
						default:
							textStage = 0;
							flavorText = 0;
							return "over";
						}//switch
					}//option 1
					if (flavorText == 2) {
						switch (textStage) {
						case 1:
							return "Wong comes in for his monthly check-up and begins arguing with you that skill with the scalpel doesn't" +
							"\ntranslate into spiritual awareness.";
						case 2: 
							return "You hit all of his chi points and paralyze him for the next hour. You win!";
						case 3: 
							return "You gain 1 Medical.";
						default: 
							flavorText = 0;
							textStage = 0;
							return "over";
						}
					}//option 2
					else
						return "Error: no flavorText for medical";
				}//insane skill
				else
					return "Error: no string code for this skill level";
			
			case "gainskill.bot":
				playerSkill = gameManager.player [gameManager.turn].bot;
				Debug.Log (playerSkill);

				if (playerSkill < 3) {
					if (Random.Range (1, 3) == 1)
						return "You scrubbed the undersides of plant leaves all week. You gain 1 botany!";
					else
						return "You added worms to different pots and watched them wriggle. You gain 1 botany!";
				} else if (playerSkill < 5) {

					if (Random.Range (1, 3) == 1)
						return "You expertly clip all the leaves off of a single plant, leaving only bare stems. You gain 1 botany.";
					else
						return "You drown only half of the plants when you water them this week! You gain 1 botany!";

				} else if (playerSkill < 7) {
						
					if (Random.Range (1, 3) == 1)
						return "You accidentally breed a new species of plant and immediately name it after your favorite holo-show " +
						"\ncharacter, much to Wong's dismay. You gain 1 botany.";
					else
						return "You discover a plant that is resistant to fire. You gain 1 botany! When you tell Wong " +
						"\nof your discovery, he says he already knew about it.";
				} else if (playerSkill < 9) {
						
					if (Random.Range (1, 3) == 1)
						return "You are now trusted with an entire greenhouse to do with as you will. You decide to devote most of it" +
						"\n to whole-grain vegetables. You gain 1 botany.";
					else
						return "You grow a plant that is shaped exactly like Wong's left elbow. He is elated, but probably" +
						"\nconfused as to why you know his left elbow so ... intimately. You gain 1 botany.";
				} else
					return "Error: no string code for botany";
			default:
				Debug.Log ("Error: no story code");
				return "Error: no string code";
			
		}//switch types of text
	}//printStory

	void goChoice()
	{
		gameManager.inChoice = true;
		roomScript.showChoice (3);	
	}
}//storyManager