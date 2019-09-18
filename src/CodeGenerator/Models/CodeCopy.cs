using System.Collections.Generic;

namespace CodeGenerator.Models
{
    public class CodeCopy
    {
        public string MainProjectFolder { get; set; }
        public string FileFormat { get; set; }
        public string ExampleClassName { get; set; }
        public List<string> ClassName { get; set; }
        public ProjectModel DataAccess { get; set; }
        public ProjectModel Business { get; set; }
    }
}