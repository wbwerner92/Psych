using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleTeam
{
	public string teamName;
	private List<Bod> units;
	private const int maxUnits = 5;

	public BattleTeam()
	{
		teamName = "Team";
		units = new List<Bod>();
	}

	// Returns the current member count
	public int numUnits
	{
		get
		{
			return (units != null) ? units.Count : 0;
		}
	}

	public Bod GetMember(int i)
	{
		if (units != null && units.Count > i)
			return units [i];
		else
			return null;
	}
	public bool AddMember(Bod bod)
	{
		if (units.Count < maxUnits)
		{
			units.Add(bod);
			return true;
		}

		return false;
	}
}
