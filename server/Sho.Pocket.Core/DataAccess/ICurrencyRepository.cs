﻿using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface ICurrencyRepository
    {
        List<Currency> GetAll();
    }
}