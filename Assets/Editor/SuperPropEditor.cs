using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SuperProp))]
public class SuperPropEditor: Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        SuperProp myScript = (SuperProp)target;
        if(GUILayout.Button("Setup Superliminal Object")) {
            myScript.SetupSuperObject();
        }
    }
}