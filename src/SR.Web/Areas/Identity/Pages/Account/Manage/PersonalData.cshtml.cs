﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SR.Web.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/account/login");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/account/login");
        }
    }
}
