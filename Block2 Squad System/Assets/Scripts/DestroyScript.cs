using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public float alpha = 0.5f;


    private void Awake()
    {
        Destroy(this.gameObject, 4f);
    }


    //TODO: implement a color change on the prefab when spawn
    private void Update()
    {
        ChangeAlpha(this.GetComponent<Renderer>().material, alpha);
    }

    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_color", newColor);
    }
} 
