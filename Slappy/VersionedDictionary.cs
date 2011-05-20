using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slappy
{
    public class VersionedDictionary<keyType, valueType>
    {

        public VersionedDictionary()
        {

        }

        public VersionedDictionary<keyType, valueType> Previous { get; private set; }

        public VersionedDictionary(VersionedDictionary<keyType, valueType> previous)
        {
            Previous = previous;
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

        public VersionedDictionary<keyType, valueType> Clone()
        {
            var clone = new VersionedDictionary<keyType, valueType>();
            var currentTarget = clone;
            var currentSource = this;
           
            while (currentSource != null)
            {
                foreach (var pair in currentSource.valueByKey)
                {
                    currentTarget[pair.Key] = pair.Value;
                }

                currentSource = currentSource.Previous;
                if (currentSource != null)
                {
                    var previousTarget = currentTarget;
                    currentTarget = new VersionedDictionary<keyType, valueType>();
                    previousTarget.Previous = currentTarget;
                }
            }

            return clone;
        }
    }
}
