using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FTPClientServerDTO
    {
        public string BaseUrl { get; set; }
        public int Port { get; set; }
        public bool IsSsh { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Directory { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
