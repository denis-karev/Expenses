namespace Expenses.Api.Models.GroupMembers;

public record GroupMemberModel(Guid Id, Guid GroupId, Guid? UserId, String? Name, DateTimeOffset JoinedAt);