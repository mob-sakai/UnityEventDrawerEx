using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Coffee.EditorExtensions.Test
{
	public class PropertyDrawerInjectorTest
	{
		const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

		[Test]
		public void PrintAllPropertyDrawer()
		{
			StringBuilder sb = new StringBuilder();
			var dic = PropertyDrawerInjector.drawerTypeForType;
			foreach (System.Type key in dic.Keys)
			{
				sb.AppendFormat("{0,-20} -> {1}\n", key.Name, PropertyDrawerInjector.GetDrawerType(key).Name);
			}
			Debug.Log(sb);
		}

		public static void PrintAllMembers(Type t)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("<color=green>#### All members in {0}</color>\n", t);
			while (t != null)
			{
				foreach (var m in t.GetMembers(BindingFlags.Static|BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.DeclaredOnly))
				{
					sb.AppendFormat("  - ({0,-10}) {1}\n", m.MemberType, m.Name);
				}
				t = t.BaseType;
			}

			Debug.Log(sb);
		}

		[Test]
		public void EventTriggerTest2()
		{
			GameObject go = new GameObject("test", typeof(EventTrigger));

			try
			{
				var et = go.GetComponent<EventTrigger>();
				et.triggers.Add(new EventTrigger.Entry(){ eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() });
				et.triggers.Add(new EventTrigger.Entry(){ eventID = EventTriggerType.Drag, callback = new EventTrigger.TriggerEvent() });


				SerializedProperty sp = new SerializedObject(et).FindProperty("m_Delegates").GetArrayElementAtIndex(0).FindPropertyRelative("callback");
				Debug.Log(sp.GetInstance());


				sp = new SerializedObject(et).FindProperty("m_Delegates").GetArrayElementAtIndex(1).FindPropertyRelative("callback");
				Debug.Log(sp.GetInstance());
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				UnityEngine.Object.DestroyImmediate(go);
			}
		}

		[Test]
		public void EventTriggerTest()
		{
			GameObject go = new GameObject("test", typeof(EventTrigger));
				
			try
			{
				var et = go.GetComponent<EventTrigger>();
				et.triggers.Add(new EventTrigger.Entry(){ eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() });


				SerializedProperty sp = new SerializedObject(et).FindProperty("m_Delegates").GetArrayElementAtIndex(0).FindPropertyRelative("callback");
				Func<object,object> func = GetPropertyFunc(sp);
				Debug.Log(func(sp));
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				UnityEngine.Object.DestroyImmediate(go);
			}
		}

		static readonly Regex regArray = new Regex(@"^Array\.data\[([0-9]+)\]\.!?", RegexOptions.Compiled);
		static readonly Regex regNested = new Regex(@"^(\w+)\.!?", RegexOptions.Compiled);
		static readonly object[] args = new object[1];

		static readonly Dictionary<int,Func<object,object>> map = new Dictionary<int, Func<object, object>>();

		public static Func<object,object> GetPropertyFunc(SerializedProperty property)
		{
			int hash = property.propertyPath.GetHashCode();

			Func<object,object> func = null;
			if (map.TryGetValue(hash, out func))
			{
				Debug.Log("cache hit! " + property.propertyPath);
				return func;
			}



			object instance = property.serializedObject.targetObject;
			string path = property.propertyPath;
			func = p => (p as SerializedProperty).serializedObject.targetObject;
			while (0 < path.Length)
			{
				Match match;
				if ((match = regArray.Match(path)).Success)
				{
					Debug.Log("Array!");

					path = regArray.Replace(path, "");
					args[0] = int.Parse(match.Groups[1].Value);
					var arg = new object[]{ int.Parse(match.Groups[1].Value) };
					var mi = instance.GetMemberInfo("get_Item") as MethodInfo;
					var _func = func;
					func = x => mi.Invoke(_func(x), arg);

					instance = mi.Invoke(instance, args);
				}
				else if ((match = regNested.Match(path)).Success)
				{
					Debug.Log("Sub!");

					var fi = instance.GetMemberInfo(match.Groups[1].Value) as FieldInfo;
					var _func = func;
					func = x => fi.GetValue(_func(x));

					instance = fi.GetValue(instance);
					path = regNested.Replace(path, "");
				}
				else
				{
					var fi = instance.GetMemberInfo(path) as FieldInfo;
					var _func = func;
					func = x => fi.GetValue(_func(x));
					path = "";
				}
			}
			map.Add(hash, func);
			return func;
		}

		public static FieldInfo GetFieldInfo(object target, String name)
		{
			// Find FieldInfo for the runtime call list.
			Type type = target.GetType();
			FieldInfo fiProperty = null;
			while (type != null && fiProperty == null)
			{
				fiProperty = type.GetField(name, kBfAll);
				type = type.BaseType;
			}
			return fiProperty;
		}
	}
}

public static class testing
{
	const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
	static readonly Dictionary<Type,Dictionary<string, MemberInfo>> map = new Dictionary<Type,Dictionary<string, MemberInfo>>();


	public static MemberInfo GetMemberInfo(this object target, string name)
	{
		Type type = target.GetType();
		Dictionary<string, MemberInfo> typemap = null;
		if (!map.TryGetValue(type, out typemap))
		{
			typemap = new Dictionary<string, MemberInfo>();
			map.Add(type, typemap);
		}

		MemberInfo mi = null;
		if (!typemap.TryGetValue(name, out mi))
		{
			mi = type.GetMemberInfo(name);
			typemap.Add(name, mi);
			Debug.LogFormat("{0}, {1}, {2}", type, name, mi);
		}
		return mi;
	}

	public static MemberInfo GetMemberInfo(this Type type, string name)
	{
		while (type != null)
		{
			var mis = type.GetMember(name, kBfAll);
			if (0 < mis.Length)
			{
				return mis[0];
			}
			type = type.BaseType;
		}
		return null;
	}
}
