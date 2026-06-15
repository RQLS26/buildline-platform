namespace Buildline.Platform.Communication.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing message aggregate.
/// </summary>
/// <param name="MessageId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="Sender">Write value for the Sender field in the frontend-compatible contract.</param>
/// <param name="Subject">Write value for the Subject field in the frontend-compatible contract.</param>
/// <param name="Preview">Write value for the Preview field in the frontend-compatible contract.</param>
/// <param name="Icon">Write value for the Icon field in the frontend-compatible contract.</param>
/// <param name="IconClass">Write value for the IconClass field in the frontend-compatible contract.</param>
/// <param name="Label">Write value for the Label field in the frontend-compatible contract.</param>
/// <param name="LabelClass">Write value for the LabelClass field in the frontend-compatible contract.</param>
/// <param name="IsRead">Write value for the IsRead field in the frontend-compatible contract.</param>
/// <param name="Starred">Write value for the Starred field in the frontend-compatible contract.</param>
/// <param name="Category">Write value for the Category field in the frontend-compatible contract.</param>
/// <param name="Time">Write value for the Time field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateMessageCommand(
    int MessageId,
    string? Sender,
    string? Subject,
    string? Preview,
    string? Icon,
    string? IconClass,
    string? Label,
    string? LabelClass,
    bool? IsRead,
    bool? Starred,
    string? Category,
    string? Time,
    string? Date);
