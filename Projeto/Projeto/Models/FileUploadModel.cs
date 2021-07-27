using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Projeto.Models
{
    public class FileUploadModel
    {
        public IFormFile file { get; set; }
    }
}
