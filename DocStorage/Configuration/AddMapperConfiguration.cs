using AutoMapper;
using DocStorage.Model;

namespace DocStorage.Api.Configuration
{
    public static class AddMapperConfiguration
    {
        public static IMapperConfigurationExpression CreateMapper(this IMapperConfigurationExpression cfg)
        {
            //User
            cfg.CreateMap<User, Domain.User>();
            cfg.CreateMap<Domain.User, User>();

            //Group
            cfg.CreateMap<Group, Domain.Group>();
            cfg.CreateMap<Domain.Group, Group>();

            //Document
            cfg.CreateMap<Document, Domain.Document>();
            cfg.CreateMap<Domain.Document, Document>();

            //UserGroup
            cfg.CreateMap<UserGroup, Domain.UserGroup>();
            cfg.CreateMap<Domain.UserGroup, UserGroup>();

            //DocumentAccess
            cfg.CreateMap<DocumentAccess, Domain.DocumentAccess>();
            cfg.CreateMap<Domain.DocumentAccess, DocumentAccess>();

            return cfg;
        }
    }
}
