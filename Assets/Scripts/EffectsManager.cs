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


	public void RunChainAnimationSequentially(List<BodToken> tokenList)
	{
		StartCoroutine(RunChainAnimationSequentiallyCoroutine(tokenList));
	}
	private IEnumerator RunChainAnimationSequentiallyCoroutine(List<BodToken> tokenList)
	{
		foreach (BodToken token in tokenList)
		{
			token.StartNewEffectAnimation();

			while (token.isAnimatingEffect)
				yield return null;
		}
		ResetBodSprites(tokenList);
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
