﻿namespace Template.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}