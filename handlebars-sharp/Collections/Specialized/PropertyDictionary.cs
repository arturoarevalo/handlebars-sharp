namespace Handlebars.Collections.Specialized
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Provides methods for creating instances of PropertyDictionary.
    /// </summary>
    public sealed class PropertyDictionary : IDictionary<string, object>
    {
        private static readonly Dictionary<Type, Dictionary <string, PropertyInfo>> typeCache = new Dictionary<Type, Dictionary<string, PropertyInfo>> ();

        private readonly object referencedInstance;
        private readonly Dictionary<string, PropertyInfo> referencedTypeCache;

        /// <summary>
        /// Initializes a new instance of a PropertyDictionary.
        /// </summary>
        /// <param name="instance">The instance to wrap in the PropertyDictionary.</param>
        public PropertyDictionary (object instance)
        {
            referencedInstance = instance;
            if (referencedInstance == null)
            {
                referencedTypeCache = new Dictionary<string, PropertyInfo> ();
            }
            else
            {
                referencedTypeCache = getCacheType (referencedInstance);
            }
        }

        private static Dictionary<string, PropertyInfo> getCacheType (object instance)
        {
            Type type = instance.GetType ();
            Dictionary<string, PropertyInfo> typeCache;
            if (!PropertyDictionary.typeCache.TryGetValue (type, out typeCache))
            {
                typeCache = new Dictionary<string, PropertyInfo> ();
                BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
                foreach (PropertyInfo propertyInfo in type.GetProperties (flags))
                {
                    if (!propertyInfo.IsSpecialName)
                    {
                        typeCache.Add (propertyInfo.Name, propertyInfo);
                    }
                }
                PropertyDictionary.typeCache.Add (type, typeCache);
            }
            return typeCache;
        }

        /// <summary>
        /// Gets the underlying instance.
        /// </summary>
        public object ReferencedInstance
        {
            get { return referencedInstance; }
        }

        [EditorBrowsable (EditorBrowsableState.Never)]
        void IDictionary<string, object>.Add (string key, object value)
        {
            throw new NotSupportedException ();
        }

        /// <summary>
        /// Determines whether a property with the given name exists.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <returns>True if the property exists; otherwise, false.</returns>
        public bool ContainsKey (string key)
        {
            return referencedTypeCache.ContainsKey (key);
        }

        /// <summary>
        /// Gets the name of the properties in the type.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return referencedTypeCache.Keys; }
        }

        [EditorBrowsable (EditorBrowsableState.Never)]
        bool IDictionary<string, object>.Remove (string key)
        {
            throw new NotSupportedException ();
        }

        /// <summary>
        /// Tries to get the value for the given property name.
        /// </summary>
        /// <param name="key">The name of the property to get the value for.</param>
        /// <param name="value">The variable to store the value of the property or the default value if the property is not found.</param>
        /// <returns>True if a property with the given name is found; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">The name of the property was null.</exception>
        public bool TryGetValue (string key, out object value)
        {
            PropertyInfo propertyInfo;
            if (!referencedTypeCache.TryGetValue (key, out propertyInfo))
            {
                value = null;
                return false;
            }
            value = GetValue (propertyInfo);
            return true;
        }

        /// <summary>
        /// Gets the values of all of the properties in the object.
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                ICollection<PropertyInfo> propertyInfos = referencedTypeCache.Values;
                List<object> values = new List<object> ();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    object value = GetValue (propertyInfo);
                    values.Add (value);
                }
                return values.AsReadOnly ();
            }
        }

        /// <summary>
        /// Gets or sets the value of the property with the given name.
        /// </summary>
        /// <param name="key">The name of the property to get or set.</param>
        /// <returns>The value of the property with the given name.</returns>
        /// <exception cref="System.ArgumentNullException">The property name was null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The type does not have a property with the given name.</exception>
        /// <exception cref="System.ArgumentException">The property did not support getting or setting.</exception>
        /// <exception cref="System.ArgumentException">
        /// The object does not match the target type, or a property is a value type but the value is null.
        /// </exception>
        public object this[string key]
        {
            get
            {
                PropertyInfo propertyInfo = referencedTypeCache[key];
                return GetValue (propertyInfo);
            }
            [EditorBrowsable (EditorBrowsableState.Never)]
            set
            {
                throw new NotSupportedException ();
            }
        }

        [EditorBrowsable (EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<string, object>>.Add (KeyValuePair<string, object> item)
        {
            throw new NotSupportedException ();
        }

        [EditorBrowsable (EditorBrowsableState.Never)]
        void ICollection<KeyValuePair<string, object>>.Clear ()
        {
            throw new NotSupportedException ();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains (KeyValuePair<string, object> item)
        {
            PropertyInfo propertyInfo;
            if (!referencedTypeCache.TryGetValue (item.Key, out propertyInfo))
            {
                return false;
            }
            object value = GetValue (propertyInfo);
            return Equals (item.Value, value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo (KeyValuePair<string, object>[] array, int arrayIndex)
        {
            List<KeyValuePair<string, object>> pairs = new List<KeyValuePair<string, object>> ();
            ICollection<KeyValuePair<string, PropertyInfo>> collection = referencedTypeCache;
            foreach (KeyValuePair<string, PropertyInfo> pair in collection)
            {
                PropertyInfo propertyInfo = pair.Value;
                object value = GetValue (propertyInfo);
                pairs.Add (new KeyValuePair<string, object> (pair.Key, value));
            }
            pairs.CopyTo (array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of properties in the type.
        /// </summary>
        public int Count
        {
            get { return referencedTypeCache.Count; }
        }

        /// <summary>
        /// Gets or sets whether updates will be ignored.
        /// </summary>
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return true; }
        }

        [EditorBrowsable (EditorBrowsableState.Never)]
        bool ICollection<KeyValuePair<string, object>>.Remove (KeyValuePair<string, object> item)
        {
            throw new NotSupportedException ();
        }

        /// <summary>
        /// Gets the propety name/value pairs in the object.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator ()
        {
            foreach (KeyValuePair<string, PropertyInfo> pair in referencedTypeCache)
            {
                object value = GetValue (pair.Value);
                yield return new KeyValuePair<string, object> (pair.Key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        private object GetValue (PropertyInfo propertyInfo)
        {
            return propertyInfo.GetValue (referencedInstance, null);
        }
    }
}