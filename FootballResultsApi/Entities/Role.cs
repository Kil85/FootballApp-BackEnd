﻿namespace FootballResultsApi.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public Roles Name { get; set; } = Roles.User;
    }
}
