namespace CalendarBackend.Controllers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Application.Queries;
    using CalendarBackend.Models;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

#pragma warning disable CA1062 // Argumente von öffentlichen Methoden validieren
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
            await Mediator.Send(value, cancellationToken).ConfigureAwait(false);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var entries = await Mediator.Send(new GetAllDutyEntries(), cancellationToken).ConfigureAwait(false);
            return this.Ok(entries);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, NullableLocalDate startAppointmentRange, NullableLocalDate endAppointmentRange, CancellationToken cancellationToken = default)
        {
            try
            {
                var entry = await Mediator.Send(new GetConcreteDutyEntry(id, startAppointmentRange?.Date, endAppointmentRange?.Date), cancellationToken).ConfigureAwait(false);

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
            var id = await Mediator.Send(value, cancellationToken).ConfigureAwait(false);
            return this.CreatedAtAction(nameof(this.Get), new { id }, id);
        }

        [Route("user")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromBody]RemoveUserFromDutyCommand value, CancellationToken cancellationToken = default)
        {
            await Mediator.Send(value, cancellationToken).ConfigureAwait(false);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }

        [Route("substitute")]
        [HttpPut]
        public async Task<IActionResult> Substitute([FromBody]AddDutySubstitutionCommand value, CancellationToken cancellationToken = default)
        {
            await Mediator.Send(value, cancellationToken).ConfigureAwait(false);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.DutyId });
        }
    }
#pragma warning restore CA1062 // Argumente von öffentlichen Methoden validieren
}
