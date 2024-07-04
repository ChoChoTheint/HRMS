using HRMS.DAO;
using HRMS.Models.DataModels;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace HRMS.Controllers
{
    public class PositionController : Controller
    {
       private readonly HRMSDdContext _dbContext;
       public PositionController(HRMSDdContext dbContext) => _dbContext = dbContext;

        
        public IActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Entry(PositionViewModel positionViewModel)
        {
            try
            {
               
                PositionEntity position = new PositionEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = positionViewModel.Code,
                    Name = positionViewModel.Name,
                    Level   = positionViewModel.Level,
                    
                    Ip = this.GetLocalIPAddress(),
                   
                };
               
                _dbContext.Position.Add(position);
                _dbContext.SaveChanges();
                TempData["info"] = "save successfully the record the system";
            }
            catch (Exception)
            {

                TempData["info"] = "error when saving the record the system";
            }
            return RedirectToAction("list");
        }

        private string  GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public IActionResult List()
        {
            //get the students record by exchanging from data model to view model
            IList<PositionViewModel> positions = _dbContext.Position.Select(s => new PositionViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Level = s.Level,
                
            }).ToList();
            return View(positions); //pass the StudenViewModel to the View(UI)
        }

        public IActionResult Delete(string Id)
        {
            try
            {
                PositionEntity position = _dbContext.Position.Where(w => w.Id == Id).FirstOrDefault();
                if (position is not null)
                {
                    _dbContext.Position.Remove(position);
                    _dbContext.SaveChanges();
                    TempData["info"] = "delete when saving the record the system";

                }
            }
            catch (Exception)
            {

                TempData["info"] = "error when delete the record the system";
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(string Id)
        {
            PositionViewModel positionvm = _dbContext.Position.Where(w => w.Id == Id).Select(s => new PositionViewModel()
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Level = s.Level,
            }).FirstOrDefault();
            return View(positionvm);
        }
        [HttpPost]
        public IActionResult Update(PositionViewModel positionViewModel)
        {
            try
            {
                PositionEntity position = new PositionEntity()
                {
                    Id = positionViewModel.Id,
                    Code = positionViewModel.Code,
                    Name = positionViewModel.Name,
                    Level = positionViewModel.Level,
                    Ip = this.GetLocalIPAddress(),
                    ModifiedAt = DateTime.Now,
                };

                _dbContext.Position.Update(position);
                _dbContext.SaveChanges();
                TempData["info"] = "update successfully the record the system";
            }
            catch (Exception)
            {

                TempData["info"] = "error when updating the record the system";
            }
            return RedirectToAction("list");
        }
    }
}
