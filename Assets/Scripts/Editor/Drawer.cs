using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Text;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneDrawer : PropertyDrawer
{
    string[] sceneName;
    [SerializeField]
    int value;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SceneNameAttribute attr = this.attribute as SceneNameAttribute;
        sceneName = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            sceneName[i] = SceneManager.GetSceneByBuildIndex(i).name;
        }
        //EditorGUI.PropertyField(position, property, new GUIContent(attr.name));
        Rect left = new Rect(position.center - new Vector2(position.size.x / 2, 0), new Vector2(position.size.x / 2, position.size.y));
        Rect right = new Rect(position.center, new Vector2(position.size.x / 2, position.size.y));
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.Popup(right, property.intValue, sceneName);
            EditorGUI.LabelField(left, property.name);
        }
        else if(property.propertyType == SerializedPropertyType.String)
        {
            int index = 0;
            if (property.stringValue != null)
            {
                index = GetSceneIndexByName(property.stringValue);
            }
            index = EditorGUI.Popup(right, index, sceneName);
            property.stringValue = sceneName[index];
            EditorGUI.LabelField(left, property.name);
        }
        else
        {
            Debug.Log("Error");
        }
    }
    int GetSceneIndexByName(string name)
    {
        if (name == "")
            return 0;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (name == SceneManager.GetSceneByBuildIndex(i).name)
            {
                return i;
            }
        }
        Debug.LogError(name);
        return -1;
    }
}


