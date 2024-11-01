using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.RestaurantsData.DTO;
using MediatR;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantByID;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurantCommand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;



namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/resturants")]

    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDTO>))]
        public async Task<IActionResult> GetAllRestaurants([FromQuery] GetllAllRestaurantQuery query)
        {
            var restaurants = await mediator.Send(query);


            return Ok(restaurants);
        }
        [HttpGet]
        [Route("{id}")]
        //[Authorize(Policy = "HasNationality")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] int id)
        {

            var restaurant = await mediator.Send(new GetRestaurantByIDQuery()
            {
                Id = id
            }

            );
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {

            await mediator.Send(new DeleteRestaurantCommand(id));


            return NoContent();



        }
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);


            return NoContent();

        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
        {
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetRestaurantById), new { id }, null);
        }


    }
}