using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingNode", menuName = "RingMenu/Node", order = 2)]
public class RingNode : ScriptableObject
{
    public string nodeName;
    public Sprite icon;
    public Ring nextRing;

}
