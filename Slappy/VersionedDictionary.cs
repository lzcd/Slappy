using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slappy
{
    public class VersionedDictionary<keyType, valueType>
    {
        protected string id;

        public VersionedDictionary()
        {

        }

        protected VersionedDictionary(string id)
        {
            this.id = id;
        }

        public VersionedDictionary<keyType, valueType> Previous { get; private set; }

        public VersionedDictionary(VersionedDictionary<keyType, valueType> previous)
            : this()
        {
            Previous = previous;
            if (previous.id == null)
            {
                id = Guid.NewGuid().ToString();
            }
        }

        private Dictionary<keyType, valueType> valueByKey;

        public valueType this[keyType key]
        {
            get
            {
                valueType value;
                if (TryGetValue(key, out value))
                {
                    return value;
                }

                throw new IndexOutOfRangeException();
            }
            set
            {
                if (valueByKey == null)
                {
                    valueByKey = new Dictionary<keyType, valueType>();
                }

                valueByKey[key] = value;
            }
        }

        public bool TryGetValue(keyType key, out valueType value)
        {
            if (valueByKey != null &&
                valueByKey.TryGetValue(key, out value))
            {
                return true;
            }

            if (Previous != null)
            {
                return Previous.TryGetValue(key, out value);
            }

            value = default(valueType);
            return false;
        }

        public int CurrentVersionKeyCount
        {
            get
            {
                if (valueByKey == null)
                {
                    return 0;
                }
                return valueByKey.Keys.Count;
            }
        }

        public VersionedDictionary<keyType, valueType> Clone()
        {
            var currentSource = this;
            var clone = new VersionedDictionary<keyType, valueType>(this);
            var currentTarget = clone;
            

            while (currentSource != null)
            {
                if (currentSource.valueByKey != null)
                {
                    foreach (var pair in currentSource.valueByKey)
                    {
                        currentTarget[pair.Key] = pair.Value;
                    }
                }

                currentSource = currentSource.Previous;
                if (currentSource != null)
                {
                    var previousTarget = currentTarget;
                    currentTarget = new VersionedDictionary<keyType, valueType>(currentSource.id);
                    previousTarget.Previous = currentTarget;
                }
            }

            return clone;
        }
    }
}
