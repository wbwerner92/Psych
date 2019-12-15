using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleSpace
{
	public GameObject objectRef;
	public Image spaceImg;
	/// <summary>
	/// Battle space has been marked as active to receive skill
	/// </summary>
	public bool isActive;
	public int posX, posY;
	public Skill loadedSkill;

	public BattleSpace(GameObject o, int x, int y)
	{
		objectRef = o;
		posX = x;
		posY = y;
		spaceImg = objectRef.GetComponent<Image>();
		SetToActive(false);
		loadedSkill = null;

		objectRef.GetComponent<Button>().onClick.AddListener(delegate {
			Debug.Log("On Click - PosX: " + posX + ", PosY: " + posY + ", Active: " + isActive);
			PerformLoadedSkill();
		});
	}

	public void SetToActive(bool active)
	{
		if (active)
		{
			isActive = true;
			spaceImg.sprite = SpriteManager.instance.GetSingleRefSprite("circle_icon");
			BattleManager.instance.battleBoard.activeSpaces.Add(this);
		}
		else
		{
			isActive = false;
			spaceImg.sprite = SpriteManager.instance.GetSingleRefSprite("blank_space");

			if (BattleManager.instance.battleBoard.activeSpaces.Contains(this))
				BattleManager.instance.battleBoard.activeSpaces.Remove(this);
		}
	}

	public void PerformLoadedSkill()
	{
		if (!isActive || loadedSkill == null)
			return;

		Debug.Log ("Battle Space - Performing " + loadedSkill.name);

		List<BodToken> tokenList = new List<BodToken>();

		BodToken user = BattleManager.instance.ActiveToken;
		user.SetBodSpriteForSkill(loadedSkill.skillType);
		if (loadedSkill.userEffectSpritePath != "")
		{
			Debug.Log ("User effect anim: " + loadedSkill.userEffectSpritePath + ".");
			user.SetEffectSprite(loadedSkill.userEffectSpritePath);
			tokenList.Add(user);
		}

		BodToken target = BattleManager.instance.battleBoard.tokenPositions[posX, posY];
		if (loadedSkill.targetEffectSpritePath != "")
		{
			Debug.Log ("Target effect anim: " + loadedSkill.targetEffectSpritePath + ".");
			target.SetEffectSprite(loadedSkill.targetEffectSpritePath);
			tokenList.Add(target);
		}

		// Invoke the skill
		loadedSkill.action.Invoke(loadedSkill.user, (target == null) ? null : target.bodRef, new Vector2(posX, posY));

		user.UpdateStatsBars();
		if (target != null)
			target.UpdateStatsBars();

		BattleManager.instance.battleBoard.ResetSpaces();
	}
}


public class BattleBoard : MonoBehaviour 
{
	public RectTransform rect;
	public GridLayoutGroup layoutGroup;
	public GameObject battleSpacePrefab;
	public BattleSpace[,] battleSpaces;
	public List<BattleSpace> activeSpaces;
	public BodToken[,] tokenPositions;
	public int dimensionX, dimensionY;
	
	// Use this for initialization
	void Start () 
	{
		activeSpaces = new List<BattleSpace>();
	}
	
	public void FinishGeneratingBattleSpaces()
	{
		for (int y = 0; y < dimensionY; y++)
		{
			for (int x = 0; x < dimensionX; x++)
			{
				BattleSpace space = new BattleSpace(Instantiate(battleSpacePrefab), x, y);
				space.objectRef.name = space.objectRef.name.Replace("(Clone)", " ") + "(" + x + ", " + y + ")";
				space.objectRef.transform.SetParent(gameObject.transform, false);
				battleSpaces[x, y] = space;
			}
		}
		tokenPositions = new BodToken[dimensionX, dimensionY];
	}
	public void Set3x7Board()
	{
		dimensionX = 7;
		dimensionY = 3;
		rect.sizeDelta = new Vector2(490, 210);
		layoutGroup.cellSize = new Vector2(70, 70);
		battleSpaces = new BattleSpace[dimensionX, dimensionY];
		FinishGeneratingBattleSpaces();
	}

	public bool SetBodTokenToPosition(BodToken token, int x, int y, bool initial = false)
	{
		if (tokenPositions[x, y] == null)
		{
			tokenPositions[x, y] = token;
			token.posX = x;
			token.posY = y;

			RectTransform tokenRect = token.transform as RectTransform;
			tokenRect.SetParent(battleSpaces[x, y].objectRef.transform, false);
			tokenRect.pivot = new Vector2(0.5f, 0f);
			tokenRect.anchoredPosition = Vector2.zero;
			if (initial)
				tokenRect.Rotate(new Vector2(-50, 0));

			return true;
		}

		return false;
	}

	public void ResetSpaces()
	{
		foreach (BattleSpace space in battleSpaces)
		{
			space.loadedSkill = null;
			space.SetToActive(false);
		}

		activeSpaces = new List<BattleSpace>();
	}
	public void ShowSpaceAvailable(Vector2 pos, Skill skill)
	{
		BattleSpace space = battleSpaces[(int)pos.x, (int)pos.y];
		space.loadedSkill = skill;

		space.SetToActive(true);
	}

}


