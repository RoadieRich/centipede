using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ResharperAnnotations;


namespace CentipedeInterfaces
{
    /// <summary>
    /// Wrapper to allow uniform access to <see cref="T:System.Reflection.FieldInfo"/> and <see cref="T:System.Reflection.PropertyInfo"/>.
    /// Designed to be used with casts, rather than constructors.
    /// </summary>
    public class FieldAndPropertyWrapper : IDisposable, IEquatable<FieldAndPropertyWrapper>
    {
        /// <summary>
        /// Creates a new <see cref="FieldAndPropertyWrapper"/>
        /// </summary>
        /// <param name="prop">The <see cref="PropertyInfo"/> to wrap</param>
        /// <seealso cref="FieldAndPropertyWrapper(FieldInfo)"/>
        [UsedImplicitly]
        private FieldAndPropertyWrapper(PropertyInfo prop)
        {
            this._member = prop;
            this._getter = o => prop.GetValue(o, null);
            this._setter = (o, v) => prop.SetValue(o, v, null);
            this._memberType = prop.PropertyType;

            _Cache[prop] = this;
        }

        /// <summary>
        /// Creates a new <see cref="FieldAndPropertyWrapper"/>
        /// </summary>
        /// <param name="field">The <see cref="FieldInfo"/> to wrap</param>
        /// <seealso cref="FieldAndPropertyWrapper(PropertyInfo)"/>
        [UsedImplicitly]
        private FieldAndPropertyWrapper(FieldInfo field)
        {
            this._member = field;
            this._getter = field.GetValue;
            this._setter = field.SetValue;
            this._memberType = field.FieldType;

            _Cache[field] = this;
        }


        /// <value>
        /// The <see cref="T:System.Type"/> that declares the member the <see cref="T:CentipedeInterfaces.FieldAndPropertyWrapper"/> is referencing
        /// </value>
        public Type DeclaringType
        {
            get
            {
                return this._member.DeclaringType;
            }
        }


        private readonly MemberInfo _member;
        private readonly Func<object, object> _getter;

        /// <summary>
        /// Gets the value of the wrapped member
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The instance to get the value of the member of</param>
        /// <returns></returns>
        public T Get<T>(object o)
        {
            return (T)this._getter(o);
        }

