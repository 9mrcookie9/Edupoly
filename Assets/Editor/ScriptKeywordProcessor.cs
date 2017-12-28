/*===============================================================
Product:    Unity *.
Developer:  Kacper Sobek - kacper.sobek2@gmail.com
Company:    Cookie Games
Date:       04.08.2017 17:00
================================================================*/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

internal sealed class ScriptKeywordProcessor: UnityEditor.AssetModificationProcessor {
    public static string ReadFile(string path) {
        return new System.IO.StreamReader(System.IO.File.OpenRead(Application.dataPath + "/" + path)).ReadToEnd();
    }
    public struct C2String {
        public string find;
        public string replace;
        public C2String(string find, string replace) {
            this.find = find;
            this.replace = replace;
        }
    }
    // UNITY DOCS: This is called by Unity when it is about to create an asset not imported by the user, eg. ".meta" files.
    public static void OnWillCreateAsset(string path) {
        List<C2String> ToReplace = new List<C2String>();
        var Template = ReadFile("Editor/NewScript.txt"); //Cant be readed by compilator as code!
        if (Template == null) {
            Debug.LogError("Template not found!");
            return;
        }
        // The path looks like this when created "Assets/ExampleScript.cs.meta"
        // So our first job is to remove the ".meta " part from the path
        path = path.Replace(".meta", "");
        string[] pth = path.Split("/".ToCharArray());

        ToReplace.Add(new C2String("#CREATIONDATE#", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
        ToReplace.Add(new C2String("#PROJECTNAME#", PlayerSettings.productName));
        ToReplace.Add(new C2String("#DEVELOPERNAME#", "System user: " + System.Environment.UserName + " - Device: " + System.Environment.UserDomainName));
        ToReplace.Add(new C2String("#COMPANY#", PlayerSettings.companyName));


        string Namespace = "Main";
        string Behav = "MonoBehaviour";
        string tmAndGo = "GameObject go;\nTransform tm;";
        string tmAndGoLoad = "tm = gameObject.transform;\ngo = gameObject;";
        bool bRemoveEditor = true;
        if (pth.Length > 3) {
            //Path should be Assets/Scripts/..../file.cs now
            if (pth[1] == "Scripts") {
                Namespace = pth[2];
            }
        }
        if (pth.Length - 2 >= 0) {
            if (pth[pth.Length - 2] == "Editor") {
                Namespace += ".Editor";
                Behav = "UnityEditor.Editor";
                tmAndGo = "";
                tmAndGoLoad = "";
                bRemoveEditor = false;
            }
        }
        ToReplace.Add(new C2String("#NAMESPACE#", Namespace));
        ToReplace.Add(new C2String("#BEHAV#", Behav));
        ToReplace.Add(new C2String("#TMANDGO#", tmAndGo));
        ToReplace.Add(new C2String("#TMANDGOLOAD#", tmAndGoLoad));
        
        // Find the index of '.' before extension, in what index the extension starts?
        var index = path.LastIndexOf(".");
        // If it does not contain a '.' character after removing the ".meta", return, it's not what we are looking for
        if (index == -1)
            return;
        ToReplace.Add(new C2String("#SCRIPTNAME#", System.IO.Path.GetFileName(path).Remove(System.IO.Path.GetFileName(path).LastIndexOf("."))));
        // Get the substring after '.' using the above extension index (get file extension)
        var file = path.Substring(index);
    
        // Now check the extension we have to determine if it's a script file, if not, do nothing
        if (file != ".cs" && file != ".js" && file != ".boo")
            return;

        // "Application.dataPath" gives us "<path to project folder>/Assets"
        // We find the start index of the "Assets" folder, we will use it to get the full name of the script file we've created
        index = Application.dataPath.LastIndexOf("Assets");

        // Get the absolute path to the created script so we can feed it into a ReadAllText (see the next code line)
        // Before this, the path is "Assets/ExampleScript.cs"
        // It becomes, "DRIVE LETTER:/Projects/YourProject/src/Assets/ExampleScript.cs" in my case, i.e. becomes absolute
        path = Application.dataPath.Substring(0, index) + path;

        file = Template;

        // Now we replace any amount of custom keywords we want. These should match the ones in your default script template, otherwise it's pointless
        for (int i = 0; i < ToReplace.Count; i++) {
            file = file.Replace(ToReplace[i].find, ToReplace[i].replace);
        }
        // We read the script into a string, changed our keywords, now we write the modified version back into the script file

        if (bRemoveEditor) {
            file = file.Replace("using UnityEditor;", "using UnityEngine.Networking;");
            file = file.Replace("MonoBehaviour", "MonoBehaviour"); //NetworkBehaviour !!
        } else {
            file = file.Replace("using UnityEngine;", "");
        }
        System.IO.File.WriteAllText(path, file);

        // Refresh the unity asset database to trigger a compilation of our changes on the script
        AssetDatabase.Refresh();
    }
}