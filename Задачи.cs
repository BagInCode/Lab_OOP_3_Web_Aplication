using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Задачи
    {
        public Задачи()
        {
            СтудентЗадача = new HashSet<СтудентЗадача>();
        }

        public int Id { get; set; }
        public string Место { get; set; }
        public DateTime Дата { get; set; }
        public string Описание { get; set; }
        public int ПользовательId { get; set; }
        public int СценарийId { get; set; }

        public virtual Пользователь Пользователь { get; set; }
        public virtual Сценарии Сценарий { get; set; }
        public virtual ICollection<СтудентЗадача> СтудентЗадача { get; set; }
    }
}
