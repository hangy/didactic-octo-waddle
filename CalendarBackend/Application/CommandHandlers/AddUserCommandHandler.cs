namespace CalendarBackend.Application.CommandHandlers
{
    using CalendarBackend.Application.Commands;
    using CalendarBackend.Domain.AggregatesModel.UserAggregate;
    using MediatR;
    using System;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;

        public AddUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Guid> Handle(AddUserCommand message, CancellationToken cancellationToken)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var aggregate = await this.userRepository.AddAsync(new User(Guid.NewGuid(), message.UserName, message.DisplayName, new MailAddress(message.MailAddress), new UserColor(message.Color)), cancellationToken).ConfigureAwait(false);

            return aggregate.Id;
        }
    }
}
