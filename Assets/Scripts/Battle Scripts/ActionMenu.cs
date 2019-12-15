using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionMenu : MonoBehaviour 
{
	public Text bodNameText;
	public GameObject mainButtonSet;
	public GameObject actionTypeButtonSet;
	public GameObject burstButton;
	public GameObject riseButton;
	public GameObject tranceButton;
	public GameObject actionButtonSet;
	public GameObject actionButtonPrefab;
	public RectTransform actionButtonContent;
	public Animation animation;
	private bool isOpen;
	private Bod bod;

	public List<ActionSkillButton> loadedSkillButtons;

	void Start()
	{
		isOpen = false;
	}

	public void SetBod(Bod b)
	{
		Debug.Log ("Setting Action Menu for: " + b.name);
		bod = b;

		Debug.Log (BodManager.instance.GetStats(bod));

		bodNameText.text = bod.name;

		mainButtonSet.SetActive(true);
		actionTypeButtonSet.SetActive(false);
		actionButtonSet.SetActive(false);
	}

	public bool IsOpen()
	{
		return isOpen;
	}
	public void OpenActionMenu()
	{
		if (animation.isPlaying)
			animation.Stop();

		animation.Play("OpenActionMenu");
		isOpen = true;
	}
	public void CloseActionMenu()
	{
		if (animation.isPlaying)
			animation.Stop();
		
		animation.Play("CloseActionMenu");
		isOpen = false;
	}

	// Main Button Functions -----
	public void ActionMenuMoveButton()
	{
		Debug.Log ("Action Menu Move Button Pressed");

		Skill moveSkill = new Skill(
			"move",
			"Move",
			"",
			"",
			SkillType.None,
			1,
			1,
			delegate (Bod user, Bod target, Vector2 targetPos)
			{
				Debug.Log ("Moving: " + user.name);
				BattleManager.instance.AddActionDisplayText(user.name + " moved to position (" + targetPos.y + ", " + targetPos.x + ")");
				BattleManager.instance.PerformMove(BattleManager.instance.ActiveToken, targetPos);
				user.ap --;
			},
			delegate (Bod user)
			{
				return true;
			},
			delegate (Bod user, Bod target, Vector2 pos1, Vector2 pos2) 
			{
				if (BattleManager.instance.TokenSpotOccupied(pos2))
					return false;

				float dist = Vector2.Distance(pos1, pos2);

				Debug.Log("Dist: " + dist);

				if (dist != 1 || user.ap <= 0 || BattleManager.instance.hasMoved)
					return false;
				else
					return true;
			}
		);

		moveSkill.user = BattleManager.instance.ActiveToken.bodRef;

		BattleManager.instance.ShowSkillUseOnBoard(moveSkill);

		if (BattleManager.instance.battleBoard.activeSpaces.Count > 0)
			BattleManager.instance.isMoving = true;
	}
	public void ActionMenuActButton()
	{
		Debug.Log ("Action Menu Act Button Pressed");
		mainButtonSet.SetActive(false);
		actionTypeButtonSet.SetActive(true);

		if (bod.burst == 0)
			burstButton.SetActive(false);
		else
			burstButton.SetActive(true);
		if (bod.rise == 0)
			riseButton.SetActive(false);
		else
			riseButton.SetActive(true);
		if (bod.trance == 0)
			tranceButton.SetActive(false);
		else 
			tranceButton.SetActive(true);
	}
	public void ActionMenuRestButton()
	{
		Debug.Log ("Action Menu Rest Button Pressed");

		Debug.Log (BattleManager.instance.ActiveToken.bodRef.name + " rests.");

		BodManager.instance.BodBattleRest(BattleManager.instance.ActiveToken.bodRef);
		BattleManager.instance.ActiveToken.UpdateStatsBars();

		BattleManager.instance.NextTurn();
	}
	public void ActionMenuCancelButton()
	{
		Debug.Log ("Action Menu Cancel Button Pressed");
		CloseActionMenu();

		BattleManager.instance.NextTurn();
	}
	// Action Type Button Functions -----
	public void ActionMenuActionTypeButton(string str)
	{
		actionTypeButtonSet.SetActive(false);
		actionButtonSet.SetActive(true);
		FillSkillButtons(str);
	}
	public void ExitActionMenuType()
	{
		actionTypeButtonSet.SetActive(false);
		mainButtonSet.SetActive(true);
	}

	public void SkillButtonUse(int i)
	{
		if (i > 0 && loadedSkillButtons.Count >= i)
		{
			ActionSkillButton(loadedSkillButtons[i - 1]);
		}
	}

	public void ExitSkillButtonMenu()
	{
		actionButtonSet.SetActive(false);
		actionTypeButtonSet.SetActive(true);
	}


	public void FillSkillButtons(string skillStr)
	{
		SkillType type = SkillType.None;
		switch(skillStr)
		{
		case "Physical":
			type = SkillType.Physical;
			break;
		case "Burst":
			type = SkillType.Burst;
			break;
		case "Rise":
			type = SkillType.Rise;
			break;
		case "Trance":
			type = SkillType.Trance;
			break;
		}
		if (type == SkillType.None)
			return;

		foreach (Transform child in actionButtonContent.transform)
		{
			Destroy(child.gameObject);
		}

		loadedSkillButtons = new List<ActionSkillButton>();
		int skillNum = 1;
		foreach (string key in bod.learnedSkills.Keys)
		{
			Skill skill = bod.learnedSkills[key];
			if (skill.skillType == type)
			{
				ActionSkillButton button = Instantiate(actionButtonPrefab).GetComponent<ActionSkillButton>();
				button.SetSkill(skill, skillNum);
				button.buttonNumber = skillNum;
				skillNum++;
				button.transform.SetParent(actionButtonContent, false);
				loadedSkillButtons.Add(button);
			}
		}

		Debug.Log ("Loaded " + loadedSkillButtons.Count + " skills.");
	}

	public void ActionSkillButton(ActionSkillButton button)
	{
		Debug.Log("Skill Button: " + button.skillVal.name + " pressed.");

		BattleManager.instance.ShowSkillUseOnBoard(button.skillVal);
	}
}
