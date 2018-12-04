
using CleverBartender_v2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CleverBartender_v2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IngredientContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DrinkContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<RecipeContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }    

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Drinks}/{action=Index}/{id?}");
            });

            GlobalVariables.MyGlobalString = "test123";
            SetupSocket();

        }

        private void SetupSocket()
        {
            GlobalVariables.server = new TcpListener(IPAddress.Parse("192.168.137.1"), 8000);

            GlobalVariables.server.Start();
            //Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);

            GlobalVariables.client  = GlobalVariables.server.AcceptTcpClient();

            //Console.WriteLine("A client connected.");

            GlobalVariables.stream  = GlobalVariables.client.GetStream();

            while (GlobalVariables.client.Available < 3)
            {
                // wait for enough bytes to be available
            }

            Byte[] bytes = new Byte[GlobalVariables.client.Available];

            GlobalVariables.stream.Read(bytes, 0, bytes.Length);

            //translate bytes of request to string
            String data = Encoding.UTF8.GetString(bytes);

            if (new System.Text.RegularExpressions.Regex("^GET").IsMatch(data))
            {
                const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

                Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                    + "Connection: Upgrade" + eol
                    + "Upgrade: websocket" + eol
                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                        System.Security.Cryptography.SHA1.Create().ComputeHash(
                            Encoding.UTF8.GetBytes(
                                new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                            )
                        )
                    ) + eol
                    + eol);

                GlobalVariables.stream.Write(response, 0, response.Length);
            }
            else
            {
                //Console.WriteLine("test no regex error");
            }

            //Console.WriteLine("Starting socket ...");
            bool messageStarted = false;
            while (!messageStarted)
            {
                while (!GlobalVariables.stream.DataAvailable) ;

                Byte[] bytesM = new Byte[GlobalVariables.client.Available];

                GlobalVariables.stream.Read(bytesM, 0, bytesM.Length);
                String dataM = Encoding.UTF8.GetString(bytesM);

                if (dataM.Remove(0, 6) == "start123")
                    messageStarted = true;
            }

            GlobalVariables.socketStarted = true;

        }
}
}
