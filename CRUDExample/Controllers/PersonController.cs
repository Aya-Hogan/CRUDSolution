using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services;

namespace CRUDExample.Controllers
{
    [Route("[Controller]")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        public PersonController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
		public IActionResult Index(string searchBy, string? searchString,
            string sortBy = nameof (PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            ViewBag.SearchFeilds = new Dictionary<string, string>() 
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "DateOfBirth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryId), "CountryId" },
                { nameof(PersonResponse.Address), "Address" },
            };

           List<PersonResponse> persons = _personService.GetSortedPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSerachString = searchString;

            List<PersonResponse> sortedPersons = 
                _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
			List<CountryResponse> countries = _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(temp => 
            new SelectListItem() {Text = temp.CountryName, Value = temp.CountryId.ToString()});
           // new SelectListItem() {}
			return View();
        }

		[HttpPost]
		[Route("[action]")]
		public IActionResult Create(PersonAddRequest personAddRequest)
		{
			if (!ModelState.IsValid)
			{
				List<CountryResponse> countries = _countriesService.GetAllCountries();
				ViewBag.Countries = countries;

				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View();
			}

			//call the service method
			PersonResponse personResponse = _personService.AddPerson(personAddRequest);

			//navigate to Index() action method (it makes another get request to "persons/index"
			return RedirectToAction("Index", "Person");
		}
	}
}

