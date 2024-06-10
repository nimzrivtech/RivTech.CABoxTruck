using RivTech.CABoxTruck.WebService.DomainEntities;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BackgroundServiceChecker : IBackgroundServiceChecker
    {
        private bool _isFromBackgroundService = false;

        public void TagAsBackgroundService()
        {
            _isFromBackgroundService = true;
        }

        public bool IsFromBackgroundService() => _isFromBackgroundService; 
    }
}
