using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Student_management_api.Services;
using Student_managment_api.Services;
using Student_managmentAPI.Services;

WebHost.CreateDefaultBuilder().
ConfigureServices(s=>
{
    IConfiguration appsettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    s.AddSingleton<Login>();
    s.AddSingleton<Registration>();
    s.AddSingleton<AddTask>();
    s.AddSingleton<GetTasks>();
    s.AddSingleton<TaskUpdate>();
    s.AddSingleton<deleteTask>();
    s.AddSingleton<GetAllTasks>();







s.AddAuthorization();
s.AddControllers();
s.AddCors();
s.AddAuthentication("SourceJWT").AddScheme<SourceJwtAuthenticationSchemeOptions, SourceJwtAuthenticationHandler>("SourceJWT", options =>
    {
        options.SecretKey = appsettings["jwt_config:Key"].ToString();
        options.ValidIssuer = appsettings["jwt_config:Issuer"].ToString();
        options.ValidAudience = appsettings["jwt_config:Audience"].ToString();
        options.Subject = appsettings["jwt_config:Subject"].ToString();
    });
}).Configure(app=>
{

app.UseAuthentication();
app.UseAuthorization();
 app.UseCors(options =>
         options.WithOrigins("https://localhost:5002", "http://localhost:5001")
         .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseRouting();
app.UseStaticFiles();



app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(e=>
{
           var login=  e.ServiceProvider.GetRequiredService<Login>();

           var register = e.ServiceProvider.GetRequiredService<Registration>();

           var add = e.ServiceProvider.GetRequiredService<AddTask>();

           var knowtasks = e.ServiceProvider.GetRequiredService<GetTasks>();

           var update = e.ServiceProvider.GetRequiredService<TaskUpdate>();

           var delete = e.ServiceProvider.GetRequiredService<deleteTask>();

           var alltask = e.ServiceProvider.GetRequiredService<GetAllTasks>();




 e.MapPost("login",
         [AllowAnonymous] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1001") // update
                         await http.Response.WriteAsJsonAsync(await login.LoginUser(rData));

         });
          e.MapGet("/bing",
                async c => await c.Response.WriteAsJsonAsync("{'Name':'Saurabh','Age':'22','Project':'Food_API'}"));

          e.MapPost("registration",
         [AllowAnonymous] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1002") 
                         await http.Response.WriteAsJsonAsync(await register.registration(rData));
                         
 }); 

  e.MapPost("addtask",
         [Authorize(AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1003") 
                         await http.Response.WriteAsJsonAsync(await add.addTask(rData));
                         
 }); 

  e.MapPost("gettasks",
         [Authorize(AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1004") 
                         await http.Response.WriteAsJsonAsync(await knowtasks.getTasksForUser(rData));
                         
 }); 

   e.MapPost("updatetask",
         [Authorize(AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1005") 
                         await http.Response.WriteAsJsonAsync(await update.updateTask(rData));
                         
 }); 


  e.MapPost("deltask",
         [Authorize(AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1006") 
                         await http.Response.WriteAsJsonAsync(await delete.RemoveTask(rData));
                         
 }); 

 e.MapPost("getalltasks",
         [Authorize(AuthenticationSchemes ="SourceJWT")] async (HttpContext http) =>
         {
             var body = await new StreamReader(http.Request.Body).ReadToEndAsync();
             requestData rData = JsonSerializer.Deserialize<requestData>(body);
              if (rData.eventID == "1007") 
                         await http.Response.WriteAsJsonAsync(await alltask.getTasksForUser(rData));
                         
 }); 


});
}).Build().Run();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Saurabh ka api!");

app.Run();
public record requestData
{
    [Required]
    public string eventID { get; set; }
    [Required]
    public IDictionary<string, object> addInfo { get; set; }
}

public record responseData
{
    public responseData()
    {
        eventID = "";
        rStatus = 0;
        rData = new Dictionary<string, object>();
    }
    [Required]
    public int rStatus { get; set; } = 0;
    public string eventID { get; set; }
    public IDictionary<string, object> addInfo { get; set; }
    public IDictionary<string, object> rData { get; set; }
}
