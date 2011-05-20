using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Slappy
{
    public class Node : DynamicObject
    {
        private Dictionary<string, object> valueByPath;

        public Node()
        {
            valueByPath = new Dictionary<string, object>();
        }

        private Node parent;
        private string name;
        
        protected Node(Node parent, string name)
        {
            this.parent = parent;
            this.name = name;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!TryGetValue(binder.Name, out result))
            {
                result = new Node(this, binder.Name);
            }
            
            return true;
        }

        

        protected bool TryGetValue(string path, out Object value)
        {
            if (parent == null)
            {
                if (valueByPath.TryGetValue(path, out value))
                {
                    return true;
                }

                return false;
            }
            else
            {
                return parent.TryGetValue(name + "/" + path, out value);  
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return TrySetValue(binder.Name, value);
        }

       
        protected bool TrySetValue(string path, Object value)
        {
            if (parent == null)
            {
                valueByPath[path] = value;
            }
            else
            {
                return parent.TrySetValue(name + "/" + path, value);
            }

            return true;
        }


        public static Node Merge(dynamic one, dynamic two)
        {
            return new Node();
        }
    }
}
