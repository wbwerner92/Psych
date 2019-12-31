using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : ManagerClass 
{
	public static SpriteManager instance;

	private Dictionary<string, Sprite> singleRefSprites;
	private Dictionary<string, Sprite[]> arrayRefSprites;

	// Icon Sprites
	[SerializeField]
	private Sprite stat_up_icon;
	[SerializeField]
	private Sprite stat_down_icon;
	[SerializeField]
	private Sprite stat_even_icon;
	[SerializeField]
	private Sprite arrow_up;
	[SerializeField]
	private Sprite arrow_left;
	[SerializeField]
	private Sprite arrow_right;
	[SerializeField]
	private Sprite arrow_down;
	[SerializeField]
	private Sprite circle_icon;
	[SerializeField]
	private Sprite blank_space;

	// Bod Sprite Arrays
	public GameObject spritePackagePrefab;
	// Bod 1
	[SerializeField]
	private Sprite[] bod_1_stand;
	[SerializeField]
	private Sprite[] bod_1_burst;
	[SerializeField]
	private Sprite[] bod_1_rise;
	[SerializeField]
	private Sprite[] bod_1_trance;
	[SerializeField]
	private Sprite[] bod_1_take_damage;
	// Bod 2
	[SerializeField]
	private Sprite[] bod_2_stand;
	[SerializeField]
	private Sprite[] bod_2_burst;
	[SerializeField]
	private Sprite[] bod_2_rise;
	[SerializeField]
	private Sprite[] bod_2_trance;
	[SerializeField]
	private Sprite[] bod_2_take_damage;

	// Effect Sprites
	[SerializeField]
	private Sprite psy_bolt_cast;
	[SerializeField]
	private Sprite raise;
	[SerializeField]
	private Sprite lower;
	[SerializeField]
	private Sprite trance_wave;
	[SerializeField]
	private Sprite heal;

	void Awake()
	{
		instance = this;
	}
	void Start()
	{
		LoadSpriteResources();
		Initialize();
	}
	private void LoadSpriteResources()
	{
		singleRefSprites = new Dictionary<string, Sprite>();
		singleRefSprites.Add("stat_up_icon", stat_up_icon);
		singleRefSprites.Add("stat_down_icon", stat_down_icon);
		singleRefSprites.Add("stat_even_icon", stat_even_icon);
		singleRefSprites.Add("arrow_up", arrow_up);
		singleRefSprites.Add("arrow_down", arrow_down);
		singleRefSprites.Add("arrow_left", arrow_left);
		singleRefSprites.Add("arrow_right", arrow_right);
		singleRefSprites.Add("circle_icon", circle_icon);
		singleRefSprites.Add("blank_space", blank_space);
		singleRefSprites.Add("raise", raise);
		singleRefSprites.Add("lower", lower);
		singleRefSprites.Add("heal", heal);
		singleRefSprites.Add("psy_bolt_cast", psy_bolt_cast);
		singleRefSprites.Add("trance_wave", trance_wave);
		
		arrayRefSprites = new Dictionary<string, Sprite[]>();
		arrayRefSprites.Add("bod_1_stand", bod_1_stand);
		arrayRefSprites.Add("bod_1_burst", bod_1_burst);
		arrayRefSprites.Add("bod_1_rise", bod_1_rise);
		arrayRefSprites.Add("bod_1_trance", bod_1_trance);
		arrayRefSprites.Add("bod_1_take_damage", bod_1_take_damage);
		arrayRefSprites.Add("bod_2_stand", bod_2_stand);
		arrayRefSprites.Add("bod_2_burst", bod_2_burst);
		arrayRefSprites.Add("bod_2_rise", bod_2_rise);
		arrayRefSprites.Add("bod_2_trance", bod_2_trance);
		arrayRefSprites.Add("bod_2_take_damage", bod_2_take_damage);
	}

	public Sprite GetSingleRefSprite(string key)
	{
		if (singleRefSprites.ContainsKey(key))
		{
			return singleRefSprites[key];
		}
		else
		{
			Debug.LogError("Sprite reference " + key + " not found.");
			return null;
		}
	}
	public Sprite[] GetArrayRefSprite(string key)
	{
		if (arrayRefSprites.ContainsKey(key))
		{
			return arrayRefSprites[key];
		}
		else
		{
			Debug.LogError("Sprite reference " + key + " not found.");
			return null;
		}
	}
}
