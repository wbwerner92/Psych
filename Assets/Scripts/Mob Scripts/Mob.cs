using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob
{
    // Info. Varibles --------------
	public string name;

    // Extra Stats
	public int hp;				// Health Points
	public int ap;				// Action Points
	public int stress; 			// Stress Points
	public bool dead;			// Dead Status

    // Skill Variables -------------
	public Dictionary<string, Skill> learnedSkills;

	// Other Variables
	public string spritePath;

    public virtual void Initialize()
    {
        dead = false;
    }
}
