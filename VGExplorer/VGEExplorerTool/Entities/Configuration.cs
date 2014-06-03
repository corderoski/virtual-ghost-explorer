using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace VGExplorerTool.Entities
{
    internal class Configuration
    {

        [DefaultValue(false)]
        public bool IncludeFilesInSchemaCreation { get; set; }



    }
}
