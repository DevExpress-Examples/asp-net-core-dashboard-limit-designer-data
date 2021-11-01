Imports System.Linq
Imports DevExpress.AspNetCore
Imports DevExpress.DashboardAspNetCore
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWeb
Imports DevExpress.Data.Filtering
Imports DevExpress.DataAccess.Sql
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.FileProviders
Imports Microsoft.Extensions.Hosting
Imports WebDashboardAspNetCore.Models

Namespace WebDashboardAspNetCore

    Public Class Startup

        Public Sub New(ByVal configuration As IConfiguration, ByVal hostingEnvironment As IWebHostEnvironment)
            Me.Configuration = configuration
            FileProvider = hostingEnvironment.ContentRootFileProvider
        End Sub

        Public ReadOnly Property Configuration As IConfiguration

        Public ReadOnly Property FileProvider As IFileProvider

        ' This method gets called by the runtime. Use this method to add services to the container.
        Public Sub ConfigureServices(ByVal services As IServiceCollection)
            services.AddHttpContextAccessor().AddDevExpressControls().AddControllersWithViews()
            services.AddScoped(Function(ByVal serviceProvider)
                Dim configurator As DashboardConfigurator = New DashboardConfigurator()
                configurator.SetDashboardStorage(New DashboardFileStorage(FileProvider.GetFileInfo("Data/Dashboards").PhysicalPath))
                configurator.SetConnectionStringsProvider(New DashboardConnectionStringsProvider(Configuration))
                configurator.AllowExecutingCustomSql = True
                Dim dataSourceStorage As DataSourceInMemoryStorage = New DataSourceInMemoryStorage()
                ' Registers an SQL data source.
                Dim sqlDataSource As DashboardSqlDataSource = New DashboardSqlDataSource("Categories", "NWindConnectionString")
                Dim query As SelectQuery = SelectQueryFluentBuilder.AddTable("Categories").SelectAllColumnsFromTable().Build("Categories")
                sqlDataSource.Queries.Add(query)
                dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml())
                Dim sqlDataSourceCustomSQL As DashboardSqlDataSource = New DashboardSqlDataSource("Products", "NWindConnectionString")
                Dim queryCustomSQL As CustomSqlQuery = New CustomSqlQuery() With {.Name = "queryCustomSQL", .Sql = "SELECT ProductID, ProductName FROM Products WHERE ProductID <= @queryParamTopID"}
                queryCustomSQL.Parameters.Add(New QueryParameter("queryParamTopID", GetType(DevExpress.DataAccess.Expression), New DevExpress.DataAccess.Expression("[Parameters.DashboarParamTopID]")))
                sqlDataSourceCustomSQL.Queries.Add(queryCustomSQL)
                dataSourceStorage.RegisterDataSource("sqlDataSourceCustom", sqlDataSourceCustomSQL.SaveToXml())
                ' Registers an Object data source.
                Dim objDataSource As DashboardObjectDataSource = New DashboardObjectDataSource("Invoices") With {.DataId = "odsInvoices"}
                dataSourceStorage.RegisterDataSource("objDataSource", objDataSource.SaveToXml())
                configurator.SetDataSourceStorage(dataSourceStorage)
                ' Applies dynamic filter in the Designer working mode for the DashboardObjectDataSource.
                AddHandler configurator.DataLoading, Sub(s, e)
                    Dim contextAccessor = serviceProvider.GetService(Of IHttpContextAccessor)()
                    Dim workingMode = contextAccessor.HttpContext.Request.Headers("DashboardWorkingMode")
                    If Equals(e.DashboardId, "Object") AndAlso Equals(e.DataId, "odsInvoices") Then
                        Dim data = Invoices.CreateData()
                        If workingMode = "Designer" Then
                            e.Data = data.Where(Function(item) Equals(item.Country, "USA"))
                        Else
                            e.Data = data
                        End If
                    End If
                End Sub
                ' Applies dynamic filter in the Designer working mode for the DashboardSqlDataSource.
                AddHandler configurator.CustomFilterExpression, Sub(s, e)
                    Dim contextAccessor = serviceProvider.GetService(Of IHttpContextAccessor)()
                    Dim workingMode = contextAccessor.HttpContext.Request.Headers("DashboardWorkingMode")
                    If Equals(e.DashboardId, "SQL") AndAlso Equals(e.QueryName, "Categories") Then
                        If workingMode = "Designer" Then e.FilterExpression = CriteriaOperator.Parse("[Categories.CategoryID] = 1")
                    End If
                End Sub
                ' Applies dynamic filter in the Designer working mode for the DashboardSqlDataSource (Custom SQL Query).
                AddHandler configurator.CustomParameters, Sub(s, e)
                    Dim contextAccessor = serviceProvider.GetService(Of IHttpContextAccessor)()
                    Dim workingMode = contextAccessor.HttpContext.Request.Headers("DashboardWorkingMode")
                    If Equals(e.DashboardId, "SQL (Custom)") Then
                        Dim topID = If(workingMode = "Designer", 3, 1000)
                        e.Parameters.Add(New DevExpress.DataAccess.Parameter("DashboarParamTopID", GetType(Integer), topID))
                    End If
                End Sub
                Return configurator
            End Function)
        End Sub

        ' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IWebHostEnvironment)
            If env.IsDevelopment() Then
                app.UseDeveloperExceptionPage()
            Else
                app.UseExceptionHandler("/Home/Error")
                app.UseHsts()
            End If

            app.UseHttpsRedirection()
            app.UseStaticFiles()
            ' Registers the DevExpress middleware.
            app.UseDevExpressControls()
            app.UseRouting()
            app.UseAuthorization()
            app.UseEndpoints(Sub(endpoints)
                ' Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard", "DefaultDashboard")
                endpoints.MapControllerRoute(name:="default", pattern:="{controller=Home}/{action=Index}/{id?}")
            End Sub)
        End Sub
    End Class
End Namespace
