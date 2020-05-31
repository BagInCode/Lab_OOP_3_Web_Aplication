using System;
using System.Collections.Generic;

namespace WebLab
{
    public partial class СтудентЗадача
    {
        public int Id { get; set; }
        public string СтудентId { get; set; }
        public int ЗадачаId { get; set; }

        public virtual Задачи Задача { get; set; }
        public virtual Студенты Студент { get; set; }
    }
}
