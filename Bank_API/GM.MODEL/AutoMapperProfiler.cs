using AutoMapper;

namespace GM.MODEL
{
    public class AutoMapperProfiler : Profile
    {
        public AutoMapperProfiler()
        {
            AllowNullCollections = true;


        }
    }
}