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

	public Animator effectAnimator;

	// Sprite references
	public SpriteRenderer currentSprite;
	public Sprite[] currentSpriteSet;
	bool facingLeft;
	string spritePath;
	public SpriteRenderer effectSprite;
	Sprite[] currentEffectSet;
	string effectSpritePath;

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

		SetEffectSpritePath("PsyBolt_user");
	}

	public void SetTokenBod(Bod b, bool faceLeft = true)
	{
		bod = b;

		spritePath = bod.spritePath;
		SetStanding();

		facingLeft = true;
		if (!faceLeft)
			FlipSprite();

		if (BodManager.instance.GetBodMind(bod) == 0)
			mindBar.SetActive(false);

		bodMenu.SetMenu(bod);
	}

	private void FlipSprite()
	{
		if (facingLeft) 
		{
			currentSprite.transform.Rotate (new Vector3 (0, 180, 0));
			facingLeft = false;
		}
		else
		{
			currentSprite.transform.Rotate (new Vector3 (0, 0, 0));
			facingLeft = true;
		}

	}
	public void SetEffectSpritePath(string pathStr)
	{
		object[] loadedEffectSprites = Resources.LoadAll("Sprites/Effects/" + pathStr, typeof(Sprite));

		currentEffectSet = new Sprite[loadedEffectSprites.Length];

		for (int i = 0; i < loadedEffectSprites.Length; i++)
		{
			currentEffectSet[i] = (Sprite)loadedEffectSprites[i];
		}
	}
	public void SetBodSpriteForSkill(SkillType skillType)
	{
		Debug.Log("Setting Skill Type Sprite: " + skillType);
		switch (skillType)
		{
		case SkillType.Burst:
			SetBurst();
			break;
		case SkillType.Rise:
			SetRise();
			break;
		case SkillType.Trance:
			SetTrance();
			break;
		default:
			break;
		}
	}
	public void SetBodSprite(string path, bool repeat = true)
	{
		object[] loadedSprites = Resources.LoadAll(spritePath + "/" + path, typeof(Sprite));
		
		currentSpriteSet = new Sprite[loadedSprites.Length];
		
		for (int i = 0; i < loadedSprites.Length; i++)
		{
			currentSpriteSet[i] = (Sprite)loadedSprites[i];
		}

		currentSprite.sprite = currentSpriteSet[0];
	}
	public void SetStanding()
	{
		SetBodSprite("stand");
	}
	public void SetBurst()
	{
		SetBodSprite("burst", false);
	}
	public void SetRise()
	{
		SetBodSprite("rise", false);
	}
	public void SetTrance()
	{
		SetBodSprite("trance", false);
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


		effectAnimator.SetTrigger("PsyBoltUse");


		if (BattleManager.instance.BattleActive)
			BattleManager.instance.SelectTokenButtonPress(this);
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
