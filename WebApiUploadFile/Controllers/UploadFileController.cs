using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Http;
using WebApiUploadFile.Models;

namespace WebApiUploadFile.Controllers
{
	public class UploadFileController : ApiController
	{
		/// <summary>
		/// 8M
		/// </summary>
		public const int MAX_LENGTH = 1024 * 1024 * 8;
		public const int MIN_WIDTH = 600;
		public const int MIN_HEIGHR = 600;

		/// <summary>
		/// 上传文件
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public string PostWithFile()
		{
			JObject jObject = GetParam(HttpContext.Current.Request);
			ParamModel model = GetParamModel(jObject);

			Tuple<bool, string> upload = UploadWithFile(HttpContext.Current.Request);
			return upload.Item2 + " 参数：" + JsonConvert.SerializeObject(model);
		}

		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="request"></param>
		private Tuple<bool, string> UploadWithFile(HttpRequest request)
		{
			bool success = true;
			string message = string.Empty;
			for (int i = 0; i < request.Files.Count; i++)
			{
				var file = request.Files[i];
				if (file.ContentLength <= 0) continue;

				#region check的代码
				if (!FitFileLength(file))
				{
					message = message + file.FileName + "文件大小不能超过8M;";
					continue;
				}

				if (!FitImageSize(file))
				{
					message = message + file.FileName + "图片长宽分别不能小于600像素;";
					continue;
				}
				#endregion

				var ext = new FileInfo(file.FileName).Extension;
				var fullPath = Path.Combine(@"G:\Image", Path.GetFileName(Guid.NewGuid() + ext));
				file.SaveAs(fullPath);
			}

			if (string.IsNullOrEmpty(message))
			{
				message = "上传成功";
			}
			Tuple<bool, string> result = new Tuple<bool, string>(success, message);
			return result;
		}

		/// <summary>
		/// 符合文件大小
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		private bool FitFileLength(HttpPostedFile file)
		{
			bool result = true;
			if (file.ContentLength >= MAX_LENGTH)
			{
				result = false;
			}
			return result;
		}

		/// <summary>
		/// 符合图片尺寸
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		private bool FitImageSize(HttpPostedFile file)
		{
			bool result = true;
			var ext = new FileInfo(file.FileName).Extension;
			if (IsImage(ext))
			{
				Image image = Image.FromStream(file.InputStream);
				if (image.Height <= MIN_HEIGHR || image.Width <= MIN_WIDTH)
				{
					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// 监测是否是图片
		/// </summary>
		/// <param name="ext"></param>
		/// <returns></returns>
		public bool IsImage(string ext)
		{
			bool result = false;
			switch (ext.ToLower())
			{
				case (".jpg"):
				case (".png"):
				case (".gif"):
				case (".bmp"):
					result = true;
					break;
				default:
					result = false;
					break;
			}
			return result;
		}

		/// <summary>
		/// 获得参数
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private JObject GetParam(HttpRequest request)
		{
			JObject jObject = new JObject();
			foreach (string key in request.Form)
			{
				jObject[key] = request.Form[key];
			}
			return jObject;
		}

		/// <summary>
		/// 简单的Model转换
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private ParamModel GetParamModel(JObject jObject)
		{
			ParamModel model = new ParamModel();
			model.Param1 = jObject["Param1"] != null ? jObject["Param1"].ToString() : "";
			model.Param2 = jObject["Param2"] != null ? jObject["Param2"].ToString() : "";
			model.Param3 = jObject["Param3"] != null ? jObject["Param3"].ToString() : "";

			return model;
		}
	}
}
