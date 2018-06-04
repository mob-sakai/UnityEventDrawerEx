using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Text;

namespace Coffee.EditorExtensions.Test
{
	public class PropertyDrawerInjectorTest
	{
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
	}
}