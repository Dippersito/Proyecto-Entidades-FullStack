namespace EntidadApi.Models
{
    public class TelefonoContacto
    {
        public int TelefonoID { get; set; }
        public int EntidadID { get; set; }
        public string Numero { get; set; } = string.Empty;
    }
}