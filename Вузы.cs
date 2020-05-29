using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Вузы
    {
        public Вузы()
        {
            Группы = new HashSet<Группы>();
            Преподаватели = new HashSet<Преподаватели>();
        }

        public int Id { get; set; }
        public string НазваниеВуза { get; set; }

        public virtual ICollection<Группы> Группы { get; set; }
        public virtual ICollection<Преподаватели> Преподаватели { get; set; }
    }
}
