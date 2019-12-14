using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour 
{
	public static EffectsManager instance;

	// Use this for initialization
	void Start () 
	{
		instance = this;
	}

	private void ResetBodSprites(List<BodToken> tokenList)
	{
		Debug.Log("Resetting Bod's Sprites");
		foreach (BodToken token in tokenList)
		{
			token.SetStanding();
		}
	}
}
