using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : ManagerClass
{
    public static MainCameraManager instance;

    private float m_defaultPosX;
    private float m_defaultPosY;
    private float m_defaultPosZ;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        m_defaultPosX = transform.position.x;
        m_defaultPosY = transform.position.y;
        m_defaultPosZ = transform.position.z;

        Initialize();
    }
}
