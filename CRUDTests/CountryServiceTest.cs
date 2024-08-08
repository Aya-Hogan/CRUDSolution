using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;


namespace CRUDTests
{
    
   public class CountryServiceTest
    {
        private readonly ICountriesService _countryService;
        public CountryServiceTest()
        {
            _countryService = new CountriesServices(false);
        }


        #region AddCountry
        [Fact]
        public void AddCounrty_NullCountry()
        {
            //arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        [Fact]
        public void AddCounrty_CountryNameIsNull()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });

        }

        [Fact]
        public void AddCounrty_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request2);
            });

        }
        [Fact]
        public void AddCounrty_ProperCountryDetails()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

           CountryResponse response = _countryService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountriees =
                _countryService.GetAllCountries();


            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountriees);

        }
        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_country_response_list =
                _countryService.GetAllCountries();

            Assert.Empty(actual_country_response_list);

        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new
            List<CountryAddRequest>()
            { 
            new CountryAddRequest(){CountryName = "USA"},
            new CountryAddRequest(){CountryName = "UK"}
            };

            List<CountryResponse> countries_list_from_add_country = new
                List<CountryResponse>();

            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add
                (_countryService.AddCountry(country_request));
            }

           List <CountryResponse> actualCountryResponseList = _countryService.GetAllCountries();

           foreach (CountryResponse expected_country in 
                countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
      

        }
        #endregion

        #region GetCountryByID

        [Fact]
        public void GetCountryByID_NullCountryID()
        {
            Guid? countryID = null;

            CountryResponse? countryResponse_from_get_fun =
            _countryService.GetCountryByID(countryID);

            Assert.Null(countryResponse_from_get_fun);
        }


        [Fact]

        public void GetCountryBYid_ValidationCountryID()
        {
            CountryAddRequest? country_add_request = new
            CountryAddRequest()
            { CountryName = "China" };
            CountryResponse? countryResponse_from_add_request = 
                _countryService.AddCountry(country_add_request);

            CountryResponse? countryResponse_from_get_ = 
            _countryService.GetCountryByID(countryResponse_from_add_request.CountryId);

            Assert.Equal(countryResponse_from_get_, countryResponse_from_add_request);

        }

        #endregion

    }
}
