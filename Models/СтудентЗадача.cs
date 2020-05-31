using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class СтудентЗадача
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно поле")]
        public string СтудентId { get; set; }
        [Required(ErrorMessage = "Обязательно поле")]
        public int ЗадачаId { get; set; }

        public virtual Задачи Задача { get; set; }
        public virtual Студенты Студент { get; set; }
    }
}
