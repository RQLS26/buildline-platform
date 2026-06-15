namespace Buildline.Platform.Iam.Domain.Model;

public enum IamError
{
    UserNotFound,
    EmailAlreadyTaken,
    InvalidCredentials,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
