using System;
using System.Reflection;
using System.Collections.Generic;

#if !STANDALONE
using UnityEngine;
#endif

namespace TNet
{
	/// <summary>
	/// Convenient wrapper for a getter and setter of a field or property on the chosen object.
	/// </summary>

	public class FieldOrProperty
	{
		[NonSerialized] public FieldInfo field;
		[NonSerialized] public PropertyInfo property;

		/// <summary>
		/// Field or property's name.
		/// </summary>

		public string name
		{
			get
			{
				if (field != null) return field.Name;
				if (property != null) return property.Name;
				return "Unnamed";
			}
		}

		/// <summary>
		/// Whether this option is writable or read-only.
		/// </summary>

		public bool canWrite
		{
			get
			{
				if (field != null) return true;
				if (property != null && property.CanWrite) return true;
				return false;
			}
		}

		/// <summary>
		/// Field or property type.
		/// </summary>

		public Type type
		{
			get
			{
				if (field != null) return field.FieldType;
				if (property != null) return property.PropertyType;
				return typeof(void);
			}
		}

		/// <summary>
		/// Get the field or property's value.
		/// </summary>

		public object GetValue (object target)
		{
			if (field != null) return field.GetValue(target);
			if (property != null) return property.GetValue(target, null);
			return null;
		}

		/// <summary>
		/// Get the field or property's value.
		/// </summary>

		public T GetValue<T> (object target) { return Serialization.Convert<T>(GetValue(target)); }

		/// <summary>
		/// Set the field or property's value.
		/// </summary>

#if STANDALONE
		public void SetValue (object target, object value, object go = null)
#else
		public void SetValue (object target, object value, GameObject go = null)
#endif
		{
#if !UNITY_EDITOR
			try
#endif
			{
				if (field != null)
				{
					var resolved = Serialization.ConvertObject(value, field.FieldType, go);

					if (resolved != null)
					{
						field.SetValue(target, resolved);
					}
#if !STANDALONE
					else if (value is int && (field.FieldType == typeof(GameObject) || typeof(Component).IsAssignableFrom(field.FieldType)))
					{
						ComponentSerialization.AddLazyRef((int)value, target, field, go);
					}
#endif
				}
				else if (property != null && property.CanWrite)
				{
					var resolved = Serialization.ConvertObject(value, property.PropertyType, go);

					if (resolved != null)
					{
						property.SetValue(target, resolved, null);
					}
#if !STANDALONE
					else if (value is int && property.PropertyType == typeof(GameObject) || typeof(Component).IsAssignableFrom(property.PropertyType))
					{
						ComponentSerialization.AddLazyRef((int)value, target, property, go);
					}
#endif
				}
#if UNITY_EDITOR
				else Debug.LogError("No property or field to set");
#endif
			}
#if !UNITY_EDITOR
			catch (Exception ex) { Tools.LogError(ex.GetType() + ": " + ex.Message); }
#endif
		}

		/// <summary>
		/// Whether the specified attribute is present.
		/// </summary>

		public bool HasAttribute<T> () where T : Attribute
		{
			if (field != null)
			{
				return field.IsDefined(typeof(T), true);
			}
			else if (property != null)
			{
				return property.IsDefined(typeof(T), true);
			}
			return false;
		}

		/// <summary>
		/// Retrieve the attribute of specified type on the field or property.
		/// </summary>

		public T GetAttribute<T> () where T : Attribute
		{
			if (field != null)
			{
				if (field.IsDefined(typeof(T), true))
					return (T)field.GetCustomAttributes(typeof(T), true)[0];
			}
			else if (property != null)
			{
				if (property.IsDefined(typeof(T), true))
					return (T)property.GetCustomAttributes(typeof(T), true)[0];
			}
			return null;
		}

		/// <summary>
		/// Retrieve the attributes of specified type on the field or property.
		/// </summary>

		public T[] GetAttributes<T> () where T : Attribute
		{
			if (field != null)
			{
				if (field.IsDefined(typeof(T), true))
					return (T[])field.GetCustomAttributes(typeof(T), true);
			}
			else if (property != null)
			{
				if (property.IsDefined(typeof(T), true))
					return (T[])property.GetCustomAttributes(typeof(T), true);
			}
			return null;
		}

		/// <summary>
		/// Create a new field or property reference of specified name.
		/// </summary>

		static public FieldOrProperty Create (Type type, string name)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var field = type.GetField(name, flags);

			if (field != null)
			{
				var fp = new FieldOrProperty();
				fp.field = field;
				return fp;
			}

			var property = type.GetProperty(name, flags);

			if (property != null)
			{
				var fp = new FieldOrProperty();
				fp.property = property;
				return fp;
			}

#if UNITY_EDITOR
#if TNET_EXCEPTIONS
			throw new Exception("Unable to find " + type + "." + name);
#else
			Debug.LogWarning("Unable to find " + type + "." + name);
			return null;
#endif
#else
		return null;
#endif
		}

