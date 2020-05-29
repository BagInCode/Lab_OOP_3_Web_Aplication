using System;
using System.Collections.Generic;

namespace LabIStTP
{
    public partial class Преподаватели
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Фио { get; set; }
        public int Пароль { get; set; }
        public int ВузId { get; set; }

        public virtual Вузы Вуз { get; set; }
    }
}
