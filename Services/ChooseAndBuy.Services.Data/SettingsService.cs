namespace ChooseAndBuy.Services.Data
{
    using System.Linq;

    using ChooseAndBuy.Data.Common.Repositories;
    using ChooseAndBuy.Data.Models;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return this.settingsRepository.All().Count();
        }
    }
}
