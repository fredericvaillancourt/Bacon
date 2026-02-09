namespace Bacon.Generator;

internal sealed record EnumInfo(string Name, string? Namespace, EquatableArray<EnumMemberInfo> Members);