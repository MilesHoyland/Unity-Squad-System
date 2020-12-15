using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RingMenuMono : MonoBehaviour
{
    public Ring data;
    public RingSector ringSectorPrefab;
    public float gapWidthDegree = 1f;
    public Action<string> callback;
    protected RingSector[] ringSectors;
    protected RingMenuMono parent;

    [HideInInspector]
    public string path;



    // Start is called before the first frame update
    void Start()
    {
        var stepLength = 360f / data.nodes.Length;
        var iconDist = Vector3.Distance(ringSectorPrefab.icon.transform.position, ringSectorPrefab.sectorPiece.transform.position);
        
        //place the nodes of the ring at their positions
        ringSectors = new RingSector[data.nodes.Length];
        for (int i = 0; i < data.nodes.Length; i++)
        {
            ringSectors[i] = Instantiate(ringSectorPrefab, transform);
            
            //set root element
            ringSectors[i].transform.localPosition = Vector3.zero;
            ringSectors[i].transform.localRotation = Quaternion.identity;

            //set sectors up
            ringSectors[i].sectorPiece.fillAmount = 1f / data.nodes.Length - gapWidthDegree / 360f;
            ringSectors[i].sectorPiece.transform.localPosition = Vector3.zero;
            ringSectors[i].sectorPiece.transform.localRotation = Quaternion.Euler(0,0, -stepLength/2f+ gapWidthDegree/2f + i * stepLength);
            ringSectors[i].sectorPiece.color = new Color(1f, 1f, 1f, 0.4f);

            //set icon
            ringSectors[i].icon.transform.localPosition = ringSectors[i].sectorPiece.transform.localPosition
                + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;

        }
    }

    // Update is called once per frame
    void Update()
    {
        var stepLength = 360f / data.nodes.Length;
        var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2), Vector3.forward) + stepLength / 2f);
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Click registered at angle: "+ mouseAngle);

        var activeElement = (int)(mouseAngle / stepLength);
        for(int i = 0; i < data.nodes.Length; i++)
        {
            if (i == activeElement)
                ringSectors[i].sectorPiece.color = new Color(1f, 1f, 1f, 0.75f);
            else
                ringSectors[i].sectorPiece.color = new Color(1f, 1f, 1f, 0.4f);
        }

        if(Input.GetMouseButtonDown(0))
        {
            var path = this.path + "/" + data.nodes[activeElement].nodeName;
            if(data.nodes[activeElement].nextRing != null)
            {
                var newSubRing = Instantiate(gameObject, transform.parent).GetComponent<RingMenuMono>();
                newSubRing.parent = this;
                for(var j = 0; j < newSubRing.transform.childCount; j++)
                {
                    Destroy(newSubRing.transform.GetChild(j).gameObject);
                }
                newSubRing.data = data.nodes[activeElement].nextRing;
                newSubRing.path = path;
                newSubRing.callback = callback;
            }
            else
            {
                callback?.Invoke(path);
            }
            gameObject.SetActive(false);
        }
    }

    private float NormalizeAngle(float a) => (a + 360f) % 360f;

}
