using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Project
{
    public class ProjectDetailForEditDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
