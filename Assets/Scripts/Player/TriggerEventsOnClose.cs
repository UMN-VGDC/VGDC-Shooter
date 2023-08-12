using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerEventsOnClose : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEnter;
    [SerializeField] private UnityEvent triggerExit;
    public void TriggerEnter()
    {
        triggerEnter.Invoke();
    }

    public void TriggerExit()
    {
        triggerExit.Invoke();
    }
}
