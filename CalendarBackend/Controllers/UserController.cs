namespace CalendarBackend.Controllers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Application.Queries;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [Route("api/[controller]"), Authorize(Policy = "TeamMember")]
    public class UserController : Controller
    {
        public UserController(IMediator mediator)
        {
            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public IMediator Mediator { get; }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            await Mediator.Send(new DeleteUserCommand(id), cancellationToken).ConfigureAwait(false);
            return this.NoContent();
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var entries = await Mediator.Send(new GetAllUsers(), cancellationToken).ConfigureAwait(false);
            return this.Ok(entries);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entries = await Mediator.Send(new GetConcreteUser(id), cancellationToken).ConfigureAwait(false);

                return Ok(entries);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddUserCommand value, CancellationToken cancellationToken = default)
        {
            var id = await Mediator.Send(value, cancellationToken).ConfigureAwait(false);
            return this.CreatedAtAction(nameof(this.Get), new { id }, id);
        }
    }
}
