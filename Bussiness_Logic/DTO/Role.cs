﻿namespace Domain.DTO
{
    public class Role
    {
        public string Name { get; set; } = string.Empty;        

        public string Department { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string[] Location { get; set; } = [];        
    }
}
