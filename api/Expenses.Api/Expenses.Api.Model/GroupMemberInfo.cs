namespace Expenses.Api.Model;

public sealed record GroupMemberInfo(Guid Id, Guid GroupId, Guid? UserId, String? Name, DateTimeOffset JoinedAt);