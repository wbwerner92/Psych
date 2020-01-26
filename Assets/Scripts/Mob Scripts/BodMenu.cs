using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BodMenu : MonoBehaviour 
{
	[HideInInspector]
	public Bod bod;
	[HideInInspector]
	public bool isOpen;
	bool isAnimating;

	public RectTransform rect;
	public RectTransform parentRect;

	Vector2 setPosition;

	// Menu Display/Stat vairables
	public Text titleText;
	public Text strText;
	public Image strIcon;
	public Text endText;
	public Image endIcon;
	public Text dexText;
	public Image dexIcon;
	public Text spdText;
	public Image spdIcon;

	// Use this for initialization
	void Start () 
	{
		SetTransform();
	}

	void Update()
	{
		if (isAnimating) 
		{
			Debug.Log ("Should be animating");
			Vector2 startPos = rect.anchoredPosition;
			rect.anchoredPosition = Vector2.Lerp(startPos, setPosition, 0.05f);

			if (Mathf.Abs(startPos.x - setPosition.x) < 1 && Mathf.Abs(startPos.y - setPosition.y) < 1)
			{
				rect.anchoredPosition = setPosition;
				isAnimating = false;
			}
		}
	}

	public void SetMenu(Bod b)
	{
		bod = b;
		SetMenuValues();
	}
	public void SetMenuValues()
	{
		if (bod == null)
			return;

		titleText.text = "Name: " + bod.name;

		strText.text = bod.str + "/" + bod.strCore;
		if (bod.str > bod.strCore)
			strIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_up_icon");
		else if (bod.str < bod.strCore)
			strIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_down_icon");
		else
			strIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_even_icon");

		endText.text = bod.end + "/" + bod.endCore;
		if (bod.end > bod.endCore)
			endIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_up_icon");
		else if (bod.end < bod.endCore)
			endIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_down_icon");
		else
			endIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_even_icon");

		dexText.text = bod.dex + "/" + bod.dexCore;
		if (bod.dex > bod.dexCore)
			dexIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_up_icon");
		else if (bod.dex < bod.dexCore)
			dexIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_down_icon");
		else
			dexIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_even_icon");

		spdText.text = bod.spd + "/" + bod.spdCore;
		if (bod.spd > bod.spdCore)
			spdIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_up_icon");
		else if (bod.spd < bod.spdCore)
			spdIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_down_icon");
		else
			spdIcon.sprite = SpriteManager.instance.GetSingleRefSprite("stat_even_icon");
	}

	public void SetTransform()
	{
		// TODO: Account for Scaling
		int height = Screen.height;
		int width = Screen.width;
		Debug.Log("Height: " + height + ", Width: " + width);
		Debug.Log("LP: " + rect.anchoredPosition);

		// Set position and size of Menu
		Vector2 newSize = new Vector2(150.0f, Mathf.Min(height, rect.sizeDelta.y));
		Vector2 newPos = new Vector2(rect.anchoredPosition.x, (-1.0f * parentRect.anchoredPosition.y));

		rect.sizeDelta = newSize;
		rect.anchoredPosition = newPos;
	}

	public void OpenMenu()
	{
		if (isAnimating)
		{
			Debug.Log ("Blocked animation");
			return;
		}

		Debug.Log("Opening Bod Menu");
		gameObject.SetActive(true);
		SetMenuValues();
		StartCoroutine(WaitToOpenMenu());
	}
	public IEnumerator WaitToOpenMenu()
	{
//		isOpen = true;

		SetTransform();

		yield return new WaitForEndOfFrame();

		Vector2 startPos = rect.anchoredPosition;
		Vector2 endPos = new Vector2((-1.0f * parentRect.anchoredPosition.x), (-1.0f * parentRect.anchoredPosition.y));
		setPosition = endPos;
		Debug.Log ("Start Pos: " + startPos + ", End Pos: " + endPos);

		isAnimating = true;

		while (isAnimating)
			yield return null;

		isOpen = true;
	}
	public void CloseMenu()
	{
		if (isAnimating)
		{
			Debug.Log ("Blocked animation");
			return;
		}

		Debug.Log("Closing Bod Menu");
		StartCoroutine(WaitToCloseMenu());
	}
	public IEnumerator WaitToCloseMenu()
	{
//		isOpen = false;

		Vector2 startPos = rect.anchoredPosition;

		float posX = (-1.0f * parentRect.anchoredPosition.x) + (Screen.width / 2) + rect.sizeDelta.x + 10.0f;
		Vector2 endPos = new Vector2(posX, (-1.0f * parentRect.anchoredPosition.y));
		setPosition = endPos;
		Debug.Log ("Start Pos: " + startPos + ", End Pos: " + endPos);

		isAnimating = true;

		while (isAnimating)
			yield return null;
		
		isOpen = false;
		gameObject.SetActive(false);
	}
}
