using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using UserRazorPage.Data;
using UserRazorPage.Entities;
using static System.Net.WebRequestMethods;

namespace UserRazorPage.Pages.Shared
{
    public class AddModel : PageModel
    {
        public Employee Employee { get; set; }

        private readonly MyDbContext _context;

        public AddModel(MyDbContext context)
        {
            _context = context;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }

        private string GenKey()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string key = new string(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
            string hash = CreateMD5(key);

            return hash;
        }

        private string GenImg()
        {
            string randKey = GenKey().ToLower();
            //string path = $"https://gravatar.com/avatar/?s=400&d=robohash&r=x";
            string path = "h" + $"ttps://gravatar.com/avatar/{randKey}?s=400&d=robohash&r=x";
            return path;

        }

        public void OnGet(int id = 0)
        {
            Employee = _context.Employees.FirstOrDefault(e => e.Id == id) ?? new Employee();
        }


        public IActionResult OnPost(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.Id > 0)
                {
                    _context.Employees.Update(employee);
                }
                else
                {
                    //checked if image path is empty
                    if (employee.PhotoPath == null)
                    {
                        employee.PhotoPath = GenImg();
                    }
                    _context.Employees.Add(employee);
                }
                _context.SaveChanges();
                return RedirectToPage("index");
            }
            return Page();

        }
    }
}
