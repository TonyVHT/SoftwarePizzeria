﻿namespace ItaliaPizza.Server.DTOs
{
    public class ClienteConsultaDTO
    {
        
            public int Id { get; set; }

            public string NombreCompleto { get; set; } = string.Empty;

            public string Telefono { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;
        
    }
}
