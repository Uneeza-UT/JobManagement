using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base($"{name} ({key}) not found.")
    {
    }
}
