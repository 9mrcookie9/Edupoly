using UnityEngine;
using UnityEditor;
using System.Collections;
using Points;

[CustomEditor(typeof(Point))]
[CanEditMultipleObjects]
public class PointEditor : Editor {
    int topOrBottom = -1;

    public override void OnInspectorGUI() {
        Point myTarget = (Point)target;
        GUILayout.Space(15);

        PointCreatorEditor pointCreator = FindObjectOfType<PointCreatorEditor>();
        Transform Points = GameObject.Find("Points")?.transform;
        if (topOrBottom < 0 || !pointCreator || !Points) {

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Add point above")) {
                topOrBottom = 0;

            }
            if (GUILayout.Button("Add point below")) {
                topOrBottom = 1;
            }
            GUILayout.EndHorizontal();
        } else {
            GUILayout.BeginVertical("box");
            GUILayout.Label(topOrBottom > 0 ? "Point above" : "Point below");
            GUILayout.BeginHorizontal();
            int thisId = myTarget.transform.GetChildId();
            if (GUILayout.Button("Standard point")) {
                var childrensBackup = Points.GetAllChildren();
                for (int i = 0; i < childrensBackup.Length; i++) {
                    childrensBackup[i].SetParent(pointCreator.transform);
                }
                Debug.Log(Points.childCount);
                for (int i = 0; i < childrensBackup.Length + 1; i++) {
                    if (i == thisId + topOrBottom) {
                        Instantiate(pointCreator.NormalPoint, Points).transform.SetName("Point #" + i).SetPositionAndRotation(childrensBackup[(i > 0 ? i - 1 : i + 1)]?.position ?? Vector3.zero, childrensBackup[(i > 0 ? i - 1 : i + 1)]?.rotation ?? Quaternion.identity);
                    } else if (i < thisId + topOrBottom) {
                        childrensBackup[i].SetName("Point #" + i);
                        childrensBackup[i].SetParent(Points);
                    } else {
                        childrensBackup[i - 1].SetName("Point #" + i);
                        childrensBackup[i - 1].SetParent(Points);
                    }
                }
                var test = FindObjectOfType<Points_Controller>();
                if (test)
                    test.points = new System.Collections.Generic.List<Point>();
            }
            if (GUILayout.Button("Special point")) {
                var childrensBackup = Points.GetAllChildren();
                for (int i = 0; i < childrensBackup.Length; i++) {
                    childrensBackup[i].SetParent(pointCreator.transform);
                }
                for (int i = 0; i < childrensBackup.Length + 1; i++) {
                    if (i == thisId + topOrBottom) {
                        Instantiate(pointCreator.SpecialPoint, Points).transform.SetName("Point #" + i);
                    } else if (i < thisId + topOrBottom) {
                        childrensBackup[i].SetName("Point #" + i);
                        childrensBackup[i].SetParent(Points);
                    } else {
                        childrensBackup[i - 1].SetName("Point #" + i);
                        childrensBackup[i - 1].SetParent(Points);
                    }
                }
                var test = FindObjectOfType<Points_Controller>();
                if (test)
                    test.points = new System.Collections.Generic.List<Point>();

            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Back")) {
                topOrBottom = -1;
            }
            GUILayout.EndVertical();
        }
        GUILayout.Space(15);
        DrawDefaultInspector();

        GUILayout.Space(15);
        if (GUILayout.Button("Save")) {
            Main.Data.SaveRescJson("Map/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, myTarget.name, myTarget.data, "json");
        }
        var data = Main.Data.LoadResourceJson<PointData>("Map/" + UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name, myTarget.name);
        if (data != null) {
            if (GUILayout.Button("Load")) {
                myTarget.data = data;
            }
            if (GUILayout.Button("Remove Save")) {
                Main.Data.RemoveResource("Map/" + UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name, myTarget.name, "json");
            }
        }
    }
}