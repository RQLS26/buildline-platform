namespace Buildline.Platform.Categories.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by material category endpoints.
/// </summary>
/// <param name="Id">Category identifier used by API clients.</param>
/// <param name="Name">Short category name displayed by frontend filters.</param>
/// <param name="Description">Description that explains the material group represented by the category.</param>
public record CategoryResource(int Id, string Name, string Description);
