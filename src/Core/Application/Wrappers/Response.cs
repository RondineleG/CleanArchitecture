﻿using System.Collections.Generic;

namespace Application.Wrappers;

public class Response<T>
{
    public Response()
    {
    }

    public Response(T data, string message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }

    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }

    public T Data { get; set; }

    public List<string> Errors { get; set; }

    public string Message { get; set; }

    public bool Succeeded { get; set; }
}