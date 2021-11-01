using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapperNet5Test.Models;

namespace AutoMapperNet5Test.Controllers
{
    using AutoMapperNet5Test.AutoMapper;
    using AutoMapperNet5Test.AutoMapper.Models;
    using AutoMapperNet5Test.AutoMapper.ViewModels;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Random random = new Random();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = this.GetModel();
            var vm = model.Map<TryViewModel>();
            var models = this.GetModels();
            var vms = models.MapCollection<TryViewModel>(new { prefix = "Mr." });

            return this.View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public TryModel GetModel()
            => new TryModel
            {
                Id = random.Next(1, 1000),
                FirstName = "Doncho",
                LastName = "Minkov",
            };

        public IQueryable<TryModel> GetModels()
            => new[]
                {
                    this.GetModel(),
                    this.GetModel(),
                    this.GetModel(),
                    this.GetModel(),
                }
                .AsQueryable();
    }
}