﻿namespace MicrobloggingApp.AuthenticationModule.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}