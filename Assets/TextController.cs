using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
	PrisonEscape v1.0
	Author: Peter Gambell.
*/

public class TextController : MonoBehaviour {
	
	public Text textbox;
	
	private enum Locations {
		CELL_START, CELL_BED, CELL_DOOR, TALLY_WALL, SACK, DOOR_OPEN, CORRIDOR_CENTRE, ESCAPE_DOOR, OTHER_CELLS,
		HEAR_GUARDS_0, HEAR_GUARDS_1, SCARE_GUARDS, SCARE_DEATH, GUARD_ROOM, CHARGE_DEATH, GUARDS_RETURN, DAGGER_GUARDS,
		MONITOR_ROOM_0, MONITOR_ROOM_1, INSULT_DEATH, TOOTHBRUSH_DEATH, SHEET_DEATH, WRITE_ARM, MAN_DEATH, RESET, FREEDOM
	};
	
	private Locations currentLocation;
	
	private enum ActionInGuardRoom {
		TOOK_CELL_KEYS, TOOK_GOLD_KEY, TRIED_COMBO_LOCK, OPENED_COMBO_LOCK
	};
	
	private ActionInGuardRoom lastActionInGuardRoom;
	
	// Items
	private bool hasToothbrush;
	private bool hasSheet;
	private bool hasCellKeys;
	private bool hasGoldKey;
	private bool hasDagger;
	
	// Guard variables
	private bool overheardOnce;
	private bool overheardScared;
	private bool scaredGuards;
	private int numTurnsInGuardRoom;
	private const int NUM_TURNS_TILL_GUARDS_RETURN = 3;
	
	// Starting variables
	private int numTallyMarks = 26;
	private bool numberOnArm = false;
	
	// Use this for initialization
	void Start() {
		currentLocation = Locations.CELL_START;
	}
	
	// Update is called once per frame
	void Update() {
		
		switch (currentLocation) {
			case Locations.CELL_START: 
				CellStart();
				break;
			case Locations.CELL_BED:
				CellBed();
				break;
			case Locations.TALLY_WALL:
				TallyWall();
				break;
			case Locations.CELL_DOOR:
				CellDoor();
				break;
			case Locations.SACK:
				Sack();
				break;
			case Locations.DOOR_OPEN:
				DoorOpen();
				break;
			case Locations.CORRIDOR_CENTRE:
				CorridorCentre();
				break;
			case Locations.ESCAPE_DOOR:
				EscapeDoor();
				break;
			case Locations.OTHER_CELLS:
				OtherCells();
				break;
			case Locations.HEAR_GUARDS_0:
				HearGuards0();
				break;
			case Locations.HEAR_GUARDS_1:
				HearGuards1();
				break;
			case Locations.SCARE_GUARDS:
				ScareGuards();
				break;
			case Locations.SCARE_DEATH:
				ScareDeath();
				break;
			case Locations.CHARGE_DEATH:
				ChargeDeath();
				break;
			case Locations.GUARD_ROOM:
				GuardRoom();
				break;
			case Locations.GUARDS_RETURN:
				GuardsReturn();
				break;
			case Locations.DAGGER_GUARDS:
				DaggerGuards();
				break;
			case Locations.MONITOR_ROOM_0:
				MonitorRoom0();
				break;
			case Locations.MONITOR_ROOM_1:
				MonitorRoom1();
				break;
			case Locations.INSULT_DEATH:
				InsultDeath();
				break;
			case Locations.TOOTHBRUSH_DEATH:
				ToothbrushDeath();
				break;
			case Locations.SHEET_DEATH:
				SheetDeath();
				break;
			case Locations.WRITE_ARM:
				WriteArm();
				break;
			case Locations.MAN_DEATH:
				ManDeath();
				break;
			case Locations.RESET:
				Reset();
				break;
			case Locations.FREEDOM:
				Freedom();
				break;
		}
		
	}
	
