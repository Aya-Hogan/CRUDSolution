using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using System;


namespace ServiceContracts
{
    public  interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonById(Guid? id);

        List<PersonResponse> GetSortedPersons(string? searchBy, string? SearchString); 
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons,
            string sortBy,SortOrderOptions sortOrder);

        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        bool DeletePerson(Guid? id);
    }
}
