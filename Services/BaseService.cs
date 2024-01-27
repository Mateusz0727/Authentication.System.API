using Authentication.System.API.Data;
using AutoMapper;

namespace Authentication.System.API.Services
{
    public class BaseService
    {
        protected BaseContext Context { get; }
        protected IMapper Mapper { get; }

        public BaseService(IMapper mapper, BaseContext context)
        {
            Mapper = mapper;
            Context = context;

        }
    }
}
