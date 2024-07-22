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

namespace Coffee.EditorExtensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class InjectablePropertyDrawer : Attribute
    {
        public Type type;
        public bool useForChildren;

        /// <summary>
        /// Tells a PropertyDrawer or DecoratorDrawer class which run-time class or attribute it's a drawer for.
        /// </summary>
        /// <param name="type">If the drawer is for a custom Serializable class, the type should be that class. If the drawer is for script variables with a specific PropertyAttribute, the type should be that atribute.</param>
        /// <param name="useForChildren">If true, the drawer will be used for any children of the specified class unless they define their own drawer.</param>
        public InjectablePropertyDrawer(Type type, bool useForChildren)
        {
            this.type = type;
            this.useForChildren = useForChildren;
        }
    }
}
#endif
