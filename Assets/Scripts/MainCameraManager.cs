using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : MonoBehaviour
{
    public static MainCameraManager instance;

    private float m_defaultPosX;
    private float m_defaultPosY;
    private float m_defaultPosZ;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        m_defaultPosX = transform.position.x;
        m_defaultPosY = transform.position.y;
        m_defaultPosZ = transform.position.z;
    }
}
