namespace FlowTask.Api.GraphQL.Queries.Reports;

using FlowTask.Application.Features.Reports.DTOs;
using FlowTask.Application.Features.Reports.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Query")]
public class ReportQuery
{
    [Authorize]
    public async Task<DashboardStatsDto> GetDashboardStats(
        int workspaceId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDashboardStatsQuery(workspaceId), ct);
}