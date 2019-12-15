using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class BodToken : MonoBehaviour
{
	// Reference to current loaded Bod
	private Bod bod;
	public Bod bodRef
	{
		get
		{
			return bod;
		}
	}

	// Sprite references
	public GameObject spriteDisplay;
	public SpritePackage spritePackage;

	// Stat Bar references
	public GameObject healthBar;
	private int maxHpWidth;
	public RectTransform hpBarRect;
	public GameObject staminaBar;
	private int maxApWidth;
	public RectTransform apBarRect;
	public GameObject mindBar;
	private int maxMindWidth;
	public RectTransform mindBarRect;

	public GameObject selectionArrow;

	// Bod Menu references
	public BodMenu bodMenu;

	// Position
	public int posX, posY;
	
	// Use this for initialization
	void Start () 
	{
//		Debug.Log ("Starting Token");
		maxHpWidth = (int)hpBarRect.sizeDelta.x;
		maxApWidth = (int)apBarRect.sizeDelta.x;
		maxMindWidth = (int)mindBarRect.sizeDelta.x;
	}

	public void SetTokenBod(Bod b, bool faceLeft = true)
	{
		// Set Bod reference
		bod = b;

		// Set SpritePackage
		string spritePath = bod.spritePath;
		spritePackage = Instantiate(Resources.Load<SpritePackage>(spritePath), spriteDisplay.transform);
		spritePackage.SetStanding();
		if (!faceLeft)
		{
			spritePackage.FlipSprite();
		}

		if (BodManager.instance.GetBodMind(bod) == 0)
		{
			mindBar.SetActive(false);
		}
		bodMenu.SetMenu(bod);
	}

	public void SetBodSpriteForSkill(SkillType skillType)
	{
		Debug.Log("Setting Skill Type Sprite: " + skillType);
		switch (skillType)
		{
			case SkillType.Burst:
				spritePackage.SetBurst();
				break;
			case SkillType.Rise:
				spritePackage.SetRise();
				break;
			case SkillType.Trance:
				spritePackage.SetTrance();
				break;
			default:
				Debug.LogError("Cannot set");
				break;
		}
	}
	
	public void ToggleBodMenu()
	{
		if (bodMenu.isOpen)
			bodMenu.CloseMenu();
		else
			bodMenu.OpenMenu();
	}

	public void BodTokenPressed()
	{
		Debug.Log("Bod Token: " + bod.name + " pressed");

		if (BattleManager.instance.BattleActive)
		{
			BattleManager.instance.SelectTokenButtonPress(this);
		}
	}

	// Calculates the percentage of HP and AP still left and udpates the UI stat bars to match
	public void UpdateStatsBars()
	{
		if (bod == null)
			return;

		int maxHpVal = BodManager.instance.GetBodMaxHP(bod);
		int hpVal = bod.hp;
		int hpPercentage = hpVal * maxHpWidth / maxHpVal;
		hpBarRect.sizeDelta = new Vector2(hpPercentage, hpBarRect.sizeDelta.y);

		int maxApVal = bod.end;
		int apVal = bod.ap;	
		int apPercentage = apVal * maxApWidth / maxApVal;
//		Debug.Log ("AP: " + apVal + " Max AP: " + maxApVal + " AP Percentage: " + apPercentage);
		apBarRect.sizeDelta = new Vector2(apPercentage, apBarRect.sizeDelta.y);

		if (mindBar.activeSelf)
		{
			int maxMindVal = BodManager.instance.GetBodMind(bod);
			int mindVal = maxMindVal - bod.stress;
			if (mindVal < 0)
				mindVal = 0;
			int mindPercentage = mindVal * maxMindWidth / maxMindVal;
			mindBarRect.sizeDelta = new Vector2(mindPercentage, mindBarRect.sizeDelta.y);
		}
	}
}
