using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour 
{
	// Battle Game Object references
	public static BattleManager instance;
//	public GameObject battleBoardPrefab;
	public BattleBoard battleBoard;
	private List<BodToken> tokens;
	public ActionMenu actionMenu;
	public Text roundText;
	public Text actionDisplayText;

	// Battle Variables
	private bool battleActive;
	public bool BattleActive
	{
		get
		{
			return battleActive;
		}
	}
	private int battleTurn;
	private List<BattleTeam> battleTeams;
	private BodToken activeToken;
	public BodToken ActiveToken
	{
		get
		{
			return activeToken;
		}
	}
	public bool hasMoved;
	private List<BodToken> turnOrder;
	private int[] turnSpeedIndex;
	public bool isMoving;

	void Start () 
	{
		Debug.Log ("Starting Battle Manager");
		instance = this;
		battleActive = false;
		turnOrder = new List<BodToken>();

		// TODO: Find better place for this
		UniversalValues.minZoom = -750.0f;
		UniversalValues.maxZoom = 0.0f;
		UniversalValues.minPosX = -350.0f;
		UniversalValues.maxPosX = 350.0f;
		UniversalValues.minPosY = -150.0f;
		UniversalValues.maxPosY = 150.0f;
	}

	private void StartBattle()
	{
		battleActive = true;
		isMoving = false;
		turnSpeedIndex = new int[tokens.Count];
		SetTurnOrder();
		SelectToken(turnOrder[0]);
	}
	private void ResetBattle()
	{
		battleTurn = 1;
		UpdateRoundText();
		battleTeams = new List<BattleTeam>();
		tokens = new List<BodToken>();
		actionDisplayText.text = "";
	}
	private void UpdateRoundText()
	{
		roundText.text = "Round: " + battleTurn;
	}
	public void Start1v1Battle(BattleTeam tOne, BattleTeam tTwo)
	{
		ResetBattle();

		battleBoard.Set3x7Board();

		battleTeams.Add(tOne);
		battleTeams.Add(tTwo);

		// Team One Positions
		if (tOne.numUnits > 0) {
			BodManager.instance.ResetBod(tOne.GetMember(0));
			BodToken token1 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token1.SetTokenBod(tOne.GetMember(0), false);
			battleBoard.SetBodTokenToPosition(token1, 1, 1, true);
			tokens.Add(token1);
		}if (tOne.numUnits > 1) {
			BodManager.instance.ResetBod(tOne.GetMember(1));
			BodToken token2  = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token2.SetTokenBod(tOne.GetMember(1), false);
			battleBoard.SetBodTokenToPosition(token2, 2, 1, true);
			tokens.Add(token2);
		}if (tOne.numUnits > 2) {
			BodManager.instance.ResetBod(tOne.GetMember(2));
			BodToken token3 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token3.SetTokenBod(tOne.GetMember(2), false);
			battleBoard.SetBodTokenToPosition(token3, 0, 1, true);
			tokens.Add(token3);
		}if (tOne.numUnits > 3) {
			BodManager.instance.ResetBod(tOne.GetMember(3));
			BodToken token4 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token4.SetTokenBod(tOne.GetMember(3), false);
			battleBoard.SetBodTokenToPosition(token4, 2, 0, true);
			tokens.Add(token4);
		}if (tOne.numUnits > 4) {
			BodManager.instance.ResetBod(tOne.GetMember(4));
			BodToken token5 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token5.SetTokenBod(tOne.GetMember(4), false);
			battleBoard.SetBodTokenToPosition(token5, 0, 0, true);
			tokens.Add(token5);
		}

		// Team Two Positions
		if (tTwo.numUnits > 0) {
			BodManager.instance.ResetBod(tTwo.GetMember(0));
			BodToken token6 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token6.SetTokenBod(tTwo.GetMember(0));
			battleBoard.SetBodTokenToPosition(token6, 1, 5, true);
			tokens.Add(token6);
		}if (tTwo.numUnits > 1) {
			BodManager.instance.ResetBod(tTwo.GetMember(1));
			BodToken token7 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token7.SetTokenBod(tTwo.GetMember(1));
			battleBoard.SetBodTokenToPosition(token7, 0, 5, true);
			tokens.Add(token7);
		}if (tTwo.numUnits > 2) {
			BodManager.instance.ResetBod(tTwo.GetMember(2));
			BodToken token8 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token8.SetTokenBod(tTwo.GetMember(2));
			battleBoard.SetBodTokenToPosition(token8, 2, 5, true);
			tokens.Add(token8);
		}if (tTwo.numUnits > 3) {
			BodManager.instance.ResetBod(tTwo.GetMember(3));
			BodToken token9 = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token9.SetTokenBod(tTwo.GetMember(3));
			battleBoard.SetBodTokenToPosition(token9, 0, 6, true);
			tokens.Add(token9);
		}if (tTwo.numUnits > 4) {
			BodManager.instance.ResetBod(tTwo.GetMember(4));
			BodToken token10  = Instantiate(BodManager.instance.bodTokenPrefab).GetComponent<BodToken>();
			token10.SetTokenBod(tTwo.GetMember(4));
			battleBoard.SetBodTokenToPosition(token10, 2, 6, true);
			tokens.Add(token10);
		}

		StartBattle();
	}

	public bool TokenSpotOccupied(Vector2 pos)
	{
		if (battleBoard != null &&
		    battleBoard.tokenPositions != null &&
			battleBoard.tokenPositions[(int)pos.x, (int)pos.y] != null)
			return true;

		return false;
	}

	public void SelectToken(BodToken token)
	{
		activeToken = token;

		DeselectAll(token);

		token.selectionArrow.SetActive(true);

		battleBoard.ResetSpaces();

		actionMenu.SetBod(activeToken.bodRef);
		if (actionMenu.IsOpen() == false)
			actionMenu.OpenActionMenu();

		isMoving = false;
		hasMoved = false;
	}
	public void SelectTokenButtonPress(BodToken token)
	{
		BattleSpace space = battleBoard.battleSpaces[token.posX, token.posY];

		if (space.loadedSkill != null)
			space.PerformLoadedSkill();
		else
			battleBoard.ResetSpaces();
	}
	private void DeselectAll(BodToken ignoreToken = null)
	{
		foreach(BodToken token in tokens)
		{
			if (token != ignoreToken)
				token.selectionArrow.SetActive(false);
		}
	}

	public void PerformMove(BodToken token, Vector2 pos)
	{
		MoveToken(token, pos);

		isMoving = false;
		hasMoved = true;
	}
	private void MoveToken(BodToken token, Vector2 pos)
	{
		Debug.Log ("Moving Token");

		battleBoard.tokenPositions[token.posX, token.posY] = null;

		battleBoard.SetBodTokenToPosition(token, (int)pos.x, (int)pos.y);
	}
	public void Knockback(Vector2 targetPos)
	{
		Debug.Log ("Knockback");

		int diffX = activeToken.posX - (int)targetPos.x;
		int diffY = activeToken.posY - (int)targetPos.y;

		BodToken token = battleBoard.tokenPositions[(int)targetPos.x, (int)targetPos.y];
		Vector2 knockbackPos = Vector2.zero;

		if (diffX > diffY || (diffX == diffY && Random.Range(0, 2) == 0))
		{
			// Horizontal move

			if (activeToken.posX > (int)targetPos.x)
			{
				// Kockback left
				if (targetPos.x > 0 && battleBoard.tokenPositions[((int)targetPos.x - 1), (int)targetPos.y] == null)
				{
					knockbackPos = new Vector2((targetPos.x - 1), targetPos.y);
				}
				else
				{
					Debug.Log ("Hit something!");
				}
			}
			else
			{
				// Knockback right
				if (targetPos.x < (battleBoard.dimensionX - 1) && battleBoard.tokenPositions[((int)targetPos.x + 1), (int)targetPos.y] == null)
				{
					knockbackPos = new Vector2((targetPos.x + 1), targetPos.y);
				}
				else
				{
					Debug.Log ("Hit something!");
				}
			}
		}
		else
		{
			// Veritical move

			if (activeToken.posY > (int)targetPos.y)
			{
				// Knockback up
				if (targetPos.y > 0 && battleBoard.tokenPositions[(int)targetPos.x, ((int)targetPos.y - 1)] == null)
				{
					knockbackPos = new Vector2(targetPos.x, (targetPos.y - 1));
				}
				else
				{
					Debug.Log ("Hit something!");
				}
			}
			else
			{
				// Knockback down
				if (targetPos.y < (battleBoard.dimensionY - 1) && battleBoard.tokenPositions[(int)targetPos.x, ((int)targetPos.y + 1)] == null)
				{
					knockbackPos = new Vector2(targetPos.x, (targetPos.y + 1));
				}
				else
				{
					Debug.Log ("Hit something!");
				}
			}
		}

		if (knockbackPos != Vector2.zero)
		{
			Debug.Log ("Setting Knockback position");
			MoveToken(token, knockbackPos);
		}

	}

	public void NextTurn()
	{
		turnOrder.RemoveAt(0);

		if (turnOrder.Count <= 0)
		{
			battleTurn ++;
			UpdateRoundText();
			SetTurnOrder();
		}

		Debug.Log("Setting next turn: " + turnOrder[0].bodRef.name);

		SelectToken(turnOrder[0]);
	}

	// Sets the turn order based on acitve bod tokens in the battle and a decreasing speed index
	// 0 Values are skipped a turn and reset to the Bod's current speed
	private void SetTurnOrder()
	{
		Debug.Log ("Set Turn Order");

		// 1. Update the Turn Speed Index by decreasing all active values and resetting 0 values (Skip -1 dead players)
		for (int i = 0; i < turnSpeedIndex.Length; i++)
		{
			// If bod is dead, set turn speed index to -1
			if (tokens[i].bodRef.dead)
				turnSpeedIndex[i] = -1;

			if (turnSpeedIndex[i] == -1)
				continue;
			else if (turnSpeedIndex[i] == 0)
				turnSpeedIndex[i] = tokens[i].bodRef.spd;
			else
				turnSpeedIndex[i] --;
		}

		// 2. Generate new set of sorted BodTokens by current speed
		List<BodToken> newSet = new List<BodToken>(tokens);
		newSet.Sort(new CompareBodTokenBySpeedIndex(tokens, turnSpeedIndex));
		foreach (BodToken token in newSet)
		{
			Debug.Log ("Set Token " + token.bodRef.name + " with speed: " + turnSpeedIndex[tokens.IndexOf(token)]);
		}

		// 3. Remove any tokens with speed values less than 1 (Skips resetting speed Bods and dead Bods)
		while (turnSpeedIndex[tokens.IndexOf(newSet[newSet.Count - 1])] < 1) 
		{
			Debug.Log ("Removing 0 Val for Bod: " + tokens[tokens.IndexOf(newSet[newSet.Count - 1])].bodRef.name);
			newSet.RemoveAt (newSet.Count - 1);
		}

		// 4. Add new set to turn order
		turnOrder.AddRange(newSet);

		string finalTurnOrder = "|";
		for (int i = 0; i < turnOrder.Count; i++)
		{
			finalTurnOrder += " Turn " + (i + 1) + ": " + turnOrder[i].bodRef.name + " |";
		}
		Debug.Log ("Final Turn Order: " + finalTurnOrder);
	}
	
	List<Vector2> trackedSpaces;
	public void ShowSkillUseOnBoard(Skill skill)
	{
		battleBoard.ResetSpaces();

		Vector2 currentSpace = new Vector2(activeToken.posX, activeToken.posY);

		trackedSpaces = new List<Vector2>();

		ShowSkillUseAvailability(skill, currentSpace);
	}
	private void ShowSkillUseAvailability(Skill skill, Vector2 space)
	{
//		Debug.Log ("ShowSkillUseAvailability - 1");

		if (trackedSpaces.Contains(space))
			return;

		Vector2 currentSpace = new Vector2(activeToken.posX, activeToken.posY);
		Vector2 checkSpace = new Vector2(space.x, space.y);
		BodToken checkToken = battleBoard.tokenPositions[(int)space.x, (int)space.y];

		trackedSpaces.Add(checkSpace);

//		Debug.Log ("MaxRange: " + skill.rangeMax);

		if (Vector2.Distance(currentSpace, checkSpace) > skill.rangeMax)
			return;

		Debug.Log ("Checking: " + skill.name + " at pos: " + space.x + ", " + space.y);

		// Check usability of Skill
		if (skill.usable(activeToken.bodRef, (checkToken == null) ? null : checkToken.bodRef, currentSpace, checkSpace))
			battleBoard.ShowSpaceAvailable(checkSpace, skill);

		// Recursive Case: Check adjacent spaces
		// Up
		if (space.y > 0)
			ShowSkillUseAvailability(skill, new Vector2(space.x, (space.y - 1)));
		// Left
		if (space.x > 0)
			ShowSkillUseAvailability(skill, new Vector2((space.x - 1), space.y));
		// Down
		if (space.y < (battleBoard.dimensionY - 1))
			ShowSkillUseAvailability(skill, new Vector2(space.x, (space.y + 1)));
		// Right
		if (space.x < (battleBoard.dimensionX - 1))
			ShowSkillUseAvailability(skill, new Vector2((space.x + 1), space.y));
	}

	/// <summary>
	/// Adds display text for a Battle Action
	/// </summary>
	/// <param name="actionText">New action text</param>
	public void AddActionDisplayText(string actionText)
	{
		Debug.Log(actionText);
		actionDisplayText.text = actionDisplayText.text + "\n" + actionText;
	}
	void Update()
	{
		if (battleActive && ControlsManager.instance.controlEvent != ControlsEvent.NONE) 
		{
			Debug.Log ("Key event w/ Control: " + ControlsManager.instance.controlEvent);

			// Move Key Control Calls
			if (isMoving)
			{
				Debug.Log ("Battle Manager - Is Moving");

				if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_LEFT)
				{
					// Move Token Left
					if (activeToken.posY > 0 && battleBoard.battleSpaces[activeToken.posX, (activeToken.posY - 1)].isActive)
						battleBoard.battleSpaces[activeToken.posX, (activeToken.posY - 1)].PerformLoadedSkill();
				}
				else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_RIGHT)
				{
					// Move Token Right
					if (activeToken.posY < (battleBoard.dimensionY - 1) && battleBoard.battleSpaces[activeToken.posX, (activeToken.posY + 1)].isActive)
						battleBoard.battleSpaces[activeToken.posX, (activeToken.posY + 1)].PerformLoadedSkill();
				}
				else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_UP)
				{
					// Move Token Up
					if (activeToken.posX > 0 && battleBoard.battleSpaces[(activeToken.posX - 1), activeToken.posY].isActive)
						battleBoard.battleSpaces[(activeToken.posX - 1), activeToken.posY].PerformLoadedSkill();
				}
				else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DOWN)
				{
					// Move Token Down
					if (activeToken.posX < (battleBoard.dimensionX - 1) && battleBoard.battleSpaces[(activeToken.posX + 1), activeToken.posY].isActive)
						battleBoard.battleSpaces[(activeToken.posX + 1), activeToken.posY].PerformLoadedSkill();
				}
				else
				{
					isMoving = false;
					battleBoard.ResetSpaces();
				}
			}
			// Move Camera Control Calls
			else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_PLUS &&
						Camera.main.transform.position.z < UniversalValues.maxZoom)
			{
				// Zoom in
				Camera.main.transform.Translate(new Vector3(0, 0, 5));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_MINUS &&
						Camera.main.transform.position.z > UniversalValues.minZoom)
			{
				// Zoom out
				Camera.main.transform.Translate(new Vector3(0, 0, -5));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DIAGONAL_UP_LEFT &&
						Camera.main.transform.position.x > UniversalValues.minPosX  &&
						Camera.main.transform.position.y < UniversalValues.maxPosY)
			{
				// Move Camera Diagonal Up + Left
				Camera.main.transform.Translate(new Vector3(-5, 5, 0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DIAGONAL_UP_RIGHT &&
						Camera.main.transform.position.x < UniversalValues.maxPosX &&
						Camera.main.transform.position.y < UniversalValues.maxPosY)
			{
				// Move Camera Diagonal Up + Right
				Camera.main.transform.Translate(new Vector3(5, 5, 0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DIAGONAL_DOWN_LEFT &&
						Camera.main.transform.position.x > UniversalValues.minPosX &&
						Camera.main.transform.position.y > UniversalValues.minPosY)
			{
				// Move Camera Diagonal Down + Left
				Camera.main.transform.Translate(new Vector3(-5, -5, 0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DIAGONAL_DOWN_RIGHT &&
						Camera.main.transform.position.x < UniversalValues.maxPosX &&
						Camera.main.transform.position.y > UniversalValues.minPosY)
			{
				// Move Camera Diagonal Down + Right
				Camera.main.transform.Translate(new Vector3(5, -5, 0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_LEFT &&
						Camera.main.transform.position.x > UniversalValues.minPosX)
			{
				// Move Camera Left
				Camera.main.transform.Translate(new Vector3(-5,0,0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_RIGHT &&
						Camera.main.transform.position.x < UniversalValues.maxPosX)
			{
				// Move Camera Right
				Camera.main.transform.Translate(new Vector3(5,0,0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_UP &&
						Camera.main.transform.position.y < UniversalValues.maxPosY)
			{
				// Move Camera Up
				Camera.main.transform.Translate(new Vector3(0,5,0));
				ControlsManager.instance.readActiveInput = true;
			}
			else if (ControlsManager.instance.controlEvent == ControlsEvent.ARROW_DOWN &&
						Camera.main.transform.position.y > UniversalValues.minPosY)
			{
				// Move Camera Down
				Camera.main.transform.Translate(new Vector3(0,-5,0));
				ControlsManager.instance.readActiveInput = true;
			}
			// Action Menu Control Calls
			else if (actionMenu.IsOpen())
			{
				// Main Menu Buttons
				if (actionMenu.mainButtonSet.activeSelf)
				{
					if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_M)
					{
						actionMenu.ActionMenuMoveButton();
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_A)
					{
						actionMenu.ActionMenuActButton();
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_R)
					{
						actionMenu.ActionMenuRestButton();
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_C)
					{
						actionMenu.ActionMenuCancelButton();
					}
				}
				// Skill Type Menu Buttons
				else if (actionMenu.actionTypeButtonSet.activeSelf)
				{
					if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_P)
					{
						actionMenu.ActionMenuActionTypeButton("Physical");
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_B && actionMenu.burstButton.activeSelf)
					{
						actionMenu.ActionMenuActionTypeButton("Burst");
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_R && actionMenu.riseButton.activeSelf)
					{
						actionMenu.ActionMenuActionTypeButton("Rise");
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_T && actionMenu.tranceButton.activeSelf)
					{
						actionMenu.ActionMenuActionTypeButton("Trance");
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_X)
					{
						actionMenu.ExitActionMenuType();
					}
				}
				// Skill Buttons
				else if (actionMenu.actionButtonSet.activeSelf)
				{
					if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_X)
					{
						actionMenu.ExitSkillButtonMenu();
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_1)
					{
						actionMenu.SkillButtonUse(1);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_2)
					{
						actionMenu.SkillButtonUse(2);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_3)
					{
						actionMenu.SkillButtonUse(3);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_4)
					{
						actionMenu.SkillButtonUse(4);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_5)
					{
						actionMenu.SkillButtonUse(5);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_6)
					{
						actionMenu.SkillButtonUse(6);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_7)
					{
						actionMenu.SkillButtonUse(7);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_8)
					{
						actionMenu.SkillButtonUse(8);
					}
					else if (ControlsManager.instance.controlEvent == ControlsEvent.KEY_9)
					{
						actionMenu.SkillButtonUse(9);
					}
				}
			}
		}
	}
}

public class CompareBodTokenBySpeedIndex : IComparer<BodToken>
{
	List<BodToken> tokens;
	int[] speedIndex;

	public CompareBodTokenBySpeedIndex(List<BodToken> t, int[] i)
	{
		tokens = t;
		speedIndex = i;
	}

	public int Compare(BodToken token1, BodToken token2)
	{
		int speed1 = speedIndex[tokens.IndexOf(token1)];
		int speed2 = speedIndex[tokens.IndexOf(token2)];

		if (speed1 > speed2)
			return -1;
		else if (speed1 < speed2)
			return 1;
		else if (Random.Range (0, 2) == 1)
			return 1;
		else
			return -1;
	}
}
