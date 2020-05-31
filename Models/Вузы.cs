using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class Вузы
    {
        public Вузы()
        {
            Группы = new HashSet<Группы>();
            Преподаватели = new HashSet<Преподаватели>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(250, ErrorMessage = "Длина до 250 символов")]
        [Display(Name = "Названия ВУЗа")]
        public string НазваниеВуза { get; set; }

        public virtual ICollection<Группы> Группы { get; set; }
        public virtual ICollection<Преподаватели> Преподаватели { get; set; }
    }
}
