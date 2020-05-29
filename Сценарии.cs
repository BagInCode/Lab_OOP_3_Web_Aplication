using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Сценарии
    {
        public Сценарии()
        {
            Задачи = new HashSet<Задачи>();
        }

        public int Id { get; set; }
        public int КВоАктёров { get; set; }
        public string Описание { get; set; }

        public virtual ICollection<Задачи> Задачи { get; set; }
    }
}
