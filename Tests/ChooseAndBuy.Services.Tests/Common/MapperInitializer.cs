namespace ChooseAndBuy.Services.Tests.Common
{
    using System.Reflection;

    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.ViewModels;

    public static class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ErrorBindingModel).GetTypeInfo().Assembly);
        }
    }
}
