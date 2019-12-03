using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionSkillButton : MonoBehaviour 
{
	public Text skillText;
	public Skill skillVal;
	[HideInInspector]
	public int buttonNumber;

	public void SetSkill(Skill skill, int num)
	{
		skillVal = skill;
		skillText.text = "(" + num + ") " + skill.name;
	}

}
