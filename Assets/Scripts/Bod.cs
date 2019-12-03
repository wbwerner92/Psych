using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bod
{
	// Info. Varibles --------------
	public string name;

	// Stats Variables -------------

	// Fortitude Stats
	public int str, strCore;	// Strength
	public int end, endCore;	// Endurance
	public int fortMod;			// Fortitude Modifier
	// Agility Stats
	public int dex, dexCore;	// Dexterity
	public int spd, spdCore;	// Speed
	public int agilMod;			// Agility Modifier
	// Mind Stats
	public int burst;			// Burst Psy
	public int rise;			// Rise Psy
	public int trance;			// Trance Psy
	public int mindMod;			// Mind Modifier
	// Extra Stats
	public int hp;				// Health Points
	public int ap;				// Action Points
	public int stress; 			// Stress Points
	public bool dead;			// Dead Status

	// Skill Variables -------------
	public Dictionary<string, Skill> learnedSkills;

	// Other Variables
	public string spritePath;

	public Bod()
	{
		name = "Bod";
		learnedSkills = new Dictionary<string, Skill>();
		BodManager.instance.GenerateNewPlayerCharacter(this);
		spritePath = "Sprites/package_1";

		dead = false;
	}
}
