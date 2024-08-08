using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testoutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countriesService = new CountriesServices(false);
            _testoutputHelper = testOutputHelper;
        }

        #region AddPerson 

        [Fact]

        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        
        }

        [Fact]

        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new
                PersonAddRequest()
            {
                PersonName = null,
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });

        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new
                PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            PersonResponse person_response_from_add =
                _personService.AddPerson(personAddRequest);
            List<PersonResponse> person_list =
                _personService.GetAllPersons();

            Assert.True(person_response_from_add.PersonId != Guid.Empty);
            Assert.Contains(person_response_from_add, person_list);

        }

        #endregion

        #region GetPersonID

        [Fact]
        public void GetPersonById_NullPersonId()
        {
            Guid? personId = null;

            PersonResponse?
                person_response_from_get = 
                _personService.GetPersonById(personId);
            Assert.Null(person_response_from_get);

        }
        [Fact]
        public void GetPersonBy_withpersonId()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            { CountryName = "Canada" };
            CountryResponse country_response = 
                _countriesService.AddCountry(country_request);

            PersonAddRequest person_rquest = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = Guid.NewGuid(),
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            PersonResponse person_respone_from_add = _personService.AddPerson(person_rquest);

            PersonResponse? person_respone_from_get =
                _personService.GetPersonById(person_respone_from_add.PersonId);

            Assert.Equal(person_respone_from_add, person_respone_from_get);


        }

        #endregion

        #region GetAllperosn

        [Fact]
        public void GettAllPersons_EmptyList()
        {
            List<PersonResponse> persons = _personService.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]  
        public void GetAllPerson_withadding()
        {

            CountryAddRequest country_req_1 = new CountryAddRequest()
            { CountryName="USA"};

            CountryAddRequest country_req_2 = new CountryAddRequest()
            { CountryName = "India" };

            CountryResponse country_res1 =  _countriesService.AddCountry(country_req_1);
            CountryResponse country_res2 = _countriesService.AddCountry(country_req_2);



            PersonAddRequest person_add_1 = new PersonAddRequest() 
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true,
            };
            
           
            
            PersonAddRequest person_add_2 = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            {person_add_1,person_add_2 };

            List<PersonResponse> persons_from_response = new List<PersonResponse>();


            foreach(var person_req in person_request)
            {
                PersonResponse person_response = _personService.AddPerson(person_req);
                persons_from_response.Add(person_response);
            }

            _testoutputHelper.WriteLine("Expected");
            foreach(PersonResponse person_res_from_add in persons_from_response)
            {
                _testoutputHelper.WriteLine(person_res_from_add.ToString());
            }

            List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();

            _testoutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_res_from_get in persons_list_from_get)
            {
                _testoutputHelper.WriteLine(person_res_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in persons_from_response)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }



        }


        #endregion

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {

            CountryAddRequest country_req_1 = new CountryAddRequest()
            { CountryName = "USA" };

            CountryAddRequest country_req_2 = new CountryAddRequest()
            { CountryName = "India" };

            CountryResponse country_res1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_res2 = _countriesService.AddCountry(country_req_2);



            PersonAddRequest person_add_1 = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true,
            };



            PersonAddRequest person_add_2 = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            {person_add_1,person_add_2 };

            List<PersonResponse> persons_from_response = new List<PersonResponse>();


            foreach (var person_req in person_request)
            {
                PersonResponse person_response = _personService.AddPerson(person_req);
                persons_from_response.Add(person_response);
            }

            _testoutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_res_from_add in persons_from_response)
            {
                _testoutputHelper.WriteLine(person_res_from_add.ToString());
            }

            List<PersonResponse> persons_list_from_search = _personService.GetSortedPersons(nameof(Person.PersonName),"");

            _testoutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_res_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_res_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in persons_from_response)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }
        }


        [Fact]
        public void GetFilteredPersons_SearchByName()
        {

            CountryAddRequest country_req_1 = new CountryAddRequest()
            { CountryName = "USA" };

            CountryAddRequest country_req_2 = new CountryAddRequest()
            { CountryName = "India" };

            CountryResponse country_res1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_res2 = _countriesService.AddCountry(country_req_2);



            PersonAddRequest person_add_1 = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res1.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true,
            };



            PersonAddRequest person_add_2 = new PersonAddRequest()
            {
                PersonName = "ayah",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_res2.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            {person_add_1,person_add_2 };

            List<PersonResponse> persons_from_response = new List<PersonResponse>();


            foreach (var person_req in person_request)
            {
                PersonResponse person_response = _personService.AddPerson(person_req);
                persons_from_response.Add(person_response);
            }

            _testoutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_res_from_add in persons_from_response)
            {
                _testoutputHelper.WriteLine(person_res_from_add.ToString());
            }

            List<PersonResponse> persons_list_from_search = _personService.GetSortedPersons(nameof(Person.PersonName), "ay");

            _testoutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_res_from_get in persons_list_from_search)
            {
                _testoutputHelper.WriteLine(person_res_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in persons_from_response)
            {
                if (person_response_from_add is not null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                    }
                }
            }



        }




        #endregion

        #region GetSortedPersons

        // when we sort based on personName in DESC

        [Fact]

        public void GetSortedPersons()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testoutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testoutputHelper.WriteLine(person_response_from_add.ToString());
            }
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print persons_list_from_get
            _testoutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testoutputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }


        #endregion

        #region UpdatePeson

        [Fact]
        public void UpdatePerson_NullPeron()
        {

            PersonUpdateRequest? person_update_request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.UpdatePerson(person_update_request);
            });

        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {

            PersonUpdateRequest? person_update_request = new
                PersonUpdateRequest(){ PersonId = Guid.NewGuid()};

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(person_update_request);
            });

        }

        [Fact]
        public void UpdatePerson_PersonNameNUll()
        {
            CountryAddRequest country =  new CountryAddRequest() { CountryName="USA"};
            CountryResponse country_response_from_addrequest = _countriesService.AddCountry(country);
            PersonAddRequest person = new PersonAddRequest() 
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_response_from_addrequest.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true

            };
            PersonResponse person_response_fromaddrequest = _personService.AddPerson(person);

            PersonUpdateRequest? person_update_request = person_response_fromaddrequest.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(person_update_request);
            });
            }

        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            CountryAddRequest country = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_response_from_addrequest = _countriesService.AddCountry(country);
            PersonAddRequest person = new PersonAddRequest()
            {
                PersonName = "aya",
                CountryId = country_response_from_addrequest.CountryId,
                Email = "balbla@gmail.com",
                Address = "usa",
                ReceiveNewLetters = true,
                Gender = GenderOptions.Female

            };
            PersonResponse person_response_fromaddrequest = _personService.AddPerson(person);

            PersonUpdateRequest person_update_request = person_response_fromaddrequest.ToPersonUpdateRequest();
            person_update_request.PersonName = "Diya";
            person_update_request.Email = "diya@gmail.com";

            PersonResponse person_respons_from_update = _personService.UpdatePerson(person_update_request);

            PersonResponse? person_respons_from_get = _personService.GetPersonById(person_update_request.PersonId);

            Assert.Equal(person_respons_from_update, person_respons_from_get);


        }


        #endregion

        #region DeletePerson

        [Fact]  
        public void  DeletePerson_valid_Id()
        {
            
            CountryAddRequest country = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_fromadd = _countriesService.AddCountry(country);

            PersonAddRequest person = new PersonAddRequest()
            {
                PersonName = "aya",
                Email = "aya@exmple.com",
                Address = "sample",
                CountryId = country_response_fromadd.CountryId,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewLetters = true
            };

            PersonResponse person_reponse_fromadd = _personService.AddPerson(person);

            var isDeleted = _personService.DeletePerson(person_reponse_fromadd.PersonId);
            Assert.True(isDeleted);

        }

        
        [Fact]
        public void DeletePerson_Invalid_Id()
        {
           
            var isDeleted = _personService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);

        }

        #endregion

    }
}

