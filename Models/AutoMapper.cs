using Authentication.System.API.Models;
using Authentication.System.API.Models.Register;
using AutoMapper;


namespace Authentication.System.API.Models
{
    public  class AutoMapperInitializator:Profile
    {
        public AutoMapperInitializator()
        {          
            UserModels();
        }
       
        protected void UserModels()
        {
            CreateMap<User, RegisterFormModel>().ReverseMap().ForMember(x=>x.UserName,m=>m.MapFrom(s=>s.SurName));
        }
       
    }
}
