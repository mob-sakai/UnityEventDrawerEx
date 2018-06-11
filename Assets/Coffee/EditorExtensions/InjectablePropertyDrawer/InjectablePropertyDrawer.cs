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