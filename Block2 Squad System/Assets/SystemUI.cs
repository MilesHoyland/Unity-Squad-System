using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemUI : MonoBehaviour
{
    public Text controlMode;
    public Text squadState;

    [SerializeField] TextMeshProUGUI m_squadStateTxt;
    

    void Start()
    {
        if (m_squadStateTxt == null)
        {
            Debug.LogError("Squad state not referenced.");
        }
    }

    public void UpdateSquadState(string change)
    {
        m_squadStateTxt.text = change;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
