using System.ComponentModel;

namespace VGExplorer.Framework.Entities
{
    public class Configuration
    {

        [DefaultValue(false)]
        public bool IncludeFilesInSchemaCreation { get; set; }

    }
}
