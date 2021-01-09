using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class FormationManager : MonoBehaviour
{
    public Squad squad;
    public GameObject fSquadiePrefab;
    [SerializeField] List<SquadiePawn> fSquadies;
    public Transform fOrigin;

    public FormationData formationData;

    void Start()
    {
        if(!fOrigin)
        {
            Debug.LogError("Formation Origin not referenced");
        }
        if(!fSquadiePrefab)
        {
            Debug.LogError("Formation Squadie Piece not set up.");
        }
        if (!squad)
        {
            Debug.LogError("Squad is not referenced.");
        }
        else
        {
            // create a squadie piece for each squad member in squad
            fSquadies = new List<SquadiePawn>();
            foreach(var s in squad.squad)
            {
                fSquadies.Add(new SquadiePawn(s, fSquadiePrefab));
            }
            // sets formation pieces to be a child of the origin & resets the local position to 0,0,0
            foreach(var sPiece in fSquadies)
            {
                sPiece.pawn.transform.SetParent(fOrigin);
                sPiece.pawn.transform.SetPositionAndRotation(fOrigin.transform.position,Quaternion.identity);
            }
        }
        LoadFormation();

    }

    void LoadFormation()
    {
        int position_count = formationData.positions.Count;

        for (int i = 0; i < fSquadies.Count; i++)
        { 
            fSquadies[i].pawn.transform.position = fOrigin.position +  formationData.positions[i];
            
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFormation();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SaveFormation();
        }
    }
    void SaveFormation()
    {
        Debug.Log("Save formation called.");
        // int i = 0;

        for(int i = 0; i < fSquadies.Count; i++)
        {
            formationData.positions[i] = (fSquadies[i].pawn.transform.position - fOrigin.position);
        }
    }

}
