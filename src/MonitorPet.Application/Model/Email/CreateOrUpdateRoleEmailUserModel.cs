﻿namespace MonitorPet.Application.Model.Email;

public class CreateOrUpdateRoleEmailUserModel
{
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}
