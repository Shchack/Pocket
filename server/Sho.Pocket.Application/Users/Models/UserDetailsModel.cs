﻿using Sho.Pocket.Application.UserCurrencies.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sho.Pocket.Application.Users.Models
{
    public class UserDetailsModel
    {
        public UserDetailsModel(string email, List<UserCurrencyModel> userCurrencies)
        {
            Email = email;
            DefaultCurrencies = userCurrencies?.Select(uc => uc.Currency).ToList();
            PrimaryCurrency = userCurrencies?.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();
        }

        public string Email { get; set; }

        public List<string> DefaultCurrencies { get; set; }

        public string PrimaryCurrency { get; set; }
    }
}
