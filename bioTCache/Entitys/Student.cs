using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bioTCache.Entitys
{
    public class Student : IEntity
    {
        public string FullName { get; set; }

        public DateTime Birthdate { get; set; }

        public float AvarageGrades { get; set; }

        public int Id { get; set; }

    }
}
