using UnityEngine;
using System;
using System.Collections;

public class SpritePackage : MonoBehaviour
{
	public bool facingLeft = true;
	
	// Mob/Bod values
	public SpriteRenderer currentMobSprite;
	public Sprite[] activeMobSprites;
	private int m_mob_frame = 0;
	private float m_mob_deltaTime = 0;
	private float m_mob_frameSeconds = 0.5f;
	public bool mob_loop = true;
	
	// Effect values
	public SpriteRenderer currentEffectSprite;
	public Sprite[] activeEffectSprites;
	private int m_eff_frame = 0;
	private float m_eff_deltaTime = 0;
	private float m_eff_frameSeconds = 0.15f;

	public Sprite[] standingSprites;
	public Sprite[] movingSprites;
	public Sprite[] attackingSprites;

	public Sprite[] burstSprites;
	public Sprite[] riseSprites;
	public Sprite[] tranceSprties;

	void Awake()
	{
		ResetMobSpriteValues();
		ResetEffectSpriteValues();
	}

	public void SetBodPackage(string bodSpritePath)
	{
		// Debug.Log("SetBodPackage: " + bodSpritePath);
		switch (bodSpritePath)
		{
			case "package_1":
				// Debug.Log("Setting package 1 Sprites");
				standingSprites = SpriteManager.instance.bod_1_stand;
				burstSprites = SpriteManager.instance.bod_1_burst;
				riseSprites = SpriteManager.instance.bod_1_rise;
				tranceSprties = SpriteManager.instance.bod_1_trance;
				break;
			case "package_2":
				// Debug.Log("Setting package 2 Sprites");
				standingSprites = SpriteManager.instance.bod_2_stand;
				burstSprites = SpriteManager.instance.bod_2_burst;
				riseSprites = SpriteManager.instance.bod_2_rise;
				tranceSprties = SpriteManager.instance.bod_2_trance;
				break;
			default:
				break;
		}
	}

	public void FlipSprite()
	{
		if (currentMobSprite == null)
		{
			return;
		}
		if (facingLeft) 
		{
			currentMobSprite.transform.Rotate(new Vector3 (0, 180, 0));
			facingLeft = false;
		}
		else
		{
			currentMobSprite.transform.Rotate(new Vector3 (0, 0, 0));
			facingLeft = true;
		}

	}

	public void ResetMobSpriteValues()
	{
		// currentMobSprite.sprite = null;
		activeMobSprites = null;
		m_mob_frame = 0;
		m_mob_deltaTime = 0;
	}
	public void SetStanding()
	{
		ResetMobSpriteValues();
		activeMobSprites = standingSprites;
	}
	public IEnumerator WaitToSetStanding()
	{
		yield return new WaitForSeconds(1.0f);
		SetStanding();
	}
	public void SetBurst()
	{
		ResetMobSpriteValues();
		activeMobSprites = burstSprites;
	}
	public void SetRise()
	{
		ResetMobSpriteValues();
		activeMobSprites = riseSprites;
	}
	public void SetTrance()
	{
		ResetMobSpriteValues();
		activeMobSprites = tranceSprties;
	}

	public void ResetEffectSpriteValues()
	{
		currentEffectSprite.sprite = null;
		activeEffectSprites = null;
		m_eff_frame = 0;
		m_eff_deltaTime = 0;
	}
	public void SetEffectSprites(string path)
	{
		Sprite[] loadedObjs = Resources.LoadAll<Sprite>("Sprites/Effects/" + path);
		if (loadedObjs != null && loadedObjs.Length > 0)
		{
			Debug.Log("Loaded " + loadedObjs.Length + " sprites");
			ResetEffectSpriteValues();
			activeEffectSprites = loadedObjs;
		}
		else
		{
			Debug.Log("Faile to load sprites");
		}
	}
     
	void Update()
	{
		// Mob/Bod Sprite Updates
		if (activeMobSprites != null && activeMobSprites.Length > 0)
		{
			// Keep track of the time that has passed
			m_mob_deltaTime += Time.deltaTime;
			
			while (m_mob_deltaTime >= m_mob_frameSeconds) 
			{
				m_mob_deltaTime -= m_mob_frameSeconds;
				m_mob_frame++;
				if(mob_loop)
				{
					m_mob_frame %= activeMobSprites.Length;
				}
				// Max limit
				else if (m_mob_frame >= activeMobSprites.Length)
				{
					m_mob_frame = activeMobSprites.Length - 1;
				}		
			}
			// Animate sprite with selected frame
			currentMobSprite.sprite = activeMobSprites[m_mob_frame];
		}

		// Effect Sprite Update
		if (activeEffectSprites != null && activeEffectSprites.Length > 0)
		{
			// Keep track of the time that has passed
			m_eff_deltaTime += Time.deltaTime;
			
			while (m_eff_deltaTime >= m_eff_frameSeconds) 
			{
				m_eff_deltaTime -= m_eff_frameSeconds;
				m_eff_frame++;
				// Max limit
				if (m_eff_frame >= activeEffectSprites.Length)
				{
					m_eff_frame = activeEffectSprites.Length - 1;
				}		
			}
			// Animate sprite with selected frame
			currentEffectSprite.sprite = activeEffectSprites[m_eff_frame];
			// End the animation
			if (m_eff_frame == activeEffectSprites.Length - 1)
			{
				ResetEffectSpriteValues();
				StartCoroutine(WaitToSetStanding());
			}
		}
	}
}
