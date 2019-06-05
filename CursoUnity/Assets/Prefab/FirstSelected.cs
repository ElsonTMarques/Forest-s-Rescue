using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelected : MonoBehaviour
{
    [SerializeField]
    private EventSystem m_EventSystem;

    [SerializeField]
    private GameObject m_SelectedObject;

    public void OnEnable()
    {
        SelectObjectOnInput();
    }

    public void SelectObjectOnInput()
    {
        if (m_EventSystem && m_SelectedObject)
        {
            m_EventSystem.SetSelectedGameObject(null);
            m_EventSystem.SetSelectedGameObject(m_SelectedObject);
            Canvas.ForceUpdateCanvases();
        }
    }
}