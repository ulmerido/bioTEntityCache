using System;

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
