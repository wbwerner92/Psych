using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bod : Mob
{
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

	public Bod()
	{
		Initialize();
	}

	public override void Initialize()
	{
		// Initialize Base class
		base.Initialize();

		name = "Bod";
		learnedSkills = new Dictionary<string, Skill>();
		BodManager.instance.GenerateNewPlayerCharacter(this);
		spritePath = "Sprites/package_1/Sprite_Package_1";
	}
}
