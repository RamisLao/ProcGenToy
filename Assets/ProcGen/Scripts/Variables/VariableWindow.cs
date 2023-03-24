using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class VariableWindow : EditorWindow
{
    private string _uppercase;
    private string _type;
    private string _path = "Assets/Scripts/Helpers/Custom/Variables/VariableTypes/";

    [MenuItem("Custom/Variable/Create New")]
    public static void ShowWindow()
    {
        GetWindow(typeof(VariableWindow));
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Enter a valid C# type with no spaces");
        string input = EditorGUILayout.TextField("Type", _type);
        if (input != null) _type = input.Trim();
        EditorGUILayout.LabelField("Enter path to Variable scripts or use default");
        string path = EditorGUILayout.TextField("Path", _path);
        if (path != null) _path = path.Trim();

        if(GUILayout.Button("Generate"))
        {
            _uppercase = StringFormatting.FirstLetterToUpper(_type);
            CreateVariable();
            CreateVariableListener();
            CreateAddToVariable();
            AssetDatabase.Refresh();
        }
    }

    private void CreateVariable()
    {
        string path = $"{_path}Variable{_uppercase}.cs";
        Debug.Log($"Creating Variable: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("");
            outfile.WriteLine($"[CreateAssetMenu(menuName = \"Variables/{_type}\")]");
            outfile.WriteLine($"public class Variable{_uppercase} : Variable<{_type}> {{}}");
        }
    }

    private void CreateVariableListener()
    {
        string path = $"{_path}VariableListener{_uppercase}.cs";
        Debug.Log($"Creating VariableListener: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("");
            outfile.WriteLine($"public class VariableListener{_uppercase} : VariableListener<{_type}> {{}}");
        }
    }

    private void CreateAddToVariable()
    {
        string path = $"{_path}AddToVariable{_uppercase}.cs";
        Debug.Log($"Creating AddToVariable: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("");
            outfile.WriteLine($"public class AddToVariable{_uppercase} : AddToVariable<{_type}> {{}}");
        }
    }
}
#endif