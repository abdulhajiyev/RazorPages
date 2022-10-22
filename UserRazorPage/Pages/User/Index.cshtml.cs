using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserRazorPage.Data;
using UserRazorPage.Entities;

namespace UserRazorPage.Pages.User
{
    public class IndexModel : PageModel
    {
        public List<Employee> Employee { get; set; }

        private readonly MyDbContext _context;

        public IndexModel(MyDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Employee = _context.Employees.ToList();
        }

        public IActionResult OnGetDelete(int id = 0)
        {
            Employee employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }

            return RedirectToPage("Index");
        } 
    }
}
