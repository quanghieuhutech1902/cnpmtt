using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CongNghePhanMemTienTien.Startup))]
namespace CongNghePhanMemTienTien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
