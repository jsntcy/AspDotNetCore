namespace CityInfo.API.Controllers
{
    using AutoMapper;
    using CityInfo.API.Models;
    using CityInfo.API.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDataStore.Current.Cities);

            var cityEntities = _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest)
        {
            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);
            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);

            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);

                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}