﻿namespace MovieTracker.Models;

public class Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public Response() { }

    public Response(bool success, string message, T data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}