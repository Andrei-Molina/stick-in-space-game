using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyTrigger : MonoBehaviour
{
    [SerializeField] protected KeyCode key;

    protected virtual void Update()
    {
        if (Input.GetKeyDown(key))
        {
            TriggerEffect();
        }
    }

    public abstract void TriggerEffect();
}
