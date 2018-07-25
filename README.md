# WebApiUploadFile
Web Api上传文件

### 核心代码
WebApiUploadFile/WebApiUploadFile/Controllers/

```
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


```
