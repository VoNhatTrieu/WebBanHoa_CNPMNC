using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(giadinhthoxinh.Startup))]
namespace giadinhthoxinh
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cấu hình SignalR với debug mode
            var hubConfiguration = new HubConfiguration()
            {
                EnableDetailedErrors = true, // Hiển thị lỗi chi tiết
                EnableJavaScriptProxies = true // Tạo JavaScript proxy
            };

            app.MapSignalR(hubConfiguration);
        }
    }
}