﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Models.Incoming;

public class RoleRequestModel
{
    public string Id { get; set; }
    public string RoleName { get; set; }
    public DateTime DateCreated { get; set; }
    public BasicUserModel User { get; set; }
}