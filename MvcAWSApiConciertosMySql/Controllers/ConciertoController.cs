using Microsoft.AspNetCore.Mvc;
using MvcAWSApiConciertosMySql.Models;
using MvcAWSApiConciertosMySql.ServiceApiConcierto;

namespace MvcAWSApiConciertosMySql.Controllers
{
    public class ConciertoController : Controller
    {
        private ServiceStorageS3 serviceStorage;
        private ServiceApiConcierto.ServiceApiConcierto service;
        private string BucketUrl;
        public ConciertoController(ServiceApiConcierto.ServiceApiConcierto service,IConfiguration configuration,ServiceStorageS3 serviceStorage,KeysModel model)
        {
            this.service = service;
            this.serviceStorage = serviceStorage;
            this.BucketUrl = model.S3Bucket;
        }


        public async  Task<IActionResult> Index()
        {
            List<Eventos> eventos = await this.service.GetEventos();
            List<string> filesS3 = await this.serviceStorage.GetVersionsFilesAsync();
            ViewData["BUCKETURL"] = this.BucketUrl;

            return View(eventos);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Eventos eventos, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceStorage.UploadFileAsync(file.FileName, stream);
            }

             await this.service.CreateConcierto(eventos.Nombre,eventos.Artista,eventos.IdCategoria,file.FileName);
            return RedirectToAction("Index");
        }

    }
}
