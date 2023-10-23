using Domain.Common;
using Shared.Helpers;

namespace Domain.ClientAggregate.Valueobjects
{
    public class Name : ValueObject
    {
        public string FirstName { get; private init; }

        public string LastName { get; private init; }

        public string FullName => $"{FirstName} {LastName}";

        public static Result<Name> Create(string firstName, string lastName)
        {
            var result = Result.Success();
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return Result<Name>.Failure("Nimi ei või olla NULL.");
            }

            firstName = firstName.Trim();
            lastName = lastName.Trim();

            if (firstName.Length < 1)
            {
                result = result.Merge(Result<Name>.Failure("Eesnimi peab olema vähemalt üks täht pikk"));
            }

            if (lastName.Length < 1)
            {
                result = result.Merge(Result<Name>.Failure("Perekonnanimi peab olema vähemalt üks täht pikk."));
            }

            if (lastName.Any(char.IsDigit) || firstName.Any(char.IsDigit))
            {
                result = result.Merge(Result.Failure("Nimi ei või sisaldada numbreid."));
            }

            return result.OnSuccess(_ => new Name { FirstName = firstName, LastName = lastName });
        }

        private Name()
        {
        }
    }
}
