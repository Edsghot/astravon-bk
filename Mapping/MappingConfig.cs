﻿using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Domain.Entity;
using Mapster;

namespace Astravon.Mapping;

public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<UserEntity, UserDto>.NewConfig();
    }
}