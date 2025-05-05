namespace Expenses.Api.Models.GroupMembers;

public sealed record AddGroupMemberSpec(
    EGroupMemberType Type,
    AddGroupMemberOfflineSpec? Offline,
    AddGroupMemberUserSpec? User);