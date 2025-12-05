namespace Prime_Software
{
    public record TokenResponse
    {
        public string Token { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
