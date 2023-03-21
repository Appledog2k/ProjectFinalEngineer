﻿using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateUser
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Display(Name = "địa chỉ thư điện tử hoặc tên tài khoản")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhớ thông tin đăng nhập?")]
        public bool RememberMe { get; set; }
    }
}
