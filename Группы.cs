using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Группы
    {
        public Группы()
        {
            Студенты = new HashSet<Студенты>();
        }

        public int Id { get; set; }
        public string Название { get; set; }
        public int ВузId { get; set; }

        public virtual Вузы Вуз { get; set; }
        public virtual ICollection<Студенты> Студенты { get; set; }
    }
}
