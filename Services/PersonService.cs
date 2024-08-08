using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesServices();
            if (initialize)
            {
                _persons.Add(new Person() { PersonId = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", ReceiveNewLetters = false, CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

                _persons.Add(new Person() { PersonId = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", ReceiveNewLetters = true, CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonId = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", ReceiveNewLetters = false, CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonId = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", ReceiveNewLetters = true, CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonId = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", ReceiveNewLetters    = false, CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonId = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", ReceiveNewLetters = false, CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                _persons.Add(new Person() { PersonId = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", ReceiveNewLetters = true, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", ReceiveNewLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", ReceiveNewLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", ReceiveNewLetters = false, CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person newperson)
        {
            PersonResponse personresponse = newperson.ToPersonResponse();
            personresponse.Country =
                _countriesService.GetCountryByID
                (newperson.CountryId)?.CountryName;
            return personresponse;

        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //i want to ask
            ValidationHelper.ModelValidation(personAddRequest);
            

           Person newperson = personAddRequest.ToPerson();

            newperson.PersonId = Guid.NewGuid();

            _persons.Add(newperson);

            return ConvertPersonToPersonResponse(newperson);

        }
        
        public List<PersonResponse> GetAllPersons()
        {
         List<PersonResponse> persons= _persons.Select(temp =>ConvertPersonToPersonResponse(temp)).ToList();
            return persons;
        }

        public PersonResponse? GetPersonById(Guid? id)
        {
            if(id == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault( temp =>
            temp.PersonId == id);
            if(person == null)
            {
                return null;
            }
            PersonResponse personResponse =ConvertPersonToPersonResponse(person);
            return personResponse;
        }

        public List<PersonResponse> GetSortedPersons(string? searchBy, string? SearchString)
        {
            List<PersonResponse> allpersons = GetAllPersons();
            List<PersonResponse> matchingPerson = allpersons;

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(SearchString))
            {
                return matchingPerson;
            }

            switch(searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPerson = allpersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains
                    (SearchString, StringComparison.OrdinalIgnoreCase):true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPerson = allpersons.Where(temp =>
                   (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains
                   (SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPerson = allpersons.Where(temp =>
                   (temp.DateOfBirth !=null ? temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains
                   (SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPerson = allpersons.Where(temp =>
                   (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains
                   (SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.CountryId):
                    matchingPerson = allpersons.Where(temp =>
                   (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains
                   (SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Address):
                    matchingPerson = allpersons.Where(temp =>
                   (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains
                   (SearchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:matchingPerson = allpersons;
                    break;

            }
            return matchingPerson;
        }

        /// <summary>
        /// return sorted list of person
        /// </summary>
        /// <param name="allpersons"> is it a list of peson to sort</param>
        /// <param name="sortBy"> the base of sorting </param>
        /// <param name="sortOrder"> ASC DESC</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allpersons;
            }
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {

                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.PersonName,StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                  => allpersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                 => allpersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                => allpersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                 => allpersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                 => allpersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewLetters), SortOrderOptions.ASC)
                 => allpersons.OrderBy(temp => temp.ReceiveNewLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewLetters), SortOrderOptions.DESC)
                => allpersons.OrderByDescending(temp => temp.ReceiveNewLetters).ToList(),
                _ => allpersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);

            if (person == null)
                throw new ArgumentException("Given person id does not exist");

            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.CountryId = personUpdateRequest.CountryId;
            person.ReceiveNewLetters = personUpdateRequest.ReceiveNewLetters;
            person.Address = personUpdateRequest.Address;

            return ConvertPersonToPersonResponse(person);
        }

        public bool DeletePerson(Guid? id)
        {
            if(id == null) throw new ArgumentNullException("id should not be null");

            Person? person = _persons.FirstOrDefault(temp =>temp.PersonId == id);

            if (person == null) {  return false; }

            var reponse = _persons.Remove(person);
            //another way _persons.RemoveAll(temp => temp.PersonId == id);
            return (reponse);

        }
    }
}
