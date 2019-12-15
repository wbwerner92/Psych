using UnityEngine;
using System.Collections;

public class SpritePackage : MonoBehaviour
{
	public SpriteRenderer currentSprite;
	public Sprite[] activeSprites;

	private int m_frame = 0;
	private float m_deltaTime = 0;
	private float m_frameSeconds = 0.5f;
	public bool loop = true;
	public bool facingLeft = true;

	public Sprite[] standingSprites;
	public Sprite[] movingSprites;
	public Sprite[] attackingSprites;

	public Sprite[] burstSprites;
	public Sprite[] riseSprites;
	public Sprite[] tranceSprties;

	public void FlipSprite()
	{
		if (currentSprite == null)
		{
			return;
		}
		if (facingLeft) 
		{
			currentSprite.transform.Rotate(new Vector3 (0, 180, 0));
			facingLeft = false;
		}
		else
		{
			currentSprite.transform.Rotate(new Vector3 (0, 0, 0));
			facingLeft = true;
		}

	}

	// public void SetBodSprite(string path, bool repeat = true)
	// {
	// 	object[] loadedSprites = Resources.LoadAll(spritePath + "/" + path, typeof(Sprite));
		
	// 	currentSpriteSet = new Sprite[loadedSprites.Length];
		
	// 	for (int i = 0; i < loadedSprites.Length; i++)
	// 	{
	// 		currentSpriteSet[i] = (Sprite)loadedSprites[i];
	// 	}

	// 	currentSprite.sprite = currentSpriteSet[0];
	// }
	public void SetStanding()
	{
		// SetBodSprite("stand");
		activeSprites = standingSprites;
	}
	public void SetBurst()
	{
		// SetBodSprite("burst", false);
		activeSprites = burstSprites;
	}
	public void SetRise()
	{
		// SetBodSprite("rise", false);
		activeSprites = riseSprites;
	}
	public void SetTrance()
	{
		// SetBodSprite("trance", false);
		activeSprites = tranceSprties;
	}
     
	void Update()
	{
		if (activeSprites != null && activeSprites.Length > 0)
		{
			//Keep track of the time that has passed
			m_deltaTime += Time.deltaTime;
			
			while (m_deltaTime >= m_frameSeconds) 
			{
				m_deltaTime -= m_frameSeconds;
				m_frame++;
				if(loop)
				{
					m_frame %= activeSprites.Length;
				}
				//Max limit
				else if (m_frame >= activeSprites.Length)
				{
					m_frame = activeSprites.Length - 1;
				}		
			}
			//Animate sprite with selected frame
			currentSprite.sprite = activeSprites[m_frame];
		}
	}
}
