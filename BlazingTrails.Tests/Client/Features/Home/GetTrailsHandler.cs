using AutoFixture;
using BlazingTrails.Shared.Features.Home.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingTrails.Tests.Client.Features.Home;

// Test stub.
public class GetTrailsHandler : IRequestHandler<GetTrailsRequest, GetTrailsRequest.Response>
{
    public async Task<GetTrailsRequest.Response> Handle(GetTrailsRequest request, CancellationToken cancellationToken)
    {
        var fixture = new Fixture();

        // 'AutoFixture' is used to create a list of dummy trails to return.
        var dummyTrails = fixture.CreateMany<GetTrailsRequest.Trail>();

        return new GetTrailsRequest.Response(dummyTrails);
    }
}