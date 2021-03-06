﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Slappy
{
    public class Node : DynamicObject
    {
        private VersionedDictionary<string, object> valueByPath;
      

        public Node()
        {
            valueByPath = new VersionedDictionary<string, object>();
        }

        protected Node Parent { get; private set; }
        private string name;

        protected Node(Node parent, string name)
        {
            Parent = parent;
            this.name = name;
        }

        protected Node(VersionedDictionary<string, object> valueByPath) : this()
        {
            this.valueByPath = valueByPath.Clone();
        }

        protected Node CreateNodeChain(object[] indexes)
        {
            var current = FindRootNode();
            foreach (var index in indexes)
            {
                current = new Node(current, index.ToString());
            }

            return current;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!TryGetValue(binder.Name, out result))
            {
                result = new Node(this, binder.Name);
            }

            return true;
        }

        protected bool TryGetValue(object[] indexes, out object value)
        {
            if (Parent != null)
            {
                return Parent.TryGetValue(indexes, out value);
            }

            var path = string.Join("/", indexes);

            return TryGetValue(path, out value);
        }
      
     


        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (!TryGetValue(indexes, out result))
            {
                result = CreateNodeChain(indexes);
            }

            return true;
        }

        private Node FindRootNode()
        {
            var parent = this;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
            }

            return parent;
        }

    
       

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return TrySetValue(binder.Name, value);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return TrySetValue(indexes, value);
        }

        protected bool TrySetValue(object[] indexes, object value)
        {
            if (Parent != null)
            {
                return Parent.TrySetValue(indexes, value);
            }

            var path = string.Join("/", indexes);
            TrySetValue(path, value);
            return true;
        }


        protected bool TryGetValue(string path, out Object value)
        {
            if (Parent != null)
            {
                return Parent.TryGetValue(name + "/" + path, out value);
            }

            if (valueByPath.TryGetValue(path, out value))
            {
                return true;
            }

            return valueByPath.TryGetValue(path, out value);
        }

        protected bool TrySetValue(string path, Object value)
        {
            if (Parent != null)
            {
                return Parent.TrySetValue(name + "/" + path, value);
            }

            valueByPath[path] = value;
            return true;
        }

        public Node Clone()
        {
            return new Node(valueByPath);
        }

        public void Commit()
        {
            if (valueByPath.CurrentVersionKeyCount > 0)
            {
                valueByPath = new VersionedDictionary<string, object>(valueByPath);
            }
        }

        public void Pull(Node other)
        {
            throw new NotImplementedException();
        }
    }
}
