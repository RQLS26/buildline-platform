namespace Buildline.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>Resource accepted by message create and partial update endpoints.</summary>
public record MessageWriteResource(
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
