namespace Brewtal.Dtos
{
    public class CommandResultDto
    {
        public bool Success { get; set; }
        public string[] ErrorMessages { get; set; }
        public int? Id { get; set; }
    }

}