#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : EditorWindow
{
    [MenuItem("CustomEditor/WayPoint")]
    public static void ShowWindows()
    {
        GetWindow<WayPointEditor>("WayPoint");
    }

    private GameObject Parent;

    private void OnGUI()
    {
        Parent = (GameObject)EditorGUILayout.ObjectField("Parent", Parent, typeof(GameObject), true);

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Add Node", GUILayout.Width(350), GUILayout.Height(25)))
            {
                if (Parent != null)
                {
                    GameObject NodeObject = new GameObject();
                    NodeObject.transform.name = "Node_" + Parent.transform.childCount;
                    Node node = NodeObject.AddComponent<Node>();

                    // Node¿« º≥¡§

                    SphereCollider coll = node.GetComponent<SphereCollider>();
                    coll.radius = 0.05f;

                    NodeObject.transform.SetParent(Parent.transform);

                    NodeObject.transform.transform.position = new Vector3(
                        Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));

                    if(1 < Parent.transform.childCount)
                    {
                        Parent.transform.GetChild(Parent.transform.childCount - 2).GetComponent<Node>().Next = node;

                        node.Next = Parent.transform.GetChild(0).GetComponent<Node>();

                    }
                }
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }
}
#endif