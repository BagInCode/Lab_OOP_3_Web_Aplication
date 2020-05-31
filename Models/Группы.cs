using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class Группы
    {
        public Группы()
        {
            Студенты = new HashSet<Студенты>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(20, ErrorMessage = "Длина до 20 символов")]
        public string Название { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public int ВузId { get; set; }

        public virtual Вузы Вуз { get; set; }
        public virtual ICollection<Студенты> Студенты { get; set; }
    }
}
