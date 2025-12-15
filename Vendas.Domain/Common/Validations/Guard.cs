using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Common.Validations;

internal static class Guard
{
    public static void AgainstNullOrEmpty(Guid id, string parameterName, string? message = null)
    {
        if (id == Guid.Empty)
            throw new DomainException(message ?? $"{parameterName} não pode ser Guid.Empty.");
    }

    public static void AgainstNull<T>(T value, string parameterName)
    {
        if (value == null)
            throw new DomainException($"{parameterName} não pode ser nulo.");
    }

    public static void AgainstNull<T>(T value, string parameterName, string message)
    {
        if (value == null)
            throw new DomainException(message);
    }
    public static void AgainstNullorWhiteSpace(string value, string parameterName, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(message ??$"{parameterName} não pode ser nulo ou vazio.");
    }

    public static void Against<TException>(bool condition, string message) where TException : Exception
    {
        if (condition)
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }

    internal static void AgainstNullOrWhite(string cep, string v1, string v2)
    {
        throw new NotImplementedException();
    }
}
