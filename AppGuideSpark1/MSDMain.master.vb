
Partial Class MSDMain
    Inherits System.Web.UI.MasterPage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Response.Cache.SetExpires(DateTime.Now.AddSeconds(60))
        'Response.Cache.SetCacheability(HttpCacheability.Public)
        'Response.Cache.SetValidUntilExpires(True)
        'Response.Cache.VaryByParams("id") = True
        'Response.Cache.SetVaryByCustom("cmsCache")

        'Remove unnecessary Ektron scripts

        'Dim checkLogin As New Ektron.CMS.Controls.Login
        'If checkLogin.IsLoggedIn <> True Then
        '	For Each Control In Page.Header.Controls
        '	    If Control.GetType.Name = "EktronJsControl" Or Control.GetType.Name = "EktronCssControl" Then
        '		'Page.Header.Controls.Remove(Control)
        '	    Else
        '		' Removes the extra bubble inline style stuff that wasn't put in a CSS.
        '		'Dim litControl As LiteralControl 
        '		'If Not (typeof Control is System.Web.UI.HtmlControls.HtmlMeta)
        '		'	litControl = CType(Control, LiteralControl)
        '		'	If litControl.Text is Nothing Then
        '		'	    litControl.Text = ""
        '		'	End If
        '		'End If
        '		
        '		
        '
        '				' Removing blank.css file
        '				'Dim htLink As HtmlLink
        '				'htLink = Ctype(Control, HtmlLink)
        '
        '				'If htLink is Nothing AND htLink .Href = "/css/blank.css" then
        '				'    Page.Header.Controls.Remove(htLink)
        '				'End If
        '			    End If
        '			Next
        '		End If

    End Sub
End Class

