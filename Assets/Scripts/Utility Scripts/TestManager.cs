using UnityEngine;
using System.Collections;

public class TestManager : ManagerClass 
{
	public static TestManager instance;

	void Awake()
	{
		instance = this;
	}
	void Start () 
	{
		Debug.Log ("Starting Test Manager");
		StartCoroutine(WaitToInitialize());
	}
	protected override IEnumerator WaitToInitialize()
	{
		yield return WaitToStartTest();
		m_initialized = true;
		Debug.Log("Instances Loaded");
		StartTest();
	}

	public IEnumerator WaitToStartTest()
	{
		while ((BodManager.instance == null || BodManager.instance.IsInitialized() == false) || 
				(QuestManager.instance == null || QuestManager.instance.IsInitialized() == false) ||
				(SkillManager.instance == null || SkillManager.instance.IsInitialized() == false) || 
				(BattleManager.instance == null || BattleManager.instance.IsInitialized() == false) || 
				(ControlsManager.instance == null || ControlsManager.instance.IsInitialized() == false) || 
				(SpriteManager.instance == null || SpriteManager.instance.IsInitialized() == false) || 
				(AudioManager.instance == null || AudioManager.instance.IsInitialized() == false) ||
				(MainCameraManager.instance == null || MainCameraManager.instance.IsInitialized() == false))
			yield return null;
	}
	public void StartTest()
	{
		Bod bod1 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod1));
		bod1.name = "Bod 1";
		Bod bod2 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod2));
		bod2.name = "Bod 2";
		Bod bod3 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod3));
		bod3.name = "Bod 3";
		Bod bod4 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod4));
		bod4.name = "Bod 4";
		Bod bod5 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod5));
		bod5.name = "Bod 5";
		bod5.spritePath = "package_2";
		Bod bod6 = new Bod();
		// Debug.Log ("Generated Bod: \n" + BodManager.instance.GetStats(bod6));
		bod6.name = "Bod 6";
		bod6.spritePath = "package_2";

		BattleTeam team1 = new BattleTeam();
		team1.AddMember(bod1);
		team1.AddMember(bod2);
		team1.AddMember(bod3);
		team1.AddMember(bod4);
		BattleTeam team2 = new BattleTeam();
		team2.AddMember(bod5);
		team2.AddMember(bod6);

		BattleManager.instance.Start1v1Battle(team1, team2);
	}
	
}
