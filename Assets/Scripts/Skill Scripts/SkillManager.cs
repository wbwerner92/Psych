using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : ManagerClass
{
	public static SkillManager instance;
	public Dictionary<string, Skill> skillDictionary;

	void Awake()
	{
		instance = this;
	}
	void Start () 
	{
		// Debug.Log("Starting Skill Manager");
		GenerateSkillDictionary();
		Initialize();
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
				// Add action text for attack
				BattleManager.instance.AddActionDisplayText(user.name + " strikes " + target.name);
				
				// Get Token Refs
				BodToken userToken = BattleManager.instance.GetToken(user);
				BodToken targetToken = BattleManager.instance.GetToken(target);

				// Get Attack Power and Aim
				int atkPower = user.str;
				int atkAim = user.dex;
				Debug.Log ("Attack Power: " + atkPower + ", Aim: " + atkAim);

				// Declare audio string for audio effect
				string audioStr = "";

				// Get Dodge Rate
				int dodgeVal = BodManager.instance.AgilRoll(target);
				Debug.Log ((target.ap > 0) ? "Dodge: " + dodgeVal : target.name + " is too tired to dodge");
				if (dodgeVal >= atkAim && target.ap > 0)
				{
					BattleManager.instance.AddActionDisplayText(target.name + " dodges!");
					audioStr = "miss_sound";
					target.ap --;
				}
				else
				{
					// Add action text for target being hit
					BattleManager.instance.AddActionDisplayText("Hit!");

					// Get Block/Dmg Absorption
					int blockVal = BodManager.instance.FortRoll(target);
					Debug.Log ((target.ap > 0) ? "Block/Absorb: " + blockVal : target.name + " is too tired to block");
					if (blockVal > 0 && target.ap > 0)
					{
						// Adjust block val if its greater than the power 
						if (blockVal > atkPower)
						{
							blockVal = atkPower;
						}
						// Add action text for absorbed/blocked damage
						BattleManager.instance.AddActionDisplayText(target.name + " absorbs " + blockVal + " damage.");
						atkPower -= blockVal;
						target.ap --;
					}

					// Apply remaining power as damage
					if (atkPower > 0)
					{
						BodManager.instance.TakeDamage(target, atkPower);
						targetToken.spritePackage.SetTakeDamage();
						audioStr = "hit_sound";

						// TODO: Knockback
						int knockbackVal = blockVal * 2;
						Debug.Log("Knockback: " + knockbackVal);
						// while (target.dead == false && atkPower > knockbackVal)
						// {
						// 	// Add action text for knockback effect
						// 	BattleManager.instance.AddActionDisplayText(target.name + " is knocked back!");
						// 	BattleManager.instance.Knockback(targetPos);
						// 	atkPower -= knockbackVal;
						// }

						targetToken.spritePackage.StartWaitToSetStanding();
					}
				}

				// Adjust user action points
				user.ap --;

				// Play audio clip
				AudioManager.instance.PlayAudioClip(audioStr);
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

				// Get Token Refs
				BodToken userToken = BattleManager.instance.GetToken(user);
				BodToken targetToken = BattleManager.instance.GetToken(target);

				BattleManager.instance.AddActionDisplayText(user.name + " fires a Psy Bolt at " + target.name + "!");

				int pow = user.burst;
				int resist = (target.stress < BodManager.instance.GetBodMind(target)) ? BodManager.instance.MindRoll(target) : 0;
				if (resist > pow)
				{
					resist = pow;
				}

				Debug.Log ("Power: " + pow + ", Resist: " + resist);

				if (resist > 0)
				{
					BattleManager.instance.AddActionDisplayText(target.name + " resists " + resist + " effect");
					pow -= resist;
					target.stress ++;

					// TODO: Target Sprite = Resist Damage
				}
				
				if (pow > 0)
				{
					BodManager.instance.TakeDamage(target, pow);
					targetToken.spritePackage.SetTakeDamage();
					targetToken.spritePackage.StartWaitToSetStanding();
				}

				AudioManager.instance.PlayAudioClip("psy_laser_sound");
				user.stress ++;
			},
			delegate(Bod user)
			{
				// Debug.Log ("Psy Bolt Requirements");
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
		skillId = "psy_push";
		newSkill = new Skill
		(
			skillId,
			"Psy Push",
			"",
			"",
			SkillType.Burst,
			1,
			1,
			delegate(Bod user, Bod target, Vector2 targetPos)
			{
				Debug.Log ("Psy Push Action");

				// Get Token Refs
				BodToken userToken = BattleManager.instance.GetToken(user);
				BodToken targetToken = BattleManager.instance.GetToken(target);

				BattleManager.instance.AddActionDisplayText(user.name + " telekinetically pushes " + target.name + "!");

				int pow = user.burst;
				int resist = (target.stress < BodManager.instance.GetBodMind(target)) ? BodManager.instance.MindRoll(target) : 0;
				if (resist > pow)
				{
					resist = pow;
				}
				Debug.Log ("Power: " + pow + ", Resist: " + resist);
				if (resist > 0)
				{
					BattleManager.instance.AddActionDisplayText(target.name + " resists " + resist + " effect");
					pow -= resist;
					target.stress ++;

					// TODO: Target Sprite = Resist Damage
				}
				if (pow > 0)
				{
					BattleManager.instance.AddActionDisplayText(target.name + " is pushed back");
					targetToken.spritePackage.SetTakeDamage();
					BattleManager.instance.Knockback(targetPos);

					// while (target.dead == false && atkPower > knockbackVal)
					// {
					// 	// Add action text for knockback effect
					// 	BattleManager.instance.AddActionDisplayText(target.name + " is knocked back!");
					// 	BattleManager.instance.Knockback(targetPos);
					// 	atkPower -= knockbackVal;
					// }
					
					
					targetToken.spritePackage.StartWaitToSetStanding();
				}

				// TODO: Push Sound Effect
				// AudioManager.instance.PlayAudioClip("psy_laser_sound");
				user.stress ++;
			},
			delegate(Bod user)
			{
				// Debug.Log ("Psy Bolt Requirements");
				if (user.burst > 0)
					return true;
				else
					return false;
			},
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2)
			{
				return true;
			}
		);
		skillDictionary.Add(skillId, newSkill);

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
			delegate(Bod user, Bod target, Vector2 targetPos) 
			{
				Debug.Log ("Raise Str Action");

				// Get Token Refs
				BodToken userToken = BattleManager.instance.GetToken(user);
				BodToken targetToken = BattleManager.instance.GetToken(target);
				
				int pow = user.rise;

				BattleManager.instance.AddActionDisplayText(user.name + " raises str by : " + pow);
				AudioManager.instance.PlayAudioClip("power_up_sound");

				user.str += pow;
				user.stress ++;
			},
			delegate(Bod user) 
			{
	//			Debug.Log ("Raise Str Requirements");
				if (user.rise > 0)
					return true;
				else 
					return false;
			},
			delegate(Bod user, Bod target, Vector2 pos1, Vector2 pos2) 
			{
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
				
				Debug.Log ("User: " + user.name + " Strength");
				return true;
			}
		);
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
			"TranceWave",
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
					BattleManager.instance.AddActionDisplayText(target.name + " resists " + resist + " effect from Scan");
					pow -= resist;
					target.stress ++;
				}
				else
				{
					BattleManager.instance.AddActionDisplayText(user.name + " scans info from " + target.name + ":\n" + BodManager.instance.GetStats(target));
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
