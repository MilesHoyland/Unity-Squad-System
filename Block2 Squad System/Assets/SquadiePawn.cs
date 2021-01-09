using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct SquadiePawn
{
    public GameObject pawn { get; set; }
    SquadMemberAI memberAI;

    public SquadiePawn(SquadMemberAI squadMember, GameObject prefab)
    {
        this.pawn = GameObject.Instantiate(prefab);
        this.memberAI = squadMember;
    }
}