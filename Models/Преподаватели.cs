using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLab
{
    public partial class Преподаватели
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Не соответствует адресу эл. почты")]
        [MaxLength(50, ErrorMessage = "Не более 50 символов")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(50, ErrorMessage = "Не более 50 символов")]
        public string Фио { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [MinLength(8, ErrorMessage = "Должен содержать 8+ символов")]
        [MaxLength(25, ErrorMessage = "Не более 25 символов")]
        [DataType(DataType.Password)]
        public string Пароль { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public int ВузId { get; set; }
        public virtual Вузы Вуз { get; set; }
    }
}
