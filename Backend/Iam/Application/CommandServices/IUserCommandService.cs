using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default);
    Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default);
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken = default);
    Task<Result<(User user, string token)>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);
}
