using System;
using System.Reflection;
using System.Linq;

namespace Hardly {
	public static class SoberTypes {
		public static Type[] GetAllSubclassesInThisAssumbly<BaseType>(this Type assemblyWithThisType, bool inSameNamespace) {
			return Assembly.GetAssembly(assemblyWithThisType).GetTypes().Where(t => !t.IsAbstract && typeof(BaseType).IsAssignableFrom(t) && (!inSameNamespace || t.Namespace.Equals(assemblyWithThisType.Namespace))).ToArray<Type>();
		}

		public static object GetDefaultValue(this Type type) {
			if(!type.IsValueType) {
				return null;
			} else {
				try {
					return Activator.CreateInstance(type);
				} catch(Exception e) {
					Log.error("Error getting default value", e);

					return null;
				}
			}
		}

		public static BaseType[] InstantiateEachSubclassWithDefaultConstructor<BaseType>(this Type assemblyWithThisType, bool inSameNamespace) {
			Type[] subclassTypes = assemblyWithThisType.GetAllSubclassesInThisAssumbly<BaseType>(inSameNamespace);
			if(subclassTypes != null) {
				BaseType[] subclassObjects = new BaseType[subclassTypes.Length];
				for(int i = 0; i < subclassTypes.Length; i++) {
					subclassObjects[i] = (BaseType)subclassTypes[i].GetConstructor(new Type[] { })?.Invoke(new object[] { });
				}

				return subclassObjects;
			}

			return null;
		}

		public static BaseType[] InstantiateEachSubclassInMyAssembly<BaseType, ArgType>(this Type assemblyWithThisType, bool inSameNamespace, ArgType arg) {
			Type[] subclassTypes = assemblyWithThisType.GetAllSubclassesInThisAssumbly<BaseType>(inSameNamespace);
			if(subclassTypes != null) {
				BaseType[] subclassObjects = new BaseType[subclassTypes.Length];
				for(int i = 0; i < subclassTypes.Length; i++) {
					subclassObjects[i] = (BaseType)subclassTypes[i].GetConstructor(new[] { typeof(ArgType) }).Invoke(new object[] { arg });
				}

				return subclassObjects;
			}

			return null;
		}

		public static bool IsDefaultValue(this object value) {
			return value == null
					  || value.Equals(value.GetType().GetDefaultValue()) || value.Equals("") || value.Equals(DBNull.Value);
		}

		public static bool IsWholeNumber(this Type type) {
			return type.Equals(typeof(int)) || type.Equals(typeof(uint)) || type.Equals(typeof(long)) || type.Equals(typeof(ulong)) || type.Equals(typeof(short)) || type.Equals(typeof(ushort)) || type.Equals(typeof(byte)) || type.Equals(typeof(sbyte));
		}

		public static T FromSql<T>(this object var) {
			return (T)FromSql(typeof(T), var);
		}

		public static object FromSql(Type t, object var) {
			if(var == null || var.GetType().Equals(typeof(DBNull))) {
				return t.GetDefaultValue();
			} else if(t.Equals(var.GetType()) || t.IsWholeNumber() || t.Equals(typeof(byte[]))) {
				return var;
			} else if(t.Equals(typeof(bool))) {
				try {

					return (ulong)var > 0;
				} catch(Exception e ) {
					e.ToString();
					return 0;
				}
			} else {
				Log.error("Unknown type " + var.GetType());
				return var;
			}
		}
	}
}
