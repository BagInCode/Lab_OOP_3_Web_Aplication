using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class Сценарии
    {
        public Сценарии()
        {
            Задачи = new HashSet<Задачи>();
        }

        public int Id { get; set; }
        [Display(Name = "Количество актёров")]
        [Required(ErrorMessage = "Обязательное поле")]
        public int КВоАктёров { get; set; }
        [MaxLength(250, ErrorMessage = "Длина до 250 символов")]
        public string Описание { get; set; }

        public virtual ICollection<Задачи> Задачи { get; set; }
    }
}
