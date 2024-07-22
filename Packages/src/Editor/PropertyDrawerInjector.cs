#if !UNITY_2022_3_OR_NEWER
#elif UNITY_2022_3_0 || UNITY_2022_3_1 || UNITY_2022_3_2 || UNITY_2022_3_3 || UNITY_2022_3_4 || UNITY_2022_3_5 || UNITY_2022_3_6 || UNITY_2022_3_7 || UNITY_2022_3_8 || UNITY_2022_3_9
#elif UNITY_2022_3_10 || UNITY_2022_3_11 || UNITY_2022_3_12 || UNITY_2022_3_13 || UNITY_2022_3_14 || UNITY_2022_3_15 || UNITY_2022_3_16 || UNITY_2022_3_17 || UNITY_2022_3_18 || UNITY_2022_3_19
#elif UNITY_2022_3_20 || UNITY_2022_3_21 || UNITY_2022_3_22
#elif UNITY_2023_1
#elif UNITY_2023_2_0 || UNITY_2023_2_1 || UNITY_2023_2_2 || UNITY_2023_2_3 || UNITY_2023_2_4 || UNITY_2023_2_5 || UNITY_2023_2_6 || UNITY_2023_2_7 || UNITY_2023_2_8 || UNITY_2023_2_9
#elif UNITY_2023_2_10 || UNITY_2023_2_11 || UNITY_2023_2_12 || UNITY_2023_2_13 || UNITY_2023_2_14
#else
#define CUSTOM_PROPERTY_DRAWER_V2
#endif

#if !CUSTOM_PROPERTY_DRAWER_V2
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Coffee.EditorExtensions
{
    public static class PropertyDrawerInjector
    {
        private const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        private static readonly Type s_TypeScriptAttributeUtility = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
        private static readonly Type s_TypeDrawerKeySet = Type.GetType("UnityEditor.ScriptAttributeUtility+DrawerKeySet, UnityEditor");
        private static readonly MethodInfo s_MiBuildDrawerTypeForTypeDictionary = s_TypeScriptAttributeUtility.GetMethod("BuildDrawerTypeForTypeDictionary", kBfAll);
        private static readonly FieldInfo s_FiDrawerTypeForType = s_TypeScriptAttributeUtility.GetField("s_DrawerTypeForType", kBfAll);
        private static readonly FieldInfo s_FiDrawer = s_TypeDrawerKeySet.GetField("drawer", kBfAll);
        private static readonly FieldInfo s_FiType = s_TypeDrawerKeySet.GetField("type", kBfAll);
        private static IDictionary s_DicDrawerTypeForType;
        private static Type[] s_LoadedTypes;

        /// <summary>
        /// Gets [Type -> DrawerType] dictionary.
        /// </summary>
        public static IDictionary drawerTypeForType
        {
            get
            {
                if (s_DicDrawerTypeForType != null) return s_DicDrawerTypeForType;

                // Get [Type -> DrawerType] dictionary from ScriptAttributeUtility class.
                s_DicDrawerTypeForType = s_FiDrawerTypeForType.GetValue(null) as IDictionary;
                if (s_DicDrawerTypeForType != null) return s_DicDrawerTypeForType;

                s_MiBuildDrawerTypeForTypeDictionary.Invoke(null, new object[0]);
                s_DicDrawerTypeForType = s_FiDrawerTypeForType.GetValue(null) as IDictionary;
                return s_DicDrawerTypeForType;
            }
        }

        /// <summary>
        /// Gets all loaded types in current domain.
        /// </summary>
        public static Type[] loadedTypes
        {
            get
            {
                if (s_LoadedTypes != null) return s_LoadedTypes;

                s_LoadedTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .ToArray();
                return s_LoadedTypes;
            }
        }

        /// <summary>
        /// Inject property drawer on load method.
        /// </summary>
        [UnityEditor.InitializeOnLoadMethod]
        public static void InjectPropertyDrawer()
        {
            // Find all drawers.
            foreach (var drawerType in loadedTypes.Where(x => x.IsSubclassOf(typeof(GUIDrawer))))
            {
                // Find all InjectablePropertyDrawer attributes.
                var attrs = drawerType.GetCustomAttributes(typeof(InjectablePropertyDrawer), true);
                foreach (InjectablePropertyDrawer attr in attrs)
                {
                    // Inject drawer type.
                    InjectPropertyDrawer(drawerType, attr);
                }
            }
        }

        /// <summary>
        /// Inject property drawer.
        /// </summary>
        /// <param name="drawerType">CustomPropertyDrawer type.</param>
        /// <param name="attr">InjectablePropertyDrawer attribute.</param>
        public static void InjectPropertyDrawer(Type drawerType, InjectablePropertyDrawer attr)
        {
            // Create drawer key set.
            var keyset = Activator.CreateInstance(s_TypeDrawerKeySet);
            s_FiDrawer.SetValue(keyset, drawerType);
            s_FiType.SetValue(keyset, attr.type);

            // Inject drawer type.
            drawerTypeForType[attr.type] = keyset;

            if (!attr.useForChildren) return;

            // Inject drawer type for subclass.
            foreach (var type in loadedTypes.Where(x => x.IsSubclassOf(attr.type)))
            {
                drawerTypeForType[type] = keyset;
            }
        }

        /// <summary>
        /// Gets the drawer type for type.
        /// </summary>
        /// <returns>The drawer type.</returns>
        /// <param name="type">The type.</param>
        public static Type GetDrawerType(Type type)
        {
            return drawerTypeForType.Contains(type)
                ? s_FiDrawer.GetValue(drawerTypeForType[type]) as Type
                : null;
        }
    }
}
#endif
