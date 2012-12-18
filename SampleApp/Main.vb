Imports MiniMVC

Public Class Main
    Inherits HttpHandlerFactory

    Public Overrides Function GetHandler(context As HttpContextBase) As IHttpHandler
        If context.Request.Path = "/" Then
            Return New HttpHandler(Sub(ctx) ctx.Html(<h1>Hello, <%= If(ctx.Request("name"), "world") %></h1>))
        End If
        Return HttpHandler.NotFound
    End Function
End Class
