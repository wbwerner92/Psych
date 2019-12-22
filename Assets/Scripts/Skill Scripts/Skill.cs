using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public enum SkillType
{
	None,
	Physical,
	Burst,
	Rise,
	Trance
}

public class Skill 
{
	public string id;
	public string name;
	public SkillType skillType;
	public Bod user;
	public Vector2 targetPos;
	public string userEffectSpritePath;
	public string targetEffectSpritePath;

	public int rangeMin, rangeMax;

	public delegate void SkillAction(Bod user, Bod target, Vector2 targetPos);
	public SkillAction action;
	public delegate bool SkillRequirements(Bod user);
	public SkillRequirements requirements;
	public delegate bool SkillUsable(Bod user, Bod target, Vector2 pos1, Vector2 pos2);
	public SkillUsable usable;

	public Skill(){}
	public Skill(string i, string n, string ue, string te, SkillType type, int min, int max, SkillAction act, SkillRequirements req, SkillUsable use)
	{
		id = i;
		name = n;
		skillType = type;
		userEffectSpritePath = ue;
		targetEffectSpritePath = te;
		rangeMin = min;
		rangeMax = max;
		action = act;
		requirements = req;
		usable = use;
	}	
}
