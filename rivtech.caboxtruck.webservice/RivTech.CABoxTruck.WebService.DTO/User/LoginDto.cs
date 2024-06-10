namespace RivTech.CABoxTruck.WebService.DTO.User
{
    public class LoginDto
    {
        public string Username { get; set; }

        // Client details
        public string DeviceType { get; set; }
        public string OS { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string AppVersion { get; set; }
    }
}
