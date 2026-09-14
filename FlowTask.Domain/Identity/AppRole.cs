namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class AppRole : IdentityRole<int>
{
    [Column("description")]
    public string? Description { get; set; }
}

public class AppUserClaim  : IdentityUserClaim<int>  { }
public class AppUserRole   : IdentityUserRole<int>   { }
public class AppUserLogin  : IdentityUserLogin<int>  { }
public class AppUserToken  : IdentityUserToken<int>  { }
public class AppRoleClaim  : IdentityRoleClaim<int>  { }