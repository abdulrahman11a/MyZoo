namespace Clinic.Core.Entities.Identity
{
        [Owned]// "owned" by another entity ( This table is part of another table) | AppUser.
    public class RefreshToken
        {
            public string Token { get; set; } = null!;
            public DateTime ExpireAt { get; set; }
            public bool IsExpired => DateTime.UtcNow >= ExpireAt;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public DateTime? RevokedAt { get; set; }
            public bool IsActive => RevokedAt == null && !IsExpired;
        }
    
}