	void CellStart() {
		// Reset items
		hasToothbrush = false;
		hasSheet = false;
		hasCellKeys = false;
		hasGoldKey = false;
		hasDagger = false;
		
		// Reset guards
		overheardOnce = false;
		overheardScared = false;
		scaredGuards = false;
		numTurnsInGuardRoom = 0;
		
		string wallText =	"You awake and find yourself in a grimy prison cell, " +
							"the walls of grey bricks glaring oppresively down at you. " +
							"You struggle to remember how you got there, but your mind recalls nothing.\n\n";
		
		if (numberOnArm) {
			wallText += "As your arm brushes against your body, you feel a sensation of pain. " +
						"Looking at your forearm, you notice the number \'2461\' has been scratched into it. " +
						"Why is that there?\n\n";
		}
		
		wallText +=	"Getting up from your hard, itchy bed, you see a barred cell door to your left, " +
					"a wall with " + numTallyMarks + " tally marks scratched into it ahead of you, " +
					"and a small sack containing your belongings lying to your right.\n\n" +
					"Press D to inspect the Door, T to add another Tally mark to the wall, or B to check your Belongings";
						
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.D)) {
			currentLocation = Locations.CELL_DOOR;
		} else if (Input.GetKeyDown(KeyCode.T)) {
			currentLocation = Locations.TALLY_WALL;
		} else if (Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.SACK;
		}
	}
	
	void CellBed() {
		string wallText = "You are in a prison cell, sitting on a hard bed";
		
		if (hasToothbrush) {
			wallText += ", holding a toothbrush";
		}
		
		wallText += ". You see a barred cell door to your left, " +
					"a wall with " + numTallyMarks + " tally marks scratched into it ahead of you, ";
		
		if (hasToothbrush) {
			wallText += "and a small sack containing nothing lying to your right.\n\n" +
						"Press D to inspect the Door, or T to add another Tally mark to the wall";
		} else {
			wallText += "and a small sack containing your belongings lying to your right.\n\n" +
						"Press D to inspect the Door, T to add another Tally mark to the wall, or B to check your Belongings";
		}
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.D)) {
			currentLocation = Locations.CELL_DOOR;
		} else if (Input.GetKeyDown(KeyCode.T)) {
			if (hasToothbrush) {
				numTallyMarks++;
			}
			currentLocation = Locations.TALLY_WALL;
			
		} else if (!hasToothbrush && Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.SACK;
		}
	}
	
	void Sack() {
		textbox.text = 	"You open your sack of belongings and find that it contains only a single toothbrush.\n\n" +
						"The end of the toothbrush is pointed, it appears to have been worn down by repeated " + 
						"rubbing against something.\n\n" +
						"Press T to Take the toothbrush, or R to Return it to the sack on the floor.";
		
		if (Input.GetKeyDown(KeyCode.T)) {
			hasToothbrush = true;
			currentLocation = Locations.CELL_BED;
		} else if (Input.GetKeyDown(KeyCode.R)) {
			currentLocation = Locations.CELL_BED;
		}
	}
	
	void TallyWall() {
		if (hasToothbrush) {
			textbox.text = 	"Utilising the sharpened edge of the toothbrush from your sack, " +
							"you scratch another tally mark into the wall.\n\n" +
							"The number of tally marks is now " + numTallyMarks + ".\n\n" +
							"Press T to add another Tally mark to the wall, or B to return to your Bed.";
		} else {
			textbox.text = "Try as you might, you just can't get your fingernail to add another tally mark to the wall. " +
							"How did you add the others?\n\nThe total number of tally marks remains at " + numTallyMarks + ".\n\n" +
							"Press B to return to your Bed.";
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.CELL_BED;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.T)) {
			numTallyMarks++;
			currentLocation = Locations.TALLY_WALL;
		}
	}
	
	void CellDoor() {
		string wallText =	"You inspect the cell door and grip the bars with your hands. Pulling against them, you can tell " +
							"that they are solidly connected and sadly have no need for maintainence in the near future.\n\n" + 
							"Feeling where the key hole is on the other side of the door, you determine that it shouldn't be ";
		
		if (hasToothbrush) {
			wallText += "too difficult to pick using your filed down toothbrush!\n\n" +
						"Press P to Pick the lock, or B to return to your Bed.";
		} else {
			wallText += "too difficult to pick, if only you had something to pick it with!\n\n" +
						"Press B to return to your Bed.";
		}
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.CELL_BED;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.P)) {
			currentLocation = Locations.DOOR_OPEN;
		}
	}
	
	void DoorOpen() {
		textbox.text = 	"After a few minutes of working the end of your toothbrush in the lock, you hear a click, " + 
						"then turn the lock clockwise. You push the cell door open. So far so good!\n\n" +
						"You venture out into the corridor, which stretches out left and right from your cell door. " +
						"Helpfully, there is a sign labelled \"North\" pointing left.\n\n" + 
						"You notice several other barred cell doors nearby, some of which are open. " +
						"Strangely, none seem to contain any prisoners.\n\n" + 
						"Press I to Investigate the open cells, N to head North down the corridor, or S to head South.";
		
		if (Input.GetKeyDown(KeyCode.I)) {
			currentLocation = Locations.OTHER_CELLS;
		} else if (Input.GetKeyDown(KeyCode.N)) {
			currentLocation = Locations.ESCAPE_DOOR;
		} else if (Input.GetKeyDown(KeyCode.S)) {
			currentLocation = Locations.HEAR_GUARDS_0;
		}
	}
	
	void CorridorCentre() {
		string wallText = 	"You are in the middle of a corridor, next to your cell door. " + 
							"The corridor stretches out north and south from your cell door, and " +
							"there are several other barred cell doors nearby, some of which are open. " +
							"Strangely, none seem to contain any prisoners.\n\n";
		
		if (!hasSheet) {
			wallText += "Press I to Investigate the open cells, N to head North down the corridor, or S to head South.";
		} else if (!hasCellKeys) {
			wallText += "Press I to Investigate the open cells again, N to head North down the corridor, or S to head South.";
		} else {
			wallText += "The ring of small keys you have seems to have a key for each cell here, you could try searching the locked cells, " +
						"to see if you come up with anything useful.\n\nPress I to Investigate the closed cells, " +
						"N to head North down the corridor, or S to head South.";
		}
		
		textbox.text = 	wallText;			
		
		if (Input.GetKeyDown(KeyCode.I)) {
			currentLocation = Locations.OTHER_CELLS;
		} else if (Input.GetKeyDown(KeyCode.N)) {
			currentLocation = Locations.ESCAPE_DOOR;
		} else if (Input.GetKeyDown(KeyCode.S)) {
			if (scaredGuards) {
				currentLocation = Locations.HEAR_GUARDS_1;
			} else {
				currentLocation = Locations.HEAR_GUARDS_0;
			}
		}
	}
	
	void EscapeDoor() {
		string wallText = 	"After some time moving north down the gloomy corridor, you see a steel door at the end. " + 
							"As you get closer, you make out the words \"ESCAPE\" painted in white on the door. " +
							"Can it really be this easy?\n\n";
		
		if (hasGoldKey) {
			wallText += "You examine the lock and compare it to the golden key you acquired. " +
						"It looks like the key should open the door.\n\n" + 
						"Press G to use the Golden key in the lock, or " +
						"B to go Back down the corridor to your cell door.";
		} else if (hasCellKeys) {
			wallText += "You examine the lock and compare it to the keys you have taken. " +
						"Unfortunately, it doesn't look like any of these keys match the lock. " + 
						"You try them just to be sure, but none of them work.\n\n" +
						"Press B to go Back down the corridor to your cell door.";
		} else {
			wallText += "You examine the lock and sadly conclude that it isn't. This lock will be much harder to crack, " +
						"you doubt you could pick it if you had a whole day to do it.\n\n" + 
						"\'If I'm going to open this door, I'll need the key\', you think to yourself.\n\n" +
						"Press B to go Back down the corridor to your cell door.";
		}
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.CORRIDOR_CENTRE;
		} else if (hasGoldKey && Input.GetKeyDown(KeyCode.G)) {
			currentLocation = Locations.MONITOR_ROOM_0;
		}
	}
	
	void OtherCells() {
		string wallText = "";
		
		if (!hasSheet) {
			wallText = 	"You walk along the corridor searching only the open cells. " +
						"Since time is short, it's not worth spending it to pick the locks of the closed cells.\n\n" + 
						"After several minutes of searching, the best item you've found is a clean white bedsheet. Hooray.\n\n" +
						"Press T to Take the bedsheet, or R to Return to your cell door empty handed.";
		} else if (!hasCellKeys) {
			wallText = 	"You search the open cells again, but come up with nothing extra.\n\n" +
						"Press R to Return to your cell door empty handed.";
		} else {
			wallText = 	"Using the ring of keys you acquired, you open the locked cells and spend some time searching them. " +
						"Unfortunately, your search yields no useful items.\n\n" +
						"Press R to Return to your cell door empty handed.";
		}
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.R)) {
			currentLocation = Locations.CORRIDOR_CENTRE;
		} else if (Input.GetKeyDown(KeyCode.T)) {
			hasSheet = true;
			currentLocation = Locations.CORRIDOR_CENTRE;
		} 
	}
	
	void HearGuards0() {
		string wallText = "";
		
		if (overheardOnce) {
			wallText +=	"You head south, and start to hear sounds coming from the end of the corridor again. " +
						"'I guess the guards are still there' you think to yourself.\n\n" +
						"You sneak up to the corner at the end of the corridor and carefully peek around it. " +
						"You see two guards in chainmail wearing swords at their hips. " +
						"You pull your head back out of sight, and overhear a bit of their conversation:\n\n" + 
						"\"As I said before, there are no ghosts in this prison. Pull yourself together Edwin!\"\n" +
						"\"I just 'erd a rumour tha's all.\"\n\n";
		} else {
			wallText +=	"You head south, and at some point notice sounds coming from the end of the corridor. " +
						"As you get closer, the sounds slowly become two seperate voices. " +
						"You discover that the corridor ends in a corner, and the voices are emanating from there.\n\n" +
						"You sneak up to the corner and carefully peek around it. You see two guards in chainmail wearing swords at their hips. " +
						"You pull your head back out of sight, and overhear a bit of their conversation:\n\n" + 
						"\"There are no ghosts in this prison. Pull yourself together Edwin!\"\n" +
						"\"I just 'erd a rumour tha's all.\"\n\n";
		}
		
		if (hasSheet) {
			wallText += "Press S to put the white Sheet over your head and attempt to scare the guards away by impersonating a ghost.\n";
		} else {
			wallText += "Hmmm, if you can somehow look like a ghost, you might have a chance of scaring them off.\n";
		}
		
		wallText += "Press C to Charge the guards and attempt to stab them with your toothbrush.\n" +
					"Press R to Retreat down the corridor back to your cell door.\n";
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.R)) {
			overheardOnce = true;
			currentLocation = Locations.CORRIDOR_CENTRE;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.C)) {
			currentLocation = Locations.CHARGE_DEATH;
		} else if (hasSheet && Input.GetKeyDown(KeyCode.S)) {
			currentLocation = Locations.SCARE_GUARDS;
		}
	}
	
	void HearGuards1() {
		string wallText =	"You walk to the corner at the end of the corridor and press your body against the wall. " +
							"You overhear the two guards around the corner.\n\n";
							
		if (overheardScared) {
			wallText += "\"You don't fink dat ghost will come back, do you?\"\n" +
						"\"For the last time, no! It probably wasn't even a ghost in the first place.\"\n\n";
		} else {
			wallText += "\"You don't fink dat ghost will come back, do you?\"\n" +
						"\"Of course not! It probably wasn't even a ghost in the first place.\"\n\n";
		}
		
		wallText += "Press S to put the white Sheet over your head and attempt to scare the guards away by impersonating a ghost.\n";
							
		if (hasDagger) {
			wallText += "Press C to Charge the guards and attempt to stab them with your dagger.\n";
		} else {
			wallText += "Press C to Charge the guards and attempt to stab them with your toothbrush.\n";
		}		  
		
		wallText += "Press R to Retreat down the corridor back to your cell door.\n";
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.R)) {
			overheardScared = true;
			currentLocation = Locations.CORRIDOR_CENTRE;
		} else if (hasDagger && Input.GetKeyDown(KeyCode.C)) {
			currentLocation = Locations.DAGGER_GUARDS;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.C)) {
			currentLocation = Locations.CHARGE_DEATH;
		} else if (hasSheet && Input.GetKeyDown(KeyCode.S)) {
			currentLocation = Locations.SCARE_DEATH;
		}
	}
	
	void ChargeDeath() {
		textbox.text = 	"You heroically charge at the nearest of the two guards and manage to stab him ineffectually, " +
						"before you are cut down by his partner.\n\nPress P to Play again.";
		
		if (Input.GetKeyDown(KeyCode.P)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void ScareGuards() {
		scaredGuards = true;
		numTurnsInGuardRoom = 0;
	
		string wallText = "You put the sheet over your head and adjust it so that it covers your whole body. " + 
						"Here goes nothing!\n" +
						"You turn round the corner performing your most terrifying moan.\n\n" +
						"\"Aaaaaaahh! A ghost!\" the nearest guard shouts, then barrels past his partner.\n" + 
						"\"Noooo, it can't be! Aaaaaaaaaaaaaaahhhhh!\" The second guard joins the first, running out the Guard's room and down the corridor.\n" +
						"\'I can't believe that worked\' you think to yourself as you pull the sheet off your head.\n\n" + 
						"Now that you have a clear look at the room, you notice two hooks on the wall that hold keys. " +
						"The first holds a set of small keys on a ring, the second holds a larger, golden key.\n" +
						"There is also a wooden door with a four number combination lock.\n\n" +
						"You suspect that the guards' courage will return soon, so you'd better act fast.\n\n" +
						
						"Press R to take the Ring of keys, G to take the Golden key, ";
						
		if (numberOnArm) {
			wallText += "A to try the number on your Arm in the combination lock, ";
		}
		
		wallText += "or C to try to open the Combination lock with a random number.";
		
		textbox.text = wallText;						
		
		if (Input.GetKeyDown(KeyCode.R)) {
			hasCellKeys = true;
			GuardRoomAction(ActionInGuardRoom.TOOK_CELL_KEYS);
			currentLocation = Locations.GUARD_ROOM;
		} else if (Input.GetKeyDown(KeyCode.G)) {
			hasGoldKey = true;
			GuardRoomAction(ActionInGuardRoom.TOOK_GOLD_KEY);
			currentLocation = Locations.GUARD_ROOM;
		} else if (Input.GetKeyDown(KeyCode.C)) {
			GuardRoomAction(ActionInGuardRoom.TRIED_COMBO_LOCK);
			currentLocation = Locations.GUARD_ROOM;
		} else if (numberOnArm && Input.GetKeyDown(KeyCode.A)) {
			GuardRoomAction(ActionInGuardRoom.OPENED_COMBO_LOCK);
			currentLocation = Locations.GUARD_ROOM;
		}
	}
	
	void ScareDeath() {
		textbox.text = 	"You put the sheet over your head and adjust it so that it covers your whole body. " + 
						"Here goes nothing again!\n" +
						"You turn round the corner performing your most terrifying moan.\n\n" +
						"\"The ghost 'as come back! Ahhh!\" the nearest guard shouts.\n" + 
						"\"Hold on, that's just a white sheet over someones head!\" the second guard says with a steady voice.\n" +
						"The sheet is pulled from over your head by a shadowy hand.\n\n" +
						
						"\"See? Now let's make you a REAL ghost!\"\n" +
						"\"Yeah, let's get 'em!\"\n\n" +
						"You try to defend yourself but the guards slice you into a human jigsaw puzzle.\n\n" +
						"Press P to Play again.";
		
		if (Input.GetKeyDown(KeyCode.P)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void GuardRoom() {
		string wallText = "";
		
		switch (lastActionInGuardRoom) {
			case ActionInGuardRoom.TOOK_CELL_KEYS:
				wallText += "You take the ring of small keys from its hook. It looks like each key has a number matching a cell on it.\n\n";
				break;
			case ActionInGuardRoom.TOOK_GOLD_KEY:
				wallText += "You take the golden key from its hook. The key doesn't seem to have any identification markings on it.\n\n";
				break;
			case ActionInGuardRoom.TRIED_COMBO_LOCK:
				wallText += "You put a random number in the combination lock and try to open the door, but unfortunately the door does not yield.\n\n";
				break;
			case ActionInGuardRoom.OPENED_COMBO_LOCK:
				wallText += "You change the numbers on the combination lock to \'2461\' and pull on the door handle. " +
							"Surprisingly, the door swings open, revealing it to be a cabinet with many shelves. " +
							"The shelves are empty apart from one, which contains a dagger.\n\n" + 
							"You take the dagger and shut the cabinet.\n\n";
				hasDagger = true;
				break;
		}
		
		wallText += "You are in a guard chamber. There is a wooden door with a four number combination lock in front of you. ";
		
		if (!hasCellKeys && !hasGoldKey) {
			wallText += "There are also two hooks on the wall that hold keys. " +
						"The first holds a set of small keys on a ring, the second holds a larger, golden key.\n\n";
		} else if (hasCellKeys && !hasGoldKey) {
			wallText += "There are also two hooks on the wall, one holds nothing, the other " +
						"holds a large, golden key.\n\n";
		} else if (!hasCellKeys && hasGoldKey) {
			wallText += "There are also two hooks on the wall, one holds nothing, the other " +
						"holds a set of small keys on a ring.\n\n";
		} else {
			wallText += "There are also two empty hooks on the wall.\n\n";
		}
		
		wallText += "The long corridor you came through is to your right, and " +
					"the guards fled down a corridor to your left, though they could be back at any moment!\n\n" +
					"Press B to go Back down the corridor you came from, ";
		
		if (!hasCellKeys) {
			wallText += "R to take the Ring of keys, ";
		}
		
		if (!hasGoldKey) {
			wallText += "G to take the Golden key, ";
		}
		
		if (!hasDagger) {
			if (numberOnArm) {
				wallText += "A to try the number on your Arm in the combination lock, ";
			}
			
			wallText += "or C to try to open the Combination lock with a random number.";
		} else {
			wallText += "or D to attempt to kill the guards with your newly acquired Dagger.";
		}
		
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.B)) {
			currentLocation = Locations.HEAR_GUARDS_1;
		} else if (!hasCellKeys && Input.GetKeyDown(KeyCode.R)) {
			hasCellKeys = true;
			GuardRoomAction(ActionInGuardRoom.TOOK_CELL_KEYS);
		} else if (!hasGoldKey && Input.GetKeyDown(KeyCode.G)) {
			hasGoldKey = true;
			GuardRoomAction(ActionInGuardRoom.TOOK_GOLD_KEY);
		} else if (!hasDagger && Input.GetKeyDown(KeyCode.C)) {
			GuardRoomAction(ActionInGuardRoom.TRIED_COMBO_LOCK);
		} else if (!hasDagger && numberOnArm && Input.GetKeyDown(KeyCode.A)) {
			GuardRoomAction(ActionInGuardRoom.OPENED_COMBO_LOCK);
		} else if (hasDagger && Input.GetKeyDown(KeyCode.D)) {
			currentLocation = Locations.DAGGER_GUARDS;
		}
	}
	
	void DaggerGuards() {
		textbox.text = 	"You go after the guards with your dagger, and manage to stab one in the back.\n\n" +
						"\"Edwin! Nooooo!\" the other guard shouts, before drawing his sword and turning on you.\n" +
						"\"I'll get you for this!\" he snarls.\n\n" +
						"You fight as best as you can with your dagger, but the guard's sword meets your gut.\n\nPress P to Play again.";
		
		if (Input.GetKeyDown(KeyCode.P)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void GuardsReturn() {
		textbox.text = 	"Before you can do anything, the two guards return from the corridor.\n\n" +
						"\"See, that's no ghost! Teach that prisoner a lesson!\"\n" +
						"\"You're right, tha's no ghost. I'll teach 'em!\"\n\n" +
						"The two guards corner you and cut you to pieces. That's a pretty brutal lesson.\n\nPress P to Play again.";
		
		if (Input.GetKeyDown(KeyCode.P)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void GuardRoomAction(ActionInGuardRoom action) {
		lastActionInGuardRoom = action;
		numTurnsInGuardRoom++;
		
		if (numTurnsInGuardRoom >= NUM_TURNS_TILL_GUARDS_RETURN) {
			currentLocation = Locations.GUARDS_RETURN;
		}
	}
	
	void MonitorRoom0() {
		string wallText =	"You put the gold key in the lock, turn it, then push against the steel door. " +
							"It opens, revealing a man in front of several screens. Some screens show various rooms of the prison, " +
							"while others contain words that you can't make out. The man turns and starts speaking to you.\n\n" +
							"\"Did you really think you could escape? Through a door with the word \'escape\' on it, how laughable.\"\n\n" +
							"\"How many times do you think you've tried to escape? Once? Twice? Believe me, it is many more. " + 
							"Each time you either die or find me, and you wake up, back in your prison cell, with no memory of what has happened.\"\n\n" +
							"\"You ignorant fool. All your actions are dictated by me, in fact here is your only option at the moment.\"\n\n" +
							"Press C to Continue listening to the man's monologue.";
							
		if (hasDagger) {
			wallText += "\n\n\nPress D to ignore that option and stab the man with your Dagger.";
		}
							
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.C)) {
			currentLocation = Locations.MONITOR_ROOM_1;
		} else if (hasDagger && Input.GetKeyDown(KeyCode.D)) {
			currentLocation = Locations.MAN_DEATH;
		}
	}
	
	void MonitorRoom1() {
		string wallText =	"\"You see, I own your every movement. I could even tell you that the combination to the guards' weapons cabinet is " +
							"2461. You won't remember it long enough to be able to use it. Still, I should really reset you now.\"\n\n" +
							"The man turns back to the screens and pushes a button on the desk below them.\n\n" +
							"\"Any last requests?\"\n\n" +
							"Press I to spend your last moments hurling Insults at the man.\n";
							
		if (hasToothbrush) {
			wallText += "Press T to charge at the man and stab him with your Toothbrush.\n";
		}				
		if (hasSheet) {
			wallText += "Press S to wrap the white Sheet around the man's head and attempt to choke him.\n";
		}
		if (hasToothbrush) {
			wallText += "Press W to use the sharp end of the toothbrush to Write \'2461\' into your forearm.\n";
		}
		if (hasDagger) {
			wallText += "Press D to stab the man with your Dagger.";
		}
		
		
		textbox.text = wallText;
		
		if (Input.GetKeyDown(KeyCode.I)) {
			currentLocation = Locations.INSULT_DEATH;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.T)) {
			currentLocation = Locations.TOOTHBRUSH_DEATH;
		} else if (hasSheet && Input.GetKeyDown(KeyCode.S)) {
			currentLocation = Locations.SHEET_DEATH;
		} else if (hasToothbrush && Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.WRITE_ARM;
		} else if (hasDagger && Input.GetKeyDown(KeyCode.D)) {
			currentLocation = Locations.MAN_DEATH;
		}
	}
	
	void InsultDeath() {
		textbox.text = 	"You spew the harshest insults you can create at this man who thinks himself all-powerful.\n\n" +
						"\"How vulgar.\" the man replies, before you start feeling faint and lapse into unconsciousness.\n\n" +
						"Press W to Wake up.";
		
		if (Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void ToothbrushDeath() {
		textbox.text = 	"You charge at the man and stab him in the back with the sharp end of your toothbrush.\n\n" +
						"\"Ow\" the man says sarcastically.\n\n" +
						"You continue to stab him, but you start to feel drowsy and your vision fades to black.\n\n" +
						"Press W to Wake up.";
		
		if (Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void SheetDeath() {
		textbox.text = 	"You wrap the white sheet over the man's head and attempt to suffocate him. " +
						"The man struggles and you hear his muffled voice, which gives you some satisfaction.\n\n" +
						"However, your vision starts to fade and your muscles grow weak. Eventually, the darkness consumes you.\n\n" +
						"Press W to Wake up.";
		
		if (Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void WriteArm() {
		textbox.text = 	"You put the sharp end of the toothbrush against your arm and scratch into your skin. " +
						"You feel pain, but the number \'2461\' is now emblazoned on your forearm.\n\n" +
						"\"Whatever are you doing?\" the man asks curiously.\n\n" +
						"Your head starts to feel light, and you can tell you're about to fall unconscious. " + 
						"\'Hopefully this works\' you think to yourself, as you sink into oblivion.\n\n" +
						"Press W to Wake up.";
		
		numberOnArm = true;
		
		if (Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void ManDeath() {
		textbox.text = 	"You take your dagger and stab the man in the kidneys. He looks at you with an expression of surprise and disbelief.\n\n" +
						"\"How did you stab me?? That wasn't supposed to be possible.\"\n" +
						"\"Looks like there was a flaw in your programming\" you reply in your best cool voice.\n\n" +
						"The man collapses to the ground. You feel some guilt and hope that he doesn't suffer too much before passing on.\n\n" + 
						"You step past his body and examine the monitors behind him. There are four rows of screens, three high. " +
						"Every room of the prison you've seen so far is shown somewhere. As for those with writing, you can't make head or tail of them. " +
						"They feature words like 'private' and 'void', and have descriptions of entering rooms and picking up items.\n\n" +
						"On the desk in front of the monitors are two buttons. One labelled \'RESET\', the other labelled \'FREEDOM\'\n" +
						"Press R to press the button labelled \'RESET\'.\n" +
						"Press F to press the button labelled \'FREEDOM\'.";
						
		if (Input.GetKeyDown(KeyCode.R)) {
			currentLocation = Locations.RESET;
		} else if (Input.GetKeyDown(KeyCode.F)) {
			currentLocation = Locations.FREEDOM;
		}
	}
	
	void Reset() {
		textbox.text = 	"Your finger pushes down on the button labelled \'RESET\'. You feel yourself getting light-headed and your muscles relaxing against your will.\n\n" +
						"\'No, I've made a mistake\' you think to yourself, but it is too late. You fall to the floor and your vision fades to black.\n\n" +
						"Press W to Wake up.";
						
		if (Input.GetKeyDown(KeyCode.W)) {
			currentLocation = Locations.CELL_START;
		}
	}
	
	void Freedom() {
		textbox.text = 	"Your finger pushes down on the button labelled \'FREEDOM\'. You feel yourself getting light-headed and your muscles relaxing against your will. " +
						"The world around you starts shaking as you feel intense fear through every fibre of your body.\n\nSuddenly, everything becomes white! " +
						"The monitors, the room, the dying man, are all blotted out by an intense white light. You get the feeling that this wasn't meant to happen. " +
						"You weren't supposed to be free, the world can't cope with it for some reason. But why have the button then?\n\n" +
						"The brightness of the light around you dulls slightly. Then, as if the power went out, all is darkness. Words appear before you.\n\n" +
						"Where would you like to go today?\n\n" +
						"As you consider your answer, you're not sure what awaits you now. But one thing is for certain. You are free.\n\n" +
						"Press P to use your freedom to Play again.";
		
		if (Input.GetKeyDown(KeyCode.P)) {
			numberOnArm = false;
			numTallyMarks = 26;
			currentLocation = Locations.CELL_START;
		}
	}	
}
