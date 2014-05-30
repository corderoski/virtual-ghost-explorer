using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace VGEExplorerTool.Entities
{

    public class NodeString
    {

        public NodeString()
        {
            Childs = new Collection<NodeString>();
        }

        public String Name { get; set; }

        public NodeStringType Type { get; set; }

        public ICollection<NodeString> Childs { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, Type);
        }

    }

    public enum NodeStringType
    {
        Folder,
        File
    }

}
