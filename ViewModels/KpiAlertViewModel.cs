namespace Dashboard.ViewModels
{
    public enum AlertLevel
    {
        Info,       // Bleu ou neutre
        Warning   // Jaune

    }
    public class KpiAlertViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public AlertLevel Severity { get; set; }
    }
}
