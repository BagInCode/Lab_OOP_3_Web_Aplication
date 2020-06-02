using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class Задачи
    {
        public Задачи()
        {
            СтудентЗадача = new HashSet<СтудентЗадача>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(100, ErrorMessage = "Длина до 250 символов")]
        public string Место { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DateTimeValidator(ErrorMessage = "Укажите дату и время начиная от сейчас и в течении 30 дней")]
        public DateTime Дата { get; set; }
        [MaxLength(250, ErrorMessage = "Длина до 250 символов")]
        public string Описание { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string ПользовательId { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public int СценарийId { get; set; }

        public virtual Пользователь Пользователь { get; set; }
        public virtual Сценарии Сценарий { get; set; }
        public virtual ICollection<СтудентЗадача> СтудентЗадача { get; set; }
    }
}
