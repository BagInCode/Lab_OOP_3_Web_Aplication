using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Студенты
    {
        public Студенты()
        {
            СтудентЗадача = new HashSet<СтудентЗадача>();
        }

        public int Id { get; set; }
        public string Mail { get; set; }
        public string Фио { get; set; }
        public int Пароль { get; set; }
        public int ГруппаId { get; set; }

        public virtual Группы Группа { get; set; }
        public virtual ICollection<СтудентЗадача> СтудентЗадача { get; set; }
    }
}
