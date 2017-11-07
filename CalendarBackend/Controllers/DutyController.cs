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
    public class DutyController : Controller
    {
        public DutyController(IMediator mediator)
        {
            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public IMediator Mediator { get; }

        [Route("user")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]AddUserToDutyCommand value, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(value, cancellationToken);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var entries = await this.Mediator.Send(new GetAllDutyEntries(), cancellationToken);
            return this.Ok(entries);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entry = await this.Mediator.Send(new GetConcreteDutyEntry(id), cancellationToken);

                return Ok(entry);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddDutyEntryCommand value, CancellationToken cancellationToken = default)
        {
            var id = await this.Mediator.Send(value, cancellationToken);
            return this.CreatedAtAction(nameof(this.Get), new { id = id }, id);
        }

        [Route("user")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody]RemoveUserFromDutyCommand value, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(value, cancellationToken);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }

        [Route("substitute")]
        [HttpPut]
        public async Task<IActionResult> Substitute([FromBody]AddDutySubstitutionCommand value, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(value, cancellationToken);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }
    }
}
