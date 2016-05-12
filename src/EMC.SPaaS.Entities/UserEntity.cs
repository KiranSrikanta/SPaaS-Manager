using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace EMC.SPaaS.Entities
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
