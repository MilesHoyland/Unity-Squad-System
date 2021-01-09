using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FormationData", menuName = "Formation")]
public class FormationData : ScriptableObject
{
    public string fname;
    public List<Vector3> positions;
}
