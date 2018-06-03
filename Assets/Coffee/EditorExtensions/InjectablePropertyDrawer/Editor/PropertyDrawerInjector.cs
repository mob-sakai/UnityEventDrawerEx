using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Coffee.EditorExtensions
{
	static class PropertyDrawerInjector
	{
		const BindingFlags bfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		static readonly Type typeScriptAttributeUtility = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
		static readonly Type typeDrawerKeySet = Type.GetType("UnityEditor.ScriptAttributeUtility+DrawerKeySet, UnityEditor");
		static readonly MethodInfo miBuildDrawerTypeForTypeDictionary = typeScriptAttributeUtility.GetMethod("BuildDrawerTypeForTypeDictionary", bfAll);
		static readonly FieldInfo fiDrawerTypeForType = typeScriptAttributeUtility.GetField("s_DrawerTypeForType", bfAll);

		/// <summary>
		/// Inject property drawer on load method.
		/// </summary>
		[UnityEditor.InitializeOnLoadMethod]
		static void InjectPropertyDrawer()
		{
			// Get [Type -> DrawerType] dictionary. 
			IDictionary dicDrawerTypeForType = fiDrawerTypeForType.GetValue(null) as IDictionary;
			if (dicDrawerTypeForType == null)
			{
				miBuildDrawerTypeForTypeDictionary.Invoke(null, new object[0]);
				dicDrawerTypeForType = fiDrawerTypeForType.GetValue(null) as IDictionary;
			}

			// Get all types in current domain.
			Type[] loadedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();

			// Find all drawers.
			foreach (var drawerType in loadedTypes.Where(x => x.IsSubclassOf(typeof(GUIDrawer))))
			{
				// Find all InjectablePropertyDrawer attributes.
				object[] attrs = drawerType.GetCustomAttributes(typeof(InjectablePropertyDrawer), true);
				foreach (InjectablePropertyDrawer attr in attrs)
				{
					// Inject drawer type.
					InjectPropertyDrawer(loadedTypes, dicDrawerTypeForType, drawerType, attr);
				}
			}
		}

		/// <summary>
		/// Inject property drawer.
		/// </summary>
		/// <param name="types">All types in current domain.</param>
		/// <param name="dic">[Type -> DrawerType] dictionary.</param>
		/// <param name="drawerType">CustomPropertyDrawer type.</param>
		/// <param name="attr">InjectablePropertyDrawer attribute.</param>
		static void InjectPropertyDrawer(Type[] types, IDictionary dic, Type drawerType, InjectablePropertyDrawer attr)
		{
			// Create drawer key set.
			object keyset = Activator.CreateInstance(typeDrawerKeySet);
			typeDrawerKeySet.GetField("drawer", bfAll).SetValue(keyset, drawerType);
			typeDrawerKeySet.GetField("type", bfAll).SetValue(keyset, attr.type);

			// Inject drawer type.
			dic[attr.type] = keyset;

			// Inject drawer type for subclass.
			if (attr.useForChildren)
			{
				foreach (var type in types.Where(x => x.IsSubclassOf(attr.type)))
				{
					dic[type] = keyset;
				}
			}
		}
	}
}