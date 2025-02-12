﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniversalValues
{
    // Camera Zoom min and max
    private static float m_minZoom;
    public static float minZoom
    {
        get 
        {
            // Debug.Log("Min Zoom: " + m_minZoom);
            return m_minZoom;
        }
        set {m_minZoom = value;}
    }
    private static float m_maxZoom;
    public static float maxZoom
    {
        get 
        {
            // Debug.Log("Max Zoom: " + m_maxZoom);
            return m_maxZoom;
        }
        set {m_maxZoom = value;}
    }

    // Camera position mins and maxs
    private static float m_minPosX;
    public static float minPosX
    {
        get 
        {
            // Debug.Log("Min Pos X: " + m_minPosX);
            return m_minPosX;
        }
        set {m_minPosX = value;}
    }
    private static float m_maxPosX;
    public static float maxPosX
    {
        get 
        {
            // Debug.Log("Max Pos X: " + m_maxPosX);
            return m_maxPosX;
        }
        set {m_maxPosX = value;}
    }
    private static float m_minPosY;
    public static float minPosY
    {
        get 
        {
            // Debug.Log("Min Pos Y: " + m_minPosY);
            return m_minPosY;
        }
        set {m_minPosY = value;}
    }
    private static float m_maxPosY;
    public static float maxPosY
    {
        get 
        {
            // Debug.Log("Max Pos Y: " + m_maxPosY);
            return m_maxPosY;
        }
        set {m_maxPosY = value;}
    }
}
