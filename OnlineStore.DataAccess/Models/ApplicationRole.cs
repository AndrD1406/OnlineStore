﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Models;

public class ApplicationRole : IdentityRole<Guid>
{
}
