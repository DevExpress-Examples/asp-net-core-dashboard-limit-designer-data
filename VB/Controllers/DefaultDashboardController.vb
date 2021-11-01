Imports DevExpress.DashboardAspNetCore
Imports DevExpress.DashboardWeb
Imports Microsoft.AspNetCore.DataProtection

Namespace WebDashboardAspNetCore.Controllers

    Public Class DefaultDashboardController
        Inherits DashboardController

        Public Sub New(ByVal configurator As DashboardConfigurator, ByVal Optional dataProtectionProvider As IDataProtectionProvider = Nothing)
            MyBase.New(configurator, dataProtectionProvider)
        End Sub
    End Class
End Namespace
