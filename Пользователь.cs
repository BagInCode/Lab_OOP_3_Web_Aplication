using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Пользователь
    {
        public Пользователь()
        {
            Задачи = new HashSet<Задачи>();
        }

        public int Id { get; set; }
        public string Mail { get; set; }
        public string Фио { get; set; }
        public int Пароль { get; set; }

        public virtual ICollection<Задачи> Задачи { get; set; }
    }
}
