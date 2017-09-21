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
    public class OutOfOfficeController : Controller
    {
        public OutOfOfficeController(IMediator mediator)
        {
            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public IMediator Mediator { get; }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(new DeleteOufOfOfficeEntryCommand(id), cancellationToken).ConfigureAwait(false);
            return this.NoContent();
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var entries = await this.Mediator.Send(new GetAllOutOfOfficeEntries(), cancellationToken);
            return this.Ok(entries);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entry = await this.Mediator.Send(new GetConcreteOutOfOfficeEntry(id), cancellationToken);

                return Ok(entry);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddOutOfOfficeEntryCommand value, CancellationToken cancellationToken = default)
        {
            var id = await this.Mediator.Send(value, cancellationToken);
            return this.CreatedAtAction(nameof(this.Get), new { id = id }, id);
        }

        [Route("reschedule")]
        [HttpPut]
        public async Task<IActionResult> Reschedule([FromBody]RescheduleOutOfOfficeEntryCommand value, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(value, cancellationToken);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.Id });
        }
        
        [Route("rereason")]
        [HttpPut]
        public async Task<IActionResult> Rereason([FromBody]RereasonOutOfOfficeEntryCommand value, CancellationToken cancellationToken = default)
        {
            await this.Mediator.Send(value, cancellationToken);
            return this.RedirectToActionPermanent(nameof(this.Get), new { id = value.Id });
        }
    }
}
