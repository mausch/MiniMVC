Imports MiniMVC
Imports MiniMVC.TaskAdapter
Imports System.Threading.Tasks
Imports System.Net

Public Class Main
    Inherits HttpHandlerFactory

    Shared Async Function GetGoogle(context As HttpContextBase) As Task
        Dim client = New WebClient
        Dim googleHtml = Await client.DownloadStringTaskAsync("http://www.google.com")
        context.Html(googleHtml)
    End Function

    Public Overrides Function GetHandler(context As HttpContextBase) As IHttpHandler
        If context.Request.Path = "/" Then
            Return New HttpHandler(Sub(ctx) ctx.Html(<h1>Hello, <%= If(ctx.Request("name"), "world") %></h1>))
        ElseIf context.Request.Path = "/google" Then
            Return TaskAdapters.AsAsyncHandler(AddressOf GetGoogle)
        End If
        Return HttpHandler.NotFound
    End Function
End Class
