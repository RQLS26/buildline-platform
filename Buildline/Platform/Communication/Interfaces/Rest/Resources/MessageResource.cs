namespace Buildline.Platform.Communication.Interfaces.Rest.Resources;

/// <summary>REST resource returned by message endpoints.</summary>
public record MessageResource(
    int Id,
    int CompanyId,
    string Sender,
    string Subject,
    string Preview,
    string Icon,
    string IconClass,
    string Label,
    string LabelClass,
    bool IsRead,
    bool Starred,
    string Category,
    string Time,
    string Date);
