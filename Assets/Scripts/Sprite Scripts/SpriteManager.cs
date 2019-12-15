using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour 
{
	public static SpriteManager instance;

	// Icon Sprites
	public Sprite stat_up_icon;
	public Sprite stat_down_icon;
	public Sprite state_even_icon;
	public Sprite arrow_up;
	public Sprite arrow_left;
	public Sprite arrow_right;
	public Sprite arrow_down;
	public Sprite circle_icon;
	public Sprite blank_space;

	// Bod Sprite Arrays
	public GameObject spritePackagePrefab;
	// Bod 1
	public Sprite[] bod_1_stand;
	public Sprite[] bod_1_burst;
	public Sprite[] bod_1_rise;
	public Sprite[] bod_1_trance;
	// Bod 2
	public Sprite[] bod_2_stand;
	public Sprite[] bod_2_burst;
	public Sprite[] bod_2_rise;
	public Sprite[] bod_2_trance;

	// Use this for initialization
	void Start () 
	{
		instance = this;
	}
}
