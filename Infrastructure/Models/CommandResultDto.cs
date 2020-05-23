using Newtonsoft.Json;
namespace Brewtal2.Infrastructure.Models
{
    public class CommandResultDto
    {
        public dynamic Data { get; set; }
        public bool Success { get; set; }
        public string[] ErrorMessages { get; set; }
        public string[] Messages { get; set; }
    }

}