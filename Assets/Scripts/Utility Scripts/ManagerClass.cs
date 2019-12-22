using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerClass : MonoBehaviour
{
    protected bool m_initialized;

    protected virtual void Initialize()
    {
        m_initialized = true;
    }
    protected virtual IEnumerator WaitToInitialize()
    {
        yield return null;
        m_initialized = true;
    }

    public bool IsInitialized()
    {
        return m_initialized;
    }
}
