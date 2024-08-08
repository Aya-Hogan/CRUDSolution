using Entities;
using ServiceContracts.DTO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage ="person ID can not be blank")]  
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "Person Name Can not be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email Can not be blank")]
        [EmailAddress(ErrorMessage = "Email should be a valid email")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewLetters { get; set; }

        public Person ToPerson()
        {

            return new Person()
            {
                PersonId = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                CountryId = CountryId,
                ReceiveNewLetters = ReceiveNewLetters,

            };
        }

    }
}