        private readonly Action<object, object> _setter;
        private readonly Type _memberType;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="value"></param>
        public void Set<T>(object o, T value)
        {
            this._setter(o, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public ActionArgumentAttribute GetArgumentAttribute()
        {
            return
                    this._member.GetCustomAttributes(typeof (ActionArgumentAttribute), true)
                           .Cast<ActionArgumentAttribute>()
                           .SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return this._member.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type MemberType
        {
            get
            {
                return this._memberType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public enum FieldType
        {
            //first value listed is used for default()
            /// <summary>
            /// 
            /// </summary>
            Other,

            /// <summary>
            /// The wrapped member is a String type (string, char, etc)
            /// </summary>
            String,

            /// <summary>
            /// Wrapped member is a numeric type (int, double, Decimal, etc)
            /// </summary>
            Numeric,

            /// <summary>
            /// Wrapped member is boolean
            /// </summary>
            Boolean,
        }

        private static readonly Dictionary<Type, FieldType> _TypeMapping = new Dictionary<Type, FieldType>
                                                                          {
                                                                              { typeof (bool),   FieldType.Boolean },
                                                                              { typeof (byte),   FieldType.Numeric },
                                                                              { typeof (sbyte),  FieldType.Numeric },
                                                                              { typeof (char),   FieldType.String  },
                                                                              { typeof (decimal),FieldType.Numeric },
                                                                              { typeof (double), FieldType.Numeric },
                                                                              { typeof (float),  FieldType.Numeric },
                                                                              { typeof (int),    FieldType.Numeric },
                                                                              { typeof (uint),   FieldType.Numeric },
                                                                              { typeof (long),   FieldType.Numeric },
                                                                              { typeof (ulong),  FieldType.Numeric },
                                                                              { typeof (short),  FieldType.Numeric },
                                                                              { typeof (ushort), FieldType.Numeric },
                                                                              { typeof (string), FieldType.String  },
                                                                          };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FieldType GetFieldTypeCategory()
        {

            Type baseType = MemberType;

            while (!_TypeMapping.ContainsKey(baseType) && baseType != typeof (object))
            {
                baseType = baseType.BaseType ?? typeof (Object);
            }

            FieldType fieldType;
            return _TypeMapping.TryGetValue(baseType, out fieldType) ? fieldType : FieldType.Other;
        }

        /// <summary>
        /// Cast <see cref="FieldInfo"/> into <see cref="FieldAndPropertyWrapper"/> 
        /// </summary>
        /// <param name="f"><see cref="FieldInfo"/> to wrap.</param>
        /// <returns><see cref="FieldAndPropertyWrapper"/> wrapping <paramref name="f"/>.</returns>
        public static implicit operator FieldAndPropertyWrapper(FieldInfo f)
        {
            if (_Cache.ContainsKey(f))
            {
                Debug.WriteLine(@"FieldAndPropertyWrapper Cache hit {0}", f);
                return _Cache[f];
            }
            Debug.WriteLine(@"FieldAndPropertyWrapper Cache miss {0}", f);
            return new FieldAndPropertyWrapper(f);
        }

        /// <summary>
        /// Unwrap <see cref="FieldAndPropertyWrapper"/> to <see cref="FieldInfo"/>
        /// </summary>
        /// <param name="wrapped"><see cref="FieldAndPropertyWrapper"/> to unwrap</param>
        /// <returns>The <see cref="FieldInfo"/> <paramref name="wrapped"/> is wrapping</returns>
        /// /// <exception cref="InvalidCastException"><paramref name="wrapped"/> is not wrapping a <see cref="FieldInfo"/>.</exception>
        public static explicit operator FieldInfo(FieldAndPropertyWrapper wrapped)
        {
            if (wrapped._member is FieldInfo)
            {
                return wrapped._member as FieldInfo;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// Cast <see cref="FieldInfo"/> into <see cref="FieldAndPropertyWrapper"/> 
        /// </summary>
        /// <param name="p"><see cref="PropertyInfo"/> to wrap.</param>
        /// <returns><see cref="FieldAndPropertyWrapper"/> wrapping <paramref name="p"/>.</returns>
        public static implicit operator FieldAndPropertyWrapper(PropertyInfo p)
        {
            return _Cache.ContainsKey(p) ? _Cache[p] : new FieldAndPropertyWrapper(p);
        }

        /// <summary>
        /// Unwrap <see cref="FieldAndPropertyWrapper"/> to <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="wrapped"><see cref="FieldAndPropertyWrapper"/> to unwrap</param>
        /// <returns>The <see cref="PropertyInfo"/> <paramref name="wrapped"/> is wrapping</returns>
        /// <exception cref="InvalidCastException"><paramref name="wrapped"/> is not wrapping a <see cref="PropertyInfo"/>.</exception>
        public static explicit operator PropertyInfo(FieldAndPropertyWrapper wrapped)
        {
            if (wrapped._member is PropertyInfo)
            {
                return wrapped._member as PropertyInfo;
            }
            throw new InvalidCastException();
        }

        private static readonly Dictionary<MemberInfo, FieldAndPropertyWrapper> _Cache =
                new Dictionary<MemberInfo, FieldAndPropertyWrapper>();

        
        /// <summary>
        /// Wrap <see cref="MemberInfo"/> in <see cref="FieldAndPropertyWrapper"/>
        /// </summary>
        /// <param name="m">The <see cref="MemberInfo"/> to wrap</param>
        /// <returns>The wrapped <see cref="MemberInfo"/>.</returns>
        /// <exception cref="InvalidCastException"><paramref name="m"/> is not an instance of <see cref="FieldInfo"/> 
        /// or <see cref="PropertyInfo"/></exception>
        public static explicit operator FieldAndPropertyWrapper(MemberInfo m)
        {
            if (_Cache.ContainsKey(m))
            {
                Debug.WriteLine(@"FieldAndPropertyWrapper Cache hit {0}", m);
                return _Cache[m];
            }

            Debug.WriteLine(@"FieldAndPropertyWrapper Cache miss {0}", m);
            FieldInfo f = m as FieldInfo;
            if (f != null)
            {   
                return new FieldAndPropertyWrapper(f);
            }

            PropertyInfo prop = m as PropertyInfo;
            if (prop != null)
            {
                return new FieldAndPropertyWrapper(prop);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Unwrap the wrapped <see cref="MemberInfo"/>
        /// </summary>
        /// <param name="wrapped">The <see cref="FieldAndPropertyWrapper"/> to unwrap</param>
        /// <returns>The unwrapped <see cref="MemberInfo"/>.</returns>
        public static implicit operator MemberInfo(FieldAndPropertyWrapper wrapped)
        {
            return wrapped._member;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            _Cache.Clear();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(FieldAndPropertyWrapper other)
        {
            // This won't work if you want to compare methods or constructors (due to the possiblity of 
            // different signatures, but then this class isn't intended to wrap those.
            return DeclaringType == other.DeclaringType
                   && Name == other.Name;
        }

        public static void SetPropertyOnObject<TObject, TValue>(TObject @object, Expression<Func<TObject, TValue>> propertySelector, TValue value)
        {
            MemberExpression memberAccess = propertySelector.Body as MemberExpression;
            if (memberAccess == null)
            {
                throw new ArgumentNullException("memberAccess");
            }

            FieldAndPropertyWrapper property = (FieldAndPropertyWrapper)memberAccess.Member;

            property.Set(@object, value);

        }

        public static TValue GetPropertyOnObject<TObject, TValue>(TObject @object, Expression<Func<TObject, TValue>> propertySelector)
        {
            MemberExpression memberAccess = propertySelector.Body as MemberExpression;
            if (memberAccess == null)
            {
                throw new ArgumentNullException("memberAccess");
            }

            FieldAndPropertyWrapper property = (FieldAndPropertyWrapper)memberAccess.Member;

            return property.Get<TValue>(@object);
        }
    }
}