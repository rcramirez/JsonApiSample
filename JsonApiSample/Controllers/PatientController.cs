using JsonApiSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private List<Patient> _patients = new List<Patient>
        {
            new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Dylan",
                Address = "Sing Song Lane",
                Age = 30,
                Email = "bob.dylan@gmail.com",
                Phone = "5554443333"
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = "Private",
                LastName = "Ryan",
                Address = "World War Street",
                Age = 28,
                Email = "private.ryan@gmail.com",
                Phone = "9998887777"
            },
            new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = "Tom",
                LastName = "Hanks",
                Address = "Wallstreet Boulevard",
                Age = 52,
                Email = "tom.hanks@gmail.com",
                Phone = "3332221111"
            }
        };
        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public PagedCollectionResponse<Patient> Get([FromQuery] PatientFilterModel filter)
        {
            // Filtering logic  
            Func<PatientFilterModel, IEnumerable<Patient>> filterData = (filterModel) =>
            {
                return _patients.Skip((filterModel.Page - 1) * filter.Limit)
                .Take(filterModel.Limit);
            };

            // Get the data for the current page  
            var response = new PagedCollectionResponse<Patient>
            {
                Items = filterData(filter)
            };

            //Get next page URL string  
            PatientFilterModel nextFilter = filter.Clone() as PatientFilterModel;
            nextFilter.Page += 1;
            var nextUrl = filterData(nextFilter).Count() <= 0 ? null : Url.Action("Get", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            PatientFilterModel previousFilter = filter.Clone() as PatientFilterModel;
            previousFilter.Page -= 1;
            var previousUrl = previousFilter.Page <= 0 ? null : this.Url.Action("Get", null, previousFilter, Request.Scheme);

            response.NextPage = !string.IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            response.PreviousPage = !string.IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return response;
        }

        [HttpGet("{id:Guid}")]
        public Patient Get(Guid id)
        {

            return _patients.Where(p => p.Id.Equals(id)).SingleOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] Patient model)
        {
            _patients.Add(model);
        }

        [HttpDelete("{id:Guid}")]
        public void Delete(Guid id)
        {

        }
    }
}