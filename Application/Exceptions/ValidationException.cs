using System;
using System.Collections.Generic;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.") => Errors = [];

    public ValidationException(IEnumerable<string> errors) : this() => Errors.AddRange(errors);

    public List<string> Errors { get; }
}