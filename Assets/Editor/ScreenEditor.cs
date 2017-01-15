using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ScreenManager))]
public class ScreenEditor : Editor {

	private ReorderableList list;

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		list.DoLayoutList ();

		serializedObject.ApplyModifiedProperties ();
	}

	//gets the array from the ScreenManager called screens
	private void OnEnable() {
		list = new ReorderableList (serializedObject, serializedObject.FindProperty ("screens"), true, true, true, true);

		// Change the header from default "Properties" to "Windows"
		list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Windows"); };

		//Change the name of the elements from "Element 0, 1 etc." to the actual elements type
		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			//get the element at the index in the array of the serializedproperty = screens
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			// typing the elements to the actualy types they are -> BaseScreen
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, Screen.width - 75, EditorGUIUtility.singleLineHeight), element, GUIContent.none); 
		};
	}
}
