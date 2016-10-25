using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Text;

[CustomEditor (typeof(ScreenManager))]
public class ScreenManagerEditor : Editor {

	private ReorderableList list;

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();
		list.DoLayoutList ();
		serializedObject.ApplyModifiedProperties ();

		//In case the order of screens change in ScreenManager -> Regenerate enum class
		if (GUILayout.Button ("Generate Screen Enums")) {
			var screens = ((ScreenManager)target).screens;
			var total = screens.Length;

			var sb = new StringBuilder ();
			sb.Append ("public enum Screens{");
			sb.Append ("None,");
			for (int i = 0; i < total; i++) {
				sb.Append (screens [i].name.Replace (" ", ""));
				if (i < total - 1) {
					sb.Append (",");
				}
			}

			sb.Append ("}");
			var path = EditorUtility.SaveFilePanel ("Save the Screen enums", "", "ScreenEnums.cs", "cs");

			using (FileStream fs = new FileStream (path, FileMode.Create)) {
				using (StreamWriter sw = new StreamWriter (fs)) {
					sw.Write (sb.ToString ());
				}
			}
			AssetDatabase.Refresh ();
		}
	}

	private void OnEnable () {
		list = new ReorderableList (serializedObject, serializedObject.FindProperty ("screens"), true, true, true, true);

		//callbacks to know how to render
		list.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField (rect, "Screens");
		};

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex (index);
			EditorGUI.PropertyField (new Rect (rect.x, rect.y, Screen.width - 75, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
		};
	}


}
