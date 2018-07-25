using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace WebApiUploadFile.Controllers
{
	/// <summary>
	/// 超级简单的文件上传
	/// </summary>
	public class EasyUploadFileController : ApiController
    {
		/// <summary>
		/// 上传文件
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public string PostWithFile()
		{
			UploadWithFile(HttpContext.Current.Request);
			return "文件上传成功";
		}

		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="request"></param>
		private void UploadWithFile(HttpRequest request)
		{
			for (int i = 0; i < request.Files.Count; i++)
			{
				var file = request.Files[i];
				if (file.ContentLength <= 0) continue;
				var ext = new FileInfo(file.FileName).Extension;
				var fullPath = Path.Combine(@"G:\Image", Path.GetFileName(Guid.NewGuid() + ext));
				file.SaveAs(fullPath);
			}
		}

	}
}
