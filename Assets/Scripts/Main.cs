using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class Async {
    public static class SetTimeout {
        public static IEnumerator Run(float time, Functions.Handle.NoArgs f) {
            yield return new WaitForSeconds(time);
            f?.Invoke();
        }
    }
}
public static class ExtensionMethods {
    public static MonoBehaviour SetTimeout(this MonoBehaviour go, float delay, Functions.Handle.NoArgs func) {
        go.StartCoroutine(Async.SetTimeout.Run(delay, func));
        return go;
    }
    public static Text SetText<T>(this Text text, T data) {
        if(text)
            text.text = data.ToString();
        return text;
    }
    public static TextMesh SetText<T>(this TextMesh textMesh, T data) {
        if(textMesh)
            textMesh.text = data.ToString();
        return textMesh;
    }
    public static Renderer SetMaterialColor(this Renderer renderer, Color color) {
        if(renderer)
            if(renderer.materials.Length == 1)
                renderer.material.color = color;
        return renderer;
    }
    public static Renderer SetMaterial(this Renderer renderer, Material material) {
        if(renderer)
            if(renderer.materials.Length == 1)
                renderer.material = material;
        return renderer;
    }
    public static Transform GetNextChildren(this Transform transform) {
        if(transform)
            if(transform.parent)
                for(int i = 0; i < transform.parent.childCount; i++) {
                    if(transform.parent.GetChild(i) == transform)
                        if(i + 1 < transform.parent.childCount)
                            return transform.parent.GetChild(i + 1);
                }
        return null;
    }
    public static Transform GetPreviousChildren(this Transform transform) {
        if(transform)
            if(transform.parent)
                for(int i = 0; i < transform.parent.childCount; i++) {
                    if(transform.parent.GetChild(i) == transform)
                        if(i - 1 >= 0)
                            return transform.parent.GetChild(i - 1);
                }
        return null;
    }
    public static Transform[] GetAllChildren(this Transform transform) {
        System.Collections.Generic.List<Transform> list = new System.Collections.Generic.List<Transform>();
        if(transform)
            for(int i = 0; i < transform.childCount; i++) {
                list.Add(transform.GetChild(i));
            }
        return list.ToArray();
    }
    public static int GetChildId(this Transform transform) {
        if(transform)
            if(transform.parent)
                for(int i = 0; i < transform.parent.childCount; i++) {
                    if(transform.parent.GetChild(i) == transform)
                        return i;
                }
        return -1;
    }
    public static Transform SetName<T>(this Transform transform, T name) {
        if(transform)
            transform.name = name.ToString();
        return transform;
    }
}
public static class Functions {
    public static class Handle {
        public delegate void NoArgs();
        public delegate void OneArg(object arg);
        public delegate void listArg(params object[] arg);
        public static void Empty() {

        }
    }
    public static class CallBack {
        public delegate void NoArgs();
        public static void Empty() {

        }
    }
}
namespace Main {
    public static class Data {
        public static void Save(string file, object toSave) {
            if(!Directory.Exists(Application.persistentDataPath + "/Data/"))
                Directory.CreateDirectory(Application.persistentDataPath + "/Data/");
            FileStream saveFile = File.Create(Application.persistentDataPath + "/Data/" + file + ".txt");
            new BinaryFormatter().Serialize(saveFile, toSave);
            saveFile.Close();
        }
        public static void SaveResc(string dir, object toSave) {
            if(!Directory.Exists(Application.persistentDataPath + "/Resources/"))
                Directory.CreateDirectory(Application.persistentDataPath + "/Resources/");
            FileStream saveFile = File.Create(Application.dataPath + "/Resources/" + dir + ".txt");
            new BinaryFormatter().Serialize(saveFile, toSave);
            saveFile.Close();
        }
        public static object Load(string dir) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.OpenRead(Application.persistentDataPath + "/Data/" + dir + ".txt");
            object toReturn = (saveFile != null) ? formatter.Deserialize(saveFile) : new object();
            saveFile.Close();
            return toReturn ?? new object();
        }

        public static object LoadResource(string dir) {
            BinaryFormatter formatter = new BinaryFormatter();
            TextAsset asset = Resources.Load(dir) as TextAsset;
            object toReturn = (asset != null) ? formatter.Deserialize(new MemoryStream(asset.bytes)) : new object();
            return toReturn ?? new object();
        }
        public static void SaveRescJson<T>(string dir, string fileName, T toSave, string ext = "bytes") {
            if(!Directory.Exists(Application.dataPath + "/Resources/" + dir))
                Directory.CreateDirectory(Application.dataPath + "/Resources/" + dir);
            StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/" + dir + "/" + fileName + "." + ext);
            writer.Write(JsonUtility.ToJson(toSave));
            writer.Close();
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public static T LoadResourceJson<T>(string dir, string fileName) where T : class {
            TextAsset asset = Resources.Load(dir + "/" + fileName) as TextAsset;
            if(asset != null)
                return JsonUtility.FromJson<T>(asset.text);
            else return null;
        }

        public static void RemoveResource(string dir, string fileName, string ext = "bytes") {
            if(!Directory.Exists(Application.dataPath + "/Resources/" + dir))
                Directory.CreateDirectory(Application.dataPath + "/Resources/" + dir);
            File.Delete(Application.dataPath + "/Resources/" + dir + "/" + fileName + "." + ext);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

    public class Main : MonoBehaviour {
        #region ItemsInArray
        public static int ItemsInArray(Transform[] items) {
            int toReturn = 0;
            for(int i = 0; i < items.Length; i++)
                if(items[i] != null)
                    toReturn++;
            return toReturn;
        }
        public static int ItemsInArray(GameObject[] items) {
            int toReturn = 0;
            for(int i = 0; i < items.Length; i++)
                if(items[i] != null)
                    toReturn++;
            return toReturn;
        }
        public static int ItemsInArray(RectTransform[] items) {
            int toReturn = 0;
            for(int i = 0; i < items.Length; i++)
                if(items[i] != null)
                    toReturn++;
            return toReturn;
        }
        #endregion
    }
}