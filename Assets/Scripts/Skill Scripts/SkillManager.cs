using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour 
{
	public static SkillManager instance;
	public Dictionary<string, Skill> skillDictionary;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Starting Skill Manager");
		instance = this;

		GenerateSkillDictionary();
	}

	public Skill GetSkill(string refName)
	{
		if (skillDictionary.ContainsKey(refName) == false)
		{
			Debug.LogError("Skill not found with name: " + refName);
			return null;
		}

		Skill newSkill = new Skill();
		Skill retrievedSkill = skillDictionary[refName];
		newSkill.id = retrievedSkill.id;
		newSkill.name = retrievedSkill.name;
		newSkill.userEffectSpritePath = retrievedSkill.userEffectSpritePath;
		newSkill.targetEffectSpritePath = retrievedSkill.targetEffectSpritePath;
		newSkill.skillType = retrievedSkill.skillType;
		newSkill.rangeMin = retrievedSkill.rangeMin;
		newSkill.rangeMax = retrievedSkill.rangeMax;
		newSkill.action = retrievedSkill.action;
		newSkill.requirements = retrievedSkill.requirements;
		newSkill.usable = retrievedSkill.usable;

		return newSkill;
	}

//	public bool CanUse(Skill skill, Vector2 pos1, Vector2 pos2)
//	{
//		if (skill.user != null && skill.usable.Invoke(skill.user, skill.target, pos1, pos2) == true)
//			return true;
//		else
//			return false;
//	}
	
//	public string GetSkillInfo(Skill skill)
//	{
//		string returnStr = "";
//		
//		returnStr += 
//			"Skill Name: " + skill.name + "\n" +
//				"User: " + ((skill.user == null) ? "NULL" : skill.user.name) + "\n" + 
//				"Target: " + ((skill.target == null) ? "NULL" : skill.target.name) + "\n" +
//				"Can Use: " + CanUse(skill);
//		
//		return returnStr;
//	}

	private void GenerateSkillDictionary()
	{
		skillDictionary = new Dictionary<string, Skill>();

		Skill newSkill = null;
		string skillId = "";

		// Physical Skills ------------------------------------------------------------------------
		// Basic Strike Attack
		skillId = "basic_strike";
		newSkill = new Skill(
			skillId,
			"Strike",
			"",
			"",
			SkillType.Physical,
			1,
			1,
			delegate (Bod user, Bod target, Vector2 targetPos)
			{
				BattleManager.instance.AddActionDisplayText(user.name + " strikes " + target.name);

				// Get Attack Power and Aim
				int atkPower = user.str;
				int atkAim = user.dex;

				Debug.Log ("Attack Power: " + atkPower + ", Aim: " + atkAim);

				// Get Dodge Rate
				int dodgeVal = BodManager.instance.AgilRoll(target);

				Debug.Log ((target.ap > 0) ? "Dodge: " + dodgeVal : target.name + " is too tired to dodge");

				if (dodgeVal >= atkAim && target.ap > 0)
				{
					BattleManager.instance.AddActionDisplayText(target.name + " dodges!");
					target.ap --;
				}
				else
				{
					BattleManager.instance.AddActionDisplayText("Hit!");

					// Get Block/Dmg Absorption
					int blockVal = BodManager.instance.FortRoll(target);

					Debug.Log ((target.ap > 0) ? "Block/Absorb: " + blockVal : target.name + " is too tired to block");

					if (blockVal > 0 && target.ap > 0)
					{
						// Adjust block val if its greater than the power 
						if (blockVal > atkPower)
							blockVal = atkPower;

						BattleManager.instance.AddActionDisplayText(target.name + " absorbs " + blockVal + " damage.");

						atkPower -= blockVal;
						target.ap --;
					}

					if (atkPower > 0)
					{
						BodManager.instance.TakeDamage(target, atkPower);

						// TODO: Knockback
						int knockbackVal = blockVal * 2;
						while (target.dead == false && atkPower > knockbackVal)
						{
							BattleManager.instance.AddActionDisplayText(target.name + " is knocked back!");

							BattleManager.instance.Knockback(targetPos);

							atkPower -= knockbackVal;
						}
					}
				}

				 user.ap --;
			},
			delegate(Bod user)
			{
//				Debug.Log ("Basic Strike Requirements");
				return true;
			}, 
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2) 
			{
				Debug.Log ("Basic Strike Usable Check, AP: " + user.ap);

				int dist = Mathf.Abs((int)pos1.x - (int)pos2.x) + Mathf.Abs((int)pos1.y - (int)pos2.y);
				
				if (user == null || target == null || target == user)
				{
					Debug.Log ("Invalid user/target");
					return false;
				}
				else if (user.ap <= 0)
				{
					Debug.Log ("User out of ap");
					return false;
				}
				else if (dist != 1)
				{
					Debug.Log ("Target out of range");
					return false;
				}

				Debug.Log ("True");
				Debug.Log ("User: " + user.name + " can strike " + target.name);
				return true;
			}
		);
		skillDictionary.Add(skillId, newSkill);


		// Burst Skills ---------------------------------------------------------------------------

		// Psy Bolt: Attack with a concentrated bolt of Psy energy, does not miss
		skillId = "psy_bolt";
		newSkill = new Skill(
			skillId,
			"Psy Bolt",
			"PsyBolt_cast",
			"",
			SkillType.Burst,
			2,
			2,
			delegate (Bod user, Bod target, Vector2 targetPos)
			{
				Debug.Log ("Psy Bolt Action");

				int pow = user.burst;
				int resist = (target.stress < BodManager.instance.GetBodMind(target)) ? BodManager.instance.MindRoll(target) : 0;
				if (resist > pow)
					resist = pow;

				Debug.Log ("Power: " + pow + ", Resist: " + resist);

				if (resist > 0)
				{
					Debug.Log(target.name + " resists " + resist + " effect");
					pow -= resist;
					target.stress ++;
				}
				
				if (pow > 0)
				{
					BodManager.instance.TakeDamage(target, pow);
				}

				user.stress ++;
			},
			delegate(Bod user)
			{
//				Debug.Log ("Psy Bolt Requirements");
				if (user.burst > 0)
					return true;
				else
					return false;
			}, 
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2) 
			{
				Debug.Log ("Psy Bolt Usable Check: " + user.stress + "/" + BodManager.instance.GetBodMind(user));

				int dist = Mathf.Abs((int)pos1.x - (int)pos2.x) + Mathf.Abs((int)pos1.y - (int)pos2.y);

				if (user == null || target == null || target == user)
				{
					Debug.Log ("Invalid user/target");
					return false;				
				}
				else if (user.stress >= BodManager.instance.GetBodMind(user))
				{
					Debug.Log (user.name + " is too stressed to use power");
					return false;
				}
				else if (dist != 2)
				{
					Debug.Log ("Target out of range: " + dist);
					return false;
				}

//				Debug.Log ("True");
				Debug.Log ("User: " + user.name + " can use Psy Bolt on " + target.name);
				return true;
			}
		);
		skillDictionary.Add(skillId, newSkill);

		// Telekenetic Push: Shift a target's position away from self
		// Telekenetic Pull: Shift a target's position toward self
		// Barrier: Erect a barrier that cannot be passed through by person or attack

		// Rise Skills ----------------------------------------------------------------------------

		// Raise Strength: Psychically raise strength stat
		skillId = "raise_str";
		newSkill = new Skill (
			skillId,
			"Strength",
			"Raise",
			"",
			SkillType.Rise,
			0,
			0,
			delegate(Bod user, Bod target, Vector2 targetPos) {

			Debug.Log ("Raise Str Action");
			
			int pow = user.rise;

			Debug.Log(user.name + " raises str by : " + pow);
			user.str += pow;

			user.stress ++;
		},
			delegate(Bod user) {

//			Debug.Log ("Raise Str Requirements");
			if (user.rise > 0)
				return true;
			else 
				return false;
		},
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2) {

			Debug.Log ("Raise Str Usable Check: " + user.stress + "/" + BodManager.instance.GetBodMind(user));

			if (user == null)
			{
				Debug.Log ("Invalid user/target");
				return false;				
			}
			else if (user.stress >= BodManager.instance.GetBodMind(user))
			{
				Debug.Log (user.name + " is too stressed to use power");
				return false;
			}
			
			Debug.Log ("User: " + user.name + " can use Scan on " + target.name);
			return true;

		});
		skillDictionary.Add(skillId, newSkill);

		// Fortify - Raise Endurance: Psychically raise endurance stat
		// Sharpen - Raise Dexterity: Psychically raise dexterity stat
		// Quicken - Raise Speed: Psychically raise speed stat

		// Trance Skills --------------------------------------------------------------------------

		// Scan: Get stat info of target
		skillId = "scan";
		newSkill = new Skill(
			skillId,
			"Scan",
			"TranceWave",
			"",
			SkillType.Trance,
			1,
			3,
			delegate (Bod user, Bod target, Vector2 targetPos)
			{
				Debug.Log ("Scan Action");

				int pow = user.trance;
				int resist = (target.stress < BodManager.instance.GetBodMind(target)) ? BodManager.instance.MindRoll(target) : 0;
				if (resist > pow)
					resist = pow;
				
				Debug.Log ("Power: " + pow + ", Resist: " + resist);
				
				if (resist >= pow)
				{
					Debug.Log(target.name + " resists " + resist + " effect from Scan");
					pow -= resist;
					target.stress ++;
				}
				
				if (pow > 0)
				{
					Debug.Log(user.name + " scans info from " + target.name + ": " + BodManager.instance.GetStats(target));
				}
				
				user.stress ++;
			},
			delegate(Bod user)
			{
//				Debug.Log ("Scan Requirements");
				if (user.trance > 0)
					return true;
				else
					return false;
			}, 
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2) 
			{
				Debug.Log ("Scan Usable Check: " + user.stress + "/" + BodManager.instance.GetBodMind(user));
				
				int dist = Mathf.Abs((int)pos1.x - (int)pos2.x) + Mathf.Abs((int)pos1.y - (int)pos2.y);
				
				if (user == null || target == null || target == user)
				{
					Debug.Log ("Invalid user/target");
					return false;				
				}
				else if (user.stress >= BodManager.instance.GetBodMind(user))
				{
					Debug.Log (user.name + " is too stressed to use power");
					return false;
				}
				else if (dist < 1 || dist > 3)
				{
					Debug.Log ("Target out of range: " + dist);
					return false;
				}
				
				Debug.Log ("User: " + user.name + " can use Scan on " + target.name);
				return true;
			}
		);
		skillDictionary.Add(skillId, newSkill);
		// Antagonize: Add stress to a target to inhibit their ability to use Psy powers
		// Calm: Reduce stress in self
		// Copy Image: Make an illusion copy of target that is indistinguishable to others
		// Hypnosis: Gain control over a target's next action
	}
}
