using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Coffee.EditorExtensions
{
    public static class PropertyGetterDelegate
    {
        private const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        private static readonly Dictionary<Type, Dictionary<string, MemberInfo>> s_TypeMemberMap = new Dictionary<Type, Dictionary<string, MemberInfo>>();
        private static readonly Regex s_RegexArray = new Regex(@"^Array\.data\[([0-9]+)\](\.!?|$)", RegexOptions.Compiled);
        private static readonly Regex s_RegexNested = new Regex(@"^(\w+)\.!?", RegexOptions.Compiled);
        private static readonly Dictionary<int, Func<object, object>> s_PropertyGetterMap = new Dictionary<int, Func<object, object>>();

        /// <summary>
        /// Get the instance object of SerializedProperty.
        /// </summary>
        public static object GetInstance(this SerializedProperty property)
        {
            try
            {
                var hash = property.propertyPath.GetHashCode();

                // Find getter delegate.
                Func<object, object> getter = null;
                if (s_PropertyGetterMap.TryGetValue(hash, out getter))
                {
                    return getter(property);
                }

                // Create getter delegate.
                object instance;
                var path = property.propertyPath;
                getter = p => (p as SerializedProperty).serializedObject.targetObject;
                while (0 < path.Length)
                {
                    var preGetter = getter;
                    instance = preGetter(property);
                    Match match;

                    // Array property.
                    if ((match = s_RegexArray.Match(path)).Success)
                    {
                        var arg = new object[] {int.Parse(match.Groups[1].Value)};
                        var mi = instance.GetMemberInfo(instance is Array ? "Get" : "get_Item", MemberTypes.Method) as MethodInfo;
                        getter = x => mi.Invoke(preGetter(x), arg);
                        path = s_RegexArray.Replace(path, "");
                    }
                    // Nested property.
                    else if ((match = s_RegexNested.Match(path)).Success)
                    {
                        var fi = instance.GetMemberInfo(match.Groups[1].Value, MemberTypes.Field) as FieldInfo;
                        getter = x => fi.GetValue(preGetter(x));
                        path = s_RegexNested.Replace(path, "");
                    }
                    // Property.
                    else
                    {
                        var fi = instance.GetMemberInfo(path, MemberTypes.Field) as FieldInfo;
                        getter = x => fi.GetValue(preGetter(x));
                        break;
                    }
                }

                s_PropertyGetterMap.Add(hash, getter);

                return getter(property);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get the member info.
        /// </summary>
        public static MemberInfo GetMemberInfo(this object target, string name, MemberTypes memberType)
        {
            var type = target.GetType();
            Dictionary<string, MemberInfo> memberMap = null;
            if (!s_TypeMemberMap.TryGetValue(type, out memberMap))
            {
                memberMap = new Dictionary<string, MemberInfo>();
                s_TypeMemberMap.Add(type, memberMap);
            }

            MemberInfo mi = null;
            if (memberMap.TryGetValue(name, out mi)) return mi;

            mi = GetMemberInfo(type, name, memberType);
            memberMap.Add(name, mi);

            return mi;
        }

        /// <summary>
        /// Get the member info.
        /// </summary>
        public static MemberInfo GetMemberInfo(this Type type, string name, MemberTypes memberType)
        {
            MemberInfo mi = null;
            while (type != null && mi == null)
            {
                mi = type.GetMember(name, kBfAll).FirstOrDefault(x => x.MemberType == memberType);
                type = type.BaseType;
            }

            return mi;
        }
    }
}
