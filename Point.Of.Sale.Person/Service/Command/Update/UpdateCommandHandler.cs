using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Person.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Person.Service.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.Update(new UpsertPerson
        {
            Id = request.Id,
            TenantId = request.TenantId,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Suffix = request.Suffix,
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            Address = request.Address,
            Email = request.Email,
            IsUser = request.IsUser,
            UserDetails = request.UserDetails
        }, cancellationToken);

        return ResultsTo.Success();
    }
}
