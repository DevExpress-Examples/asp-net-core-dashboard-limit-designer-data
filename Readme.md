<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/423407636/21.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1040827)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Dashboard for ASP.NET Core - How to limit data displayed in the Designer working mode

This example illustrates how to filter data in the [Designer working mode](https://docs.devexpress.com/Dashboard/116301/web-dashboard/aspnet-web-forms-dashboard-control/designer-and-viewer-modes) to improve the performance by displaying only several records from the datasource.

The [DashboardControl.onOptionChanged](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.DashboardControlOptions#js_devexpress_dashboard_dashboardcontroloptions_onoptionchanged) event is handled to catch the moment when the working mode is changed. The current mode is passed to the server through the [AjaxRemoteService.headers](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.AjaxRemoteService#js_devexpress_dashboard_ajaxremoteservice_headers) dictionary. After that, a data reloding callback is nitiated through the [DashboardControl.reloadData](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.DashboardControl?p=netframework#js_devexpress_dashboard_dashboardcontrol_reloaddata) method call (see also: [Manage an In-Memory Data Cache](https://docs.devexpress.com/Dashboard/400983/web-dashboard/dashboard-backend/manage-an-in-memory-data-cache)).

On the server side the [IHttpContextAccessor](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-3.0) with dependency injection is used to access the passed working mode(`HttpContext.Request.Headers["DashboardWorkingMode"]`) in code.

This example illustrates how to filter data in the following data sources:

* [DashboardObjectDataSource](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardObjectDataSource)
The [DashboardConfigurator.DataLoading](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.DataLoading) event is handled for this purpose.

* [DashboardSqlDataSource](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource) with the regular [SelectQuery](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.SelectQuery)
The [DashboardConfigurator.CustomFilterExpression](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.CustomFilterExpression) event is handed for this purpose.

* [DashboardSqlDataSource](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardSqlDataSource) with the custom SQL query - [CustomSqlQuery](https://docs.devexpress.com/CoreLibraries/DevExpress.DataAccess.Sql.CustomSqlQuery)
Custom SQL query contains a parameter (`queryParamTopID`) mapped to the [Dashboard Parameter](https://docs.devexpress.com/Dashboard/117062/web-dashboard/create-dashboards-on-the-web/data-analysis/dashboard-parameters?p=netframework) (see [Pass Parameter Values](https://docs.devexpress.com/Dashboard/117775/web-dashboard/create-dashboards-on-the-web/data-analysis/dashboard-parameters/pass-parameter-values) and [Pass Query Parameters](https://docs.devexpress.com/Dashboard/117192/web-dashboard/create-dashboards-on-the-web/providing-data/working-with-sql-data-sources/pass-query-parameters)). The dashboard parameter and its values are generated dynamically in the [DashboardConfigurator.CustomParameters](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.CustomParameters) event handler.


<!-- default file list -->
*Files to look at*:

* [Startup.cs](./CS/Startup.cs)
* [Index.cshtml](./CS/Views/Home/Index.cshtml)

<!-- default file list end -->

## Documentation

- [Designer and Viewer Modes](https://docs.devexpress.com/Dashboard/116301/web-dashboard/aspnet-web-forms-dashboard-control/designer-and-viewer-modes)
- [Manage an In-Memory Data Cache](https://docs.devexpress.com/Dashboard/400983/web-dashboard/dashboard-backend/manage-an-in-memory-data-cache)
- [Dashboard Parameters](https://docs.devexpress.com/Dashboard/117062/web-dashboard/create-dashboards-on-the-web/data-analysis/dashboard-parameters?p=netframework)
- [Pass Parameter Values](https://docs.devexpress.com/Dashboard/117775/web-dashboard/create-dashboards-on-the-web/data-analysis/dashboard-parameters/pass-parameter-values)
- [Pass Query Parameters](https://docs.devexpress.com/Dashboard/117192/web-dashboard/create-dashboards-on-the-web/providing-data/working-with-sql-data-sources/pass-query-parameters)

## More Examples

- [Dashboard for ASP.NET Core - How to implement authentication](https://github.com/DevExpress-Examples/ASPNET-Core-Dashboard-Authentication)
- [Dashboard for ASP.NET Core - How to implement multi-tenant Dashboard architecture](https://github.com/DevExpress-Examples/DashboardUserBasedAspNetCore)
- [Dashboard for ASP.NET Core - How to load different data based on the current user](https://github.com/DevExpress-Examples/DashboardDifferentUserDataAspNetCore)

## See also
[Web Dashboard - How to research performance issues (a lot of time to display a dashboard or perform a specific action)](https://supportcenter.devexpress.com/ticket/details/t754184)