		/// <summary>
		/// Create a new field or property reference of specified name.
		/// </summary>

		static public FieldOrProperty Create (Type type, FieldInfo field)
		{
			if (field != null)
			{
				var fp = new FieldOrProperty();
				fp.field = field;
				return fp;
			}
			return null;
		}

		/// <summary>
		/// Create a new field or property reference of specified name.
		/// </summary>

		static public FieldOrProperty Create (Type type, PropertyInfo property)
		{
			if (property != null)
			{
				var fp = new FieldOrProperty();
				fp.property = property;
				return fp;
			}
			return null;
		}

		/// <summary>
		/// Compares the referenced field and property.
		/// </summary>

		static public bool Matches (FieldOrProperty a, FieldOrProperty b)
		{
			if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
			if (ReferenceEquals(b, null)) return ReferenceEquals(a, null);
			return a.field == b.field && a.property == b.property;
		}

		static public bool operator == (FieldOrProperty a, FieldOrProperty b) { return Matches(a, b); }
		static public bool operator != (FieldOrProperty a, FieldOrProperty b) { return !Matches(a, b); }

		public override bool Equals (object obj) { return Matches(this, obj as FieldOrProperty); }

		public override int GetHashCode ()
		{
			if (field != null) return field.GetHashCode();
			if (property != null) return property.GetHashCode();
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// Convenience extension methods for the TNet.Property.
	/// </summary>

	static public class FieldOrPropertyExtensions
	{
		static Dictionary<Type, Dictionary<string, FieldOrProperty>> mFoPs = new Dictionary<Type, Dictionary<string, FieldOrProperty>>();

		/// <summary>
		/// Get the specified field or property. The result is cached in a lookup table.
		/// </summary>

		static public FieldOrProperty GetFieldOrProperty (this object obj, string name)
		{
			var type = obj.GetType();
			Dictionary<string, FieldOrProperty> dict;

			if (!mFoPs.TryGetValue(type, out dict))
			{
				dict = new Dictionary<string, FieldOrProperty>();
				mFoPs[type] = dict;
			}

			FieldOrProperty fp = null;

			if (!dict.TryGetValue(name, out fp))
			{
				fp = FieldOrProperty.Create(type, name);
				dict[name] = fp;
			}
			return fp;
		}

		/// <summary>
		/// Get the specified field or property. The result is cached in a lookup table.
		/// </summary>

		static public FieldOrProperty GetFieldOrProperty (this Type type, string name)
		{
			Dictionary<string, FieldOrProperty> dict;

			if (!mFoPs.TryGetValue(type, out dict))
			{
				dict = new Dictionary<string, FieldOrProperty>();
				mFoPs[type] = dict;
			}

			FieldOrProperty fp = null;

			if (!dict.TryGetValue(name, out fp))
			{
				fp = FieldOrProperty.Create(type, name);
				dict[name] = fp;
			}
			return fp;
		}

		/// <summary>
		/// Get the value of a field or property of an object.
		/// </summary>

		static public object GetFieldOrPropertyValue (this object obj, string name)
		{
			var fp = obj.GetFieldOrProperty(name);
			return (fp != null) ? fp.GetValue(obj) : null;
		}

		/// <summary>
		/// Get the value of a field or property of an object.
		/// </summary>

		static public object GetFieldOrPropertyValue (this object obj, string name, object defaultVal)
		{
			var fp = obj.GetFieldOrProperty(name);
			return (fp != null) ? fp.GetValue(obj) : defaultVal;
		}

		/// <summary>
		/// Get the specified field or property of an object, cast into the chosen type using TNet's serialization.
		/// </summary>

		static public T GetFieldOrPropertyValue<T> (this object obj, string name)
		{
			return Serialization.Convert<T>(obj.GetFieldOrPropertyValue(name));
		}

		/// <summary>
		/// Get the specified field or property of an object, cast into the chosen type using TNet's serialization.
		/// </summary>

		static public T GetFieldOrPropertyValue<T> (this object obj, string name, T defaultVal)
		{
			var fp = obj.GetFieldOrProperty(name);

			if (fp != null)
			{
				object val = fp.GetValue(obj);
				return Serialization.Convert<T>(val);
			}
			return defaultVal;
		}

		/// <summary>
		/// Set the value of a field or property of an object.
		/// </summary>

#if STANDALONE
		static public void SetFieldOrPropertyValue (this object obj, string name, object val, object go = null)
		{
			var fp = obj.GetFieldOrProperty(name);
			if (fp != null) fp.SetValue(obj, val);
		}
#else
		static public void SetFieldOrPropertyValue (this object obj, string name, object val, GameObject go = null)
		{
			var fp = obj.GetFieldOrProperty(name);
			if (fp != null) fp.SetValue(obj, val, go);
			else Debug.LogWarning("Unable to find " + obj.GetType() + "." + name);
		}
#endif
	}
}
