using System;
using System.Collections.Generic;

namespace ManageUser.Models;

public partial class UserProfile
{
    public int UserProfileId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public bool Sex { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Avatar { get; set; }

    public virtual User User { get; set; } = null!;
}

