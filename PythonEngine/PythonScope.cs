using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Scripting.Hosting;
using ResharperAnnotations;


namespace PythonEngine
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class PythonScope : IDictionary, IDictionary<String, Object>, IBindingList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        internal PythonScope(ScriptScope scope)
        {
            this.Scope = scope;
        }

        internal readonly ScriptScope Scope;
        private List<ListChangedEventHandler> _listChanged;

        // 
        /// <summary>
        /// Determines if this context or any outer scope contains the defined name.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">name is a null reference. </exception>
        /// <param name="name">The variable name to look for</param>
        /// <returns><see cref="bool"/> indicating whether the variable exists</returns>
        public bool ContainsVariable(string name)
        {
            return this.Scope.ContainsVariable(name);
        }

        /// <summary>
        /// Gets an array of variable names and their values stored in the scope.
        /// </summary>
        /// <returns><see cref="KeyValuePair{TKey,TValue}">KeyValuePairs</see> of variable names and values. 
        /// </returns>
        public IEnumerable<KeyValuePair<string, dynamic>> GetItems()
        {
            return this.Scope.GetItems();
        }

        /// <summary>
        /// Gets a value stored in the scope under the given name.
        /// </summary>
        /// <typeparam name="T">blah blah blah</typeparam>
        /// <exception cref="System.MissingMemberException">The specified name is not defined in the scope.</exception>
        /// <exception cref="System.ArgumentNullException">name is a null reference</exception>   
        public T GetVariable<T>(string name)
        {
            return this.Scope.GetVariable<T>(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public dynamic GetVariable(string name)
        {

            try
            {
                return this.Scope.GetVariable(name);
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveVariable(string name)
        {
            bool removed = this.Scope.RemoveVariable(name);
            OnVariableChanged(name, PythonVariableChangedAction.Delete);
            return removed;
        }

        /// <summary>
        /// Sets the name to the specified value.
        /// </summary>
        /// <exception cref="ArgumentNullException">name is a null reference.</exception>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetVariable(string name, object value)
        {
            this.Scope.SetVariable(name, value);
            OnVariableChanged(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetVariable<T>(string name, out T value)
        {
            bool exists = Scope.ContainsVariable(name);
            value = exists ? this.Scope.GetVariable<T>(name) : default(T);
            return exists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetVariable(string name, out dynamic value)
        {
            return TryGetVariable<dynamic>(name, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static explicit operator ScriptScope(PythonScope scope)
        {
            return scope.Scope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static explicit operator PythonScope(ScriptScope scope)
        {
            return new PythonScope(scope);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
        /// </returns>
        /// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        int IList.Add(object value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IDictionary"/> object contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IDictionary"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary"/> object.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception>
        public bool Contains(object key)
        {
            return this.Scope.ContainsVariable((string)key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <param name="key">The <see cref="T:System.Object"/> to use as the key of the element to add. </param><param name="value">The <see cref="T:System.Object"/> to use as the value of the element to add. </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception><exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.IDictionary"/> object. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> is read-only.-or- The <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
        void IDictionary.Add(object key, object value)
        {
            string name = (string)key;
            this.Scope.SetVariable(name, (dynamic)value);
            OnVariableChanged(name, PythonVariableChangedAction.Add);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(String key, dynamic value)
        {
            string name = key;
            this.Scope.SetVariable(name, value);
            OnVariableChanged(name, PythonVariableChangedAction.Add);
        }
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(KeyValuePair<string, object> item)
        {
            Scope.SetVariable(item.Key,item.Value);
            OnVariableChanged(item.Key, PythonVariableChangedAction.Add);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> object is read-only. </exception>
        public void Clear()
        {
            this.Scope.GetItems().ToList().ForEach(kvp => this.Scope.RemoveVariable(kvp.Key));
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>. </param>
        int IList.IndexOf(object value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        void IList.Insert(int index, object value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.Remove(object value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
        object IList.this[int index]
        {
            get
            {
                return Scope.GetItems().ToArray()[index];
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return Scope.ContainsVariable(item.Key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <see cref="KeyValuePair{String,Object}"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            Scope.GetItems().ToArray().CopyTo(array,arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(KeyValuePair<string, object> item)
        {
            bool removed = Scope.RemoveVariable(item.Key);
            OnVariableChanged(item.Key, PythonVariableChangedAction.Delete);
            return removed;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.
        /// </returns>
        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return new PythonScopeEnum(this);
        }

        /// <summary>
        /// Returns an <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.
        /// </returns>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new PythonScopeEnum(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public class PythonScopeEnum : IDictionaryEnumerator, IEnumerator<KeyValuePair<string, object>>
        {
            private readonly IEnumerator<KeyValuePair<string, object>> _iter;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="scope"></param>
            public PythonScopeEnum(PythonScope scope)
            {
                this._iter = scope.Scope.GetItems().GetEnumerator();
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                return this._iter.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                this._iter.Reset();
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            KeyValuePair<string, object> IEnumerator<KeyValuePair<string, object>>.Current
            {
                get
                {
                    return _iter.Current;
                }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
            public object Current
            {
                get
                {
                    return this._iter.Current;
                }
            }

            /// <summary>
            /// Gets the key of the current dictionary entry.
            /// </summary>
            /// <returns>
            /// The key of the current element of the enumeration.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public object Key
            {
                get
                {
                    return this._iter.Current.Key;
                }
            }

            /// <summary>
            /// Gets the value of the current dictionary entry.
            /// </summary>
            /// <returns>
            /// The value of the current element of the enumeration.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public object Value
            {
                get
                {
                    return this._iter.Current.Value;
                }
            }

            /// <summary>
            /// Gets both the key and the value of the current dictionary entry.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.DictionaryEntry"/> containing both the key and the value of the current dictionary entry.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public DictionaryEntry Entry
            {
                get
                {
                    return new DictionaryEntry(this._iter.Current.Key, this._iter.Current.Value);
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <param name="key">The key of the element to remove. </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> object is read-only.-or- The <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
        void IDictionary.Remove(object key)
        {
            string name = (string)key;
            this.Scope.RemoveVariable(name);
            OnVariableChanged(name, PythonVariableChangedAction.Delete);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set. </param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IDictionary"/> object is read-only.-or- The property is set, <paramref name="key"/> does not exist in the collection, and the <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
        public dynamic this[object key]
        {
            get
            {
                return this.Scope.GetVariable((string)key);
            }
            set
            {
                string name = (string)key;
                if (Scope.ContainsVariable(name))
                {
                    if (value != this.Scope.GetVariable(name))
                    {

                        this.Scope.SetVariable(name, value);
                        OnVariableChanged(name);
                    }
                }
                else
                {
                    this.Scope.SetVariable(name, value);
                    OnVariableChanged(name, PythonVariableChangedAction.Add);
                }
            }
        }
        
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(string key)
        {
            return Scope.ContainsVariable(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public bool Remove(string key)
        {
            bool removed = Scope.RemoveVariable(key);
            if (removed)
            {
                OnVariableChanged(key, PythonVariableChangedAction.Delete);
            }
            return removed; 
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        bool IDictionary<string,object>.TryGetValue(string key, out object value)
        {
            return TryGetVariable(key, out value);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        /// The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public dynamic this[string key]
        {
            get
            {
                return Scope.GetVariable(key);
            }
            set
            {
                if(Scope.ContainsVariable(key))
                {
                    dynamic val = Scope.GetVariable(key);
                    if (!value.Equals(val))
                    {
                        Scope.SetVariable(key, value);
                        OnVariableChanged(key);
                    }
                }
                else
                {
                    Scope.SetVariable(key, value);
                    OnVariableChanged(key, PythonVariableChangedAction.Add);
                    
                }
            }
        }
        
        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection"/> object containing the keys of the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.ICollection"/> object containing the keys of the <see cref="T:System.Collections.IDictionary"/> object.
        /// </returns>
        public ICollection Keys
        {
            get
            {
                return (ICollection)Scope.GetVariableNames();
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        ICollection<string> IDictionary<string, object>.Keys
        {
            get
            {
                return (ICollection<string>)this.Scope.GetVariableNames();
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public ICollection<dynamic> Values
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        
        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.
        /// </returns>
        ICollection IDictionary.Values
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IDictionary"/> object is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"/> object has a fixed size.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IDictionary"/> object has a fixed size; otherwise, false.
        /// </returns>
        bool IDictionary.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception><exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        public void CopyTo(Array array, int index)
        {
            Scope.GetItems().ToArray().CopyTo(array,index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        //public IEnumerator GetEnumerator()
        //{
        //    return this.Scope.GetItems().GetEnumerator();
        //}

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return this.Scope.GetItems().Count();
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        object ICollection.SyncRoot
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PythonVariableChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnVariableChanged(string propertyName, PythonVariableChangedAction action = PythonVariableChangedAction.Change)
        {
            PythonVariableChangedEventHandler handler = PropertyChanged;
            if (handler == null)
            {
                return;
            }
            handler(this, new PythonVariableChangedEventArgs(propertyName, action));
            ListChangedType changedType;

            switch (action)
            {
            case PythonVariableChangedAction.Add:
                changedType = ListChangedType.ItemAdded;
                break;
            case PythonVariableChangedAction.Delete:
                changedType = ListChangedType.ItemDeleted;
                break;
            default:
                changedType = ListChangedType.ItemChanged;
                break;
            }
            int index = this.Scope.GetVariableNames().ToList().IndexOf(propertyName);
            ListChangedEventArgs args = new ListChangedEventArgs(changedType, index);
                
            OnListChanged(args);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <returns>
        /// The item added to the list.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.AllowNew"/> is false. </exception>
        object IBindingList.AddNew()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds the <see cref="T:System.ComponentModel.PropertyDescriptor"/> to the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to add to the indexes used for searching. </param>
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor"/> and a <see cref="T:System.ComponentModel.ListSortDirection"/>.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to sort by. </param>
        /// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values. </param>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </summary>
        /// <returns>
        /// The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
        /// </returns>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to search on. </param>
        /// <param name="key">The value of the <paramref name="property"/> parameter to search for. </param>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSearching"/> is false. </exception>
        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the <see cref="T:System.ComponentModel.PropertyDescriptor"/> from the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to remove from the indexes used for searching. </param>
        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes any sort applied using <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>.
        /// </summary>
        /// <returns>
        /// true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>; 
        /// otherwise, false.
        /// </returns>
        bool IBindingList.AllowNew
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether you can update items in the list.
        /// </summary>
        /// <returns>
        /// true if you can update the items in the list; otherwise, false.
        /// </returns>
        bool IBindingList.AllowEdit
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether you can remove items from the list, using <see cref="M:System.Collections.IList.Remove(System.Object)"/> or <see cref="M:System.Collections.IList.RemoveAt(System.Int32)"/>.
        /// </summary>
        /// <returns>
        /// true if you can remove items from the list; otherwise, false.
        /// </returns>
        bool IBindingList.AllowRemove
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or an item in the list changes.
        /// </summary>
        /// <returns>
        /// true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or when an item changes; otherwise, false.
        /// </returns>
        bool IBindingList.SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets whether the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method.
        /// </summary>
        /// <returns>
        /// true if the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method; otherwise, false.
        /// </returns>
        bool IBindingList.SupportsSearching
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether the list supports sorting.
        /// </summary>
        /// <returns>
        /// true if the list supports sorting; otherwise, false.
        /// </returns>
        bool IBindingList.SupportsSorting
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets whether the items in the list are sorted.
        /// </summary>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/> has been called and <see cref="M:System.ComponentModel.IBindingList.RemoveSort"/> has not been called; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        bool IBindingList.IsSorted
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        PropertyDescriptor IBindingList.SortProperty
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the direction of the sort.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"><see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
        ListSortDirection IBindingList.SortDirection
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        private void OnListChanged(ListChangedEventArgs args)
        {
            var handler = ListChanged;
            {
                if (handler != null)
                {
                    handler(this, args);
                }
            }
        }
    }

    /// <summary>Raised when a variable is changed within the <see cref="PythonScope"/>.</summary>
    /// <param name="sender" />
    /// <param name="args" />
    public delegate void PythonVariableChangedEventHandler(object sender, PythonVariableChangedEventArgs args);

    /// <summary />
    public class PythonVariableChangedEventArgs : PropertyChangedEventArgs
    {
        private readonly PythonVariableChangedAction _action;

        /// <summary />
        public PythonVariableChangedAction Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. </param>
        /// <param name="action">What happened</param>
        public PythonVariableChangedEventArgs(string propertyName, PythonVariableChangedAction action)
                : base(propertyName)
        {
            _action = action;
        }
    }

    /// <summary />
    public enum PythonVariableChangedAction
    {
        /// <summary />
        Change,

        /// <summary />
        Add,

        /// <summary />
        Delete
    }
}