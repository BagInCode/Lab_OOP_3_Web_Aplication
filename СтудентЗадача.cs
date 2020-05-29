using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class СтудентЗадача
    {
        public int Id { get; set; }
        public int СтудентId { get; set; }
        public int ЗадачаId { get; set; }

        public virtual Задачи Задача { get; set; }
        public virtual Студенты Студент { get; set; }
    }
}
