Imports MiniMVC
Imports MiniMVC.TaskAdapter
Imports System.Threading.Tasks
Imports System.Net

Public Class Main
    Inherits HttpHandlerFactory

    Shared Async Function GetGoogleAsync(context As HttpContextBase) As Task
        Dim client = New WebClient
        Dim googleHtml = Await client.DownloadStringTaskAsync("http://www.google.com")
        context.Response.Html(googleHtml)
    End Function

    Shared Sub GetGoogle(context As HttpContextBase)
        Dim client = New WebClient
        Dim googleHtml = client.DownloadString("http://www.google.com")
        context.Response.Html(googleHtml)
    End Sub

    Public Overrides Function GetHandler(context As HttpContextBase) As IHttpHandler
        If context.Request.Path = "/" Then
            Return New HttpHandler(Sub(ctx) ctx.Response.Html(<h1>Hello, <%= If(ctx.Request("name"), "world") %></h1>))
        ElseIf context.Request.Path = "/google" Then
            Return New HttpHandler(AddressOf GetGoogle)
        ElseIf context.Request.Path = "/googleasync" Then
            Return TaskAdapters.AsAsyncHandler(AddressOf GetGoogleAsync)
        End If
        Return HttpHandler.NotFound
    End Function
End Class
