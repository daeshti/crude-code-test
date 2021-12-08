using System.Threading.Tasks;
using CrudTest.Application.Common.Models;
using CrudTest.Application.Customers.Commands.CreateCustomer;
using CrudTest.Application.Customers.Commands.DeleteCustomer;
using CrudTest.Application.Customers.Commands.UpdateCustomer;
using CrudTest.Application.Customers.Queries.GetCustomerById;
using CrudTest.Application.Customers.Queries.GetCustomersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace CrudTest.Presentation.Server.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<CustomerBriefDto>>> GetTodoItemsWithPagination(
            [FromQuery] GetCustomersWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            return await Mediator.Send(new GetCustomerByIdQuery() { CustomerId = id });
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateCustomerCommand command)
        {
            return await Mediator.Send(command);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCustomerCommand command)
        {
            if (id != command.CustomerId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteCustomerCommand() { CustomerId = id });

            return NoContent();
        }
    }
}