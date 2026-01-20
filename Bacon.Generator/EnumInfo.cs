namespace Bacon.Generator;

internal record EnumInfo(string Name, string? Namespace, EquatableArray<EnumMemberInfo> Members);