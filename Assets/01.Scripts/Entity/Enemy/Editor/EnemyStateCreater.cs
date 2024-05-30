using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;

public class EnemyStateCreater : EditorWindow
{
    const string stateString = "State";

    private string _enemyName;
    private List<string> _stateNames = new List<string>() { "Idle", "Battle", "Attack", "Stuned", "Dead" };
    private Regex _regex;

    [MenuItem("Generator/EnemyState")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EnemyStateCreater));
    }
    private void OnEnable()
    {
        _regex = new Regex(@"^[a-zA-Z]+$");
    }
    public void OnGUI()
    {
        _enemyName = EditorGUILayout.TextField("EnemyName", _enemyName);



        if (!CheckString(_enemyName))
        {
            return;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("���� � ���¸� �������?", EditorStyles.boldLabel);
        ReorderableListGUI.ListField(_stateNames, DrawListItem);
        EditorGUILayout.HelpBox("State�� �����ϰ� �Է����ּ���!", MessageType.Info);

        if (_stateNames.Count < 1)
        {
            EditorGUILayout.HelpBox("��밡 �ƹ� ���� ������ ���ݾƿ�!", MessageType.Warning);
            return;
        }
        foreach (var states in _stateNames)
        {
            if (!CheckString(states))
            {
                return;
            }
        }

        GUILayout.Space(30);
        if (GUILayout.Button("Create"))
        {
            string enemyPath = Path.Combine(Application.dataPath, "01.Scripts", "Enemy", $"{_enemyName}");
            string scriptPath = Path.Combine(enemyPath, $"{_enemyName}.cs");
            if (!File.Exists(scriptPath))
            {
                MakeScript(scriptPath);
            }
            foreach (var enumName in _stateNames)
            {
                string enumPath = Path.Combine(enemyPath, stateString, $"{_enemyName}{enumName}State.cs");
                if (!File.Exists(enumPath))
                {
                    MakeState(enumName,enumPath);
                }
            }
            AssetDatabase.Refresh();
        }
    }
    private void MakeState(string enumName,string statePath)
    {
        string template = string.Empty;
        string templatePath = Path.Combine(Application.dataPath, "01.Scripts", "Enemy", "Editor", "EnemyState.template");
        try
        {
            using (FileStream readStream = new FileStream(templatePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(readStream))
                {
                    StringBuilder script = new StringBuilder(reader.ReadToEnd());
                    script = script.Replace("{Name}", _enemyName);
                    template = script.Replace("{Enum}", enumName).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("�Ƹ� ���� ã�µ� ������ �ֳ�����..");
        }
        Directory.CreateDirectory(Path.GetDirectoryName(statePath));
        using (FileStream writeStream = new FileStream(statePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                writer.Write(template);
            }
        }
    }
    private void MakeScript(string scriptPath)
    {
        string template = string.Empty;
        string templatePath = Path.Combine(Application.dataPath, "01.Scripts", "Enemy", "Editor", "EnemyScript.template");
        try
        {
            using (FileStream readStream = new FileStream(templatePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(readStream))
                {
                    StringBuilder script = new StringBuilder(reader.ReadToEnd());
                    template = script.Replace("{Name}", _enemyName).ToString(); //������ �� �б�
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("�Ƹ� ���� ã�µ� ������ �ֳ�����..");
        }

        template = AddEnum(template);
        Directory.CreateDirectory(Path.GetDirectoryName(scriptPath));
        using (FileStream writeStream = new FileStream(scriptPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                writer.Write(template);
            }
        }
    }
    private string AddEnum(string scriptStr)
    {
        StringBuilder start = new StringBuilder(_enemyName);
        start.Append("StateEnum{");

        StringBuilder enumBuilds = new StringBuilder(string.Empty);
        foreach (string str in _stateNames)
        {
            enumBuilds.Append(str);
            enumBuilds.Append(", ");
        }


        StringBuilder script = new StringBuilder(scriptStr);
        script.Insert(scriptStr.IndexOf(start.ToString()) + start.Length, enumBuilds.ToString());

        return script.ToString();
    }
    private bool CheckString(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            EditorGUILayout.HelpBox("���� ����־��!", MessageType.Warning);
            return false;
        }
        if (!_regex.IsMatch(str))
        {
            EditorGUILayout.HelpBox("Ư�����ڿ� ���ڴ� �ʿ�����ʳ���?!", MessageType.Warning);
            return false;
        }

        return true;
    }
    private string DrawListItem(Rect position, string value)
    {
        // Text fields do not like null values!
        if (value == null)
            value = "";
        return EditorGUI.TextField(position, value);
    }
}
