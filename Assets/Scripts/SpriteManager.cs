using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour 
{
	public static SpriteManager instance;

	public Sprite stat_up_icon;
	public Sprite stat_down_icon;
	public Sprite state_even_icon;
	public Sprite arrow_up;
	public Sprite arrow_left;
	public Sprite arrow_right;
	public Sprite arrow_down;
	public Sprite circle_icon;
	public Sprite blank_space;

	// Use this for initialization
	void Start () 
	{
		instance = this;
	}
}
