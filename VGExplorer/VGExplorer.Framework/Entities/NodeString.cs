using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VGExplorer.Framework.Helpers;

namespace VGExplorer.Framework.Entities
{

    public class NodeString
    {

        public NodeString()
        {
            Childs = new Collection<NodeString>();
        }

        public String Name { get; set; }

        public NodeStringType Type { get; set; }

        public DateTime Date { get; set; }

        public String Size { get; set; }

        public ICollection<NodeString> Childs { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, Type);
        }

        /// <summary>
        /// Returns a string that represents the current object in a longer-info format.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public string ToLongString()
        {
            var size = "";
            if (String.IsNullOrEmpty(Size))
                size = String.Format(" - {0}", Size);
            return String.Format("{0} [{1}{2}]", Name, FormatHelper.GetFormattedDateTime(Date), size);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var external = obj as NodeString;
            if (external == null) return false;

            if (!this.ToString().Equals(external.ToString()))
                return false;

            if (this.Childs != external.Childs || this.Childs.Count != external.Childs.Count)
                return false;

            //for (int i = 0; i < this.Childs.Count; i++)
            //{
            //    if (this.Childs.ElementAt(i) != external.Childs.ElementAt(i))
            //        return false;
            //}

            //this.Childs.All(a => external.Childs.Any(b => a == b));

            return this.Childs.All(a => external.Childs.Any(a.Equals));
        }

    }
}
