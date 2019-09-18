namespace CodeGenerator.Models
{
    public class ProjectModel
    {
        public string ProjectName { get; set; }
        public ProjectInfoModel Interface { get; set; }
        public ProjectInfoModel Operation { get; set; }
    }
}