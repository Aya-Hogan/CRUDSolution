﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if(obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }
            CountryResponse country_to_compare = (CountryResponse)obj;
            return this.CountryId == country_to_compare.CountryId
                && this.CountryName == country_to_compare.CountryName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public static class CountryExtensions
    {
            public static CountryResponse ToCountryRsponse(this Country country)
            {
                return new CountryResponse()
                {
                    CountryId = country.CountryId,
                    CountryName = country.CountryName
                };
            }
    }
    
}
