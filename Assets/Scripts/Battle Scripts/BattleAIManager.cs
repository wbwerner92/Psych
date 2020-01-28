using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BattleAIType
{
    int ai_behavior;
    int ai_style;
}

public class BattleAIManager : ManagerClass
{
    public static BattleAIManager instance;

    // AI Behaviors
    public const int AI_BAHAVIOR_NEUTRAL = 0;
    public const int AI_BEHAVIOR_AGGRESIVE = 1;
    public const int AI_BEHAVIOR_DEFENSIVE = 2;
    public const int AI_BEHAVIOR_SUPPORT = 3;
    // AI Styles
    public const int AI_STYLE_BURST = 0;
    public const int AI_STYLE_RISE = 1;
    public const int AI_STYLE_TRANCE = 2;


    // Active AI
    public Dictionary<Bod, BattleAIType> aiDict;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        aiDict = new Dictionary<Bod, BattleAIType>();
        Initialize();
    }
}
