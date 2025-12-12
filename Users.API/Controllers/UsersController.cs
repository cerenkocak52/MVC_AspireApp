using CORE.APP.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Users;

namespace Users.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public Task<CommandResponse> Create(UserCreateRequest request)
        => _mediator.Send(request);

    [HttpPut]
    public Task<CommandResponse> Update(UserUpdateRequest request)
        => _mediator.Send(request);

    [HttpDelete("{id}")]
    public Task<CommandResponse> Delete(int id)
        => _mediator.Send(new UserDeleteRequest { Id = id });

    [HttpGet]
    public Task<IQueryable<UserQueryResponse>> Get()
        => _mediator.Send(new UserQueryRequest());
    }