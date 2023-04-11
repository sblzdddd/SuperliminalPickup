using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperProp : MonoBehaviour
{
    public string objectName = "unnamed";
    public bool haveGravity = true;
    public bool outline = true;
    public Color pointedColor = new Color(0f, 0.57f, 1f, 1f);
    public Color holdColor = new Color(1f, 1f, 1f, 1f);
    public void SetupSuperObject() {
        GameObject selfObj = transform.gameObject;
        if (selfObj.GetComponent<MeshCollider>() == null)
            selfObj.AddComponent(typeof(MeshCollider));
        if (selfObj.GetComponent<Rigidbody>() == null)
            selfObj.AddComponent(typeof(Rigidbody));
        if (outline && selfObj.GetComponent<Outline>() == null)
            selfObj.AddComponent(typeof(Outline));
    }
}
