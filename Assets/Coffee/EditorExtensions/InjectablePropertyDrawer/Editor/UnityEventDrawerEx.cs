using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Diagnostics;

namespace Coffee.EditorExtensions
{
	[InjectablePropertyDrawer(typeof(UnityEventBase), true)]
	public class UnityEventDrawerEx : UnityEventDrawer
	{
		const BindingFlags bfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		static readonly FieldInfo fiReorderableList = typeof(UnityEventDrawer).GetField("m_ReorderableList", bfAll);
		static readonly FieldInfo fiCalls = typeof(UnityEventBase).GetField("m_Calls", bfAll);
		static readonly FieldInfo fiRuntimeCalls = Type.GetType("UnityEngine.Events.InvokableCallList, UnityEngine").GetField("m_RuntimeCalls", bfAll);
		static GUIStyle styleToggle;
		static GUIStyle styleBg;

		static bool foldStatus { get { return UnityEventDrawerExSettings.instance.foldStatus; } set { UnityEventDrawerExSettings.instance.foldStatus = value; } }

		static readonly Dictionary<Type, FieldInfo> fiDelegateMap = new Dictionary<Type, FieldInfo>();

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			ReorderableList ro = fiReorderableList.GetValue(this) as ReorderableList;
			if (ro == null)
			{
				base.GetPropertyHeight(property, label);
				ro = fiReorderableList.GetValue(this) as ReorderableList;
			}

			bool isEmpty = property.FindPropertyRelative("m_PersistentCalls").FindPropertyRelative("m_Calls").arraySize == 0;
			ro.elementHeight = isEmpty
				? 16
				: 16 * 2 + 11;

			return foldStatus
				? base.GetPropertyHeight(property, label) + GetRuntimeCalls(property).Count * 17
				: base.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var RuntimeCalls = GetRuntimeCalls(property);
			float height = foldStatus
				? RuntimeCalls.Count * 17 + 16
				: 16;
			var r = new Rect(position.x + 2, position.y + position.height - height, position.width - 4, height);
			DrawRuntimeCallToggle(r, RuntimeCalls.Count);

			base.OnGUI(position, property, label);

			if (foldStatus)
			{
				r = new Rect(r.x + 16, r.y + 15, r.width - 16, 16);
				EditorStyles.objectField.fontSize = 9;
				foreach (var c in RuntimeCalls)
				{
					DrawDelegate(r, GetDelegate(c));
					r.y += r.height + 1;
				}
				EditorStyles.objectField.fontSize = 11;
			}
		}

		static Delegate GetDelegate(object invokableCall)
		{
			Type type = invokableCall.GetType();
			FieldInfo fiDelegate;
			if (!fiDelegateMap.TryGetValue(type, out fiDelegate))
			{
				fiDelegate = type.GetField("Delegate", bfAll);
				fiDelegateMap.Add(type, fiDelegate);
			}

			return fiDelegate.GetValue(invokableCall) as Delegate;
		}

		static void DrawRuntimeCallToggle(Rect rect, int count)
		{
			if (styleBg == null)
			{
				styleBg = new GUIStyle("ProgressBarBack");
				styleToggle = new GUIStyle("OL Toggle") { fontSize = 9 };
			}

			GUI.Label(rect, "", styleBg);
			string text = string.Format("Show runtime calls ({0})", count);
			rect.width -= 80;
			rect.height = 14;
			foldStatus = GUI.Toggle(rect, foldStatus, text, styleToggle);
		}

		static void DrawDelegate(Rect rect, Delegate del)
		{
			Rect r = new Rect(rect.x, rect.y, rect.width * 0.3f, rect.height);
			try
			{
				MethodInfo method = del.Method;
				object target = del.Target;

				var obj = target as UnityEngine.Object;
				if (obj)
				{
					EditorGUI.ObjectField(r, obj, obj.GetType(), true);
				}
				else if (target != null)
				{
					EditorGUI.LabelField(r, string.Format("{0} ({1})", target.ToString(), target.GetType()), EditorStyles.miniLabel);
				}
				else
				{
					EditorGUI.LabelField(r, "null", EditorStyles.miniLabel);
				}

				r.x += r.width;
				r.width = rect.width - r.width;
				EditorGUI.LabelField(r, method.ReflectedType + "." + method.Name, EditorStyles.miniLabel);
			}
			catch
			{
				EditorGUI.LabelField(rect, "null delegate", EditorStyles.miniLabel);
			}
		}

		public static IList GetRuntimeCalls(SerializedProperty property)
		{
			var instance = property.serializedObject.targetObject;
			Type type = instance.GetType();
			FieldInfo fiProperty = null;
			while (type != null && fiProperty == null)
			{
				fiProperty = type.GetField(property.name, bfAll);
				type = type.BaseType;
			}

			return fiRuntimeCalls.GetValue(fiCalls.GetValue(fiProperty.GetValue(instance))) as IList;
		}

	}

	[Serializable]
	public class UnityEventDrawerExSettings : ScriptableSingleton<UnityEventDrawerExSettings>
	{
		public bool foldStatus = true;
	}
}
