using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BodManager : ManagerClass 
{
	public static BodManager instance;
	public GameObject bodTokenPrefab;

	void Awake()
	{
		instance = this;
	}
	void Start () 
	{
		// Debug.Log ("Starting Bod Manager");
		Initialize();
	}

	public void GenerateNewPlayerCharacter(Bod bod)
	{
		GenerateCharacterBodyStats(bod);
		GenerateCharacterMindStats(bod);
		GenerateCharacterSkills(bod);
	}
	public void GenerateCharacterBodyStats(Bod bod)
	{
		bod.str = bod.strCore = UnityEngine.Random.Range(1, 6);
		bod.end = bod.endCore = UnityEngine.Random.Range(1, 6);
		bod.fortMod = 0;
		bod.dex = bod.dexCore = UnityEngine.Random.Range(1, 6);
		bod.spd = bod.spdCore = UnityEngine.Random.Range(1, 6);
		bod.agilMod = 0;
	}
	public void GenerateCharacterMindStats(Bod bod)
	{
		bod.burst = UnityEngine.Random.Range(0, 4);
		bod.rise = UnityEngine.Random.Range(0, 4);
		bod.trance = UnityEngine.Random.Range(0, 4);
		bod.mindMod = 0;
	}
	public void GenerateCharacterSkills(Bod bod)
	{
		LearnSkill(bod, SkillManager.instance.GetSkill("basic_strike"));
		LearnSkill(bod, SkillManager.instance.GetSkill("psy_bolt"));
		LearnSkill(bod, SkillManager.instance.GetSkill("raise_str"));
		LearnSkill(bod, SkillManager.instance.GetSkill("scan"));
	}
	public void LearnSkill(Bod bod, Skill skill)
	{
		// Debug.Log ("Learn skill: " + skill.name);

		if (bod.learnedSkills.ContainsKey(skill.id))
		{
			// Debug.Log(bod.name + " already knows this skill");
			return;
		}
		if (skill.requirements(bod) == false)
		{
			// Debug.Log(bod.name + " does not meet the requirments to learn: " + skill.name);
			return;
		}

		// Debug.Log(bod.name + " learned " + skill.name + ", maxRange: " + skill.rangeMax);
		bod.learnedSkills.Add(skill.id, skill);
		skill.user = bod;
	}

	public int GetBodMaxHP(Bod bod)
	{
		return bod.strCore + bod.endCore + bod.dexCore + bod.spdCore;
	}
	public int GetBodMind(Bod bod)
	{
		return bod.burst + bod.rise + bod.trance;
	}
	public void ResetBod(Bod bod)
	{
		bod.dead = false;
		bod.hp = GetBodMaxHP(bod);
		bod.ap = bod.endCore;
		bod.stress = 0;
	}
	public int FortRoll(Bod bod)
	{
		return Mathf.Max(0, UnityEngine.Random.Range(bod.fortMod, (bod.str + bod.end + 1)));
	}
	public int AgilRoll(Bod bod)
	{
		return Mathf.Max(0, UnityEngine.Random.Range(bod.agilMod, (bod.dex + bod.spd + 1)));
	}
	public int MindRoll(Bod bod)
	{
		return Mathf.Max(0, UnityEngine.Random.Range(bod.mindMod, (GetBodMind(bod) - bod.stress + 1)));
	}

	public void TakeDamage(Bod bod, int dmg)
	{
		BattleManager.instance.AddActionDisplayText(bod.name + " takes " + dmg + " damage!");

		bod.hp -= dmg;

		if (bod.hp <= 0)
		{
			BattleManager.instance.BodDefeated(bod);
			bod.hp = 0;
			bod.dead = true;
		}
	}

	public string GetStats(Bod bod)
	{
		string returnStr = "";
		returnStr += 
				"Name: " + bod.name + "\n" + 
				"Strength: " + bod.str + " (" + bod.strCore + ")\n" +
				"Endurance: " + bod.end + " (" + bod.endCore + ")\n" + 
				"Dexterity: " + bod.dex + " (" + bod.dexCore + ")\n" + 
				"Speed: " + bod.spd + " (" + bod.spdCore + ")\n" +
				"Burst: " + bod.burst + "\n" + 
				"Rise: " + bod.rise + "\n" + 
				"Trance: " + bod.trance;
		return returnStr;
	}

	public void BodBattleRest(Bod bod)
	{
		bod.ap = bod.end;
		bod.stress = 0;
	}
}

public class CompareBodTokensBySpeed : IComparer<BodToken>
{
	public int Compare(BodToken token1, BodToken token2)
	{
		if (token1.bodRef.spd > token2.bodRef.spd)
			return -1;
		else if (token1.bodRef.spd < token2.bodRef.spd)
			return 1;
		else 
			return 0;
	}
}