/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   
 Description    :   
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(MonoBehaviour), true)]
public class CustomInspectorBase : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*
         * Retrieve all public and private methods in the inspected monobehaviour.
         */
        MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < methods.Length; i++)
        {

            /*
             * Retrieve [Button] attributes attached to the method.
             */
            object[] buttonAttribute = methods[i].GetCustomAttributes(typeof(ButtonAttribute), true);

            if (buttonAttribute.Length > 0)
            {
                if (GUILayout.Button(methods[i].Name))
                {
                    ((MonoBehaviour)target).Invoke(methods[i].Name, 0f);
                }
            }
        }

        FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < fields.Length; i++)
        {
            //BoxGroupAttribute att = (BoxGroupAttribute)Attribute.GetCustomAttribute(fields[i], typeof(BoxGroupAttribute));

            //if (att != null)
            //{
            //    Debug.Log("Atttribute: " + att._header);
            //}

            /*
             * Retrieve [BoxGroup] attributes attached to the variable.
             */
            object[] boxGroupAttribute = fields[i].GetCustomAttributes(typeof(BoxGroupAttribute), true);

            if (boxGroupAttribute.Length > 0)
            {
                //Debug.Log("Name:" + fields[i].Name);

                var p = serializedObject.FindProperty(fields[i].Name);
            }
        }
    }
}
