Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Collections.Generic


Partial Class AppGuideDomestic
    Inherits System.Web.UI.Page

    Dim legendItems As New Dictionary(Of String, String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim googleScript As String = "<script type=""text/javascript"">" & _
                                        "var gaJsHost = ((""https:"" == document.location.protocol) ? ""https://ssl."" : ""http://www."");" & _
                                        "document.write(unescape(""%3Cscript src='"" + gaJsHost + ""google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E""));" & _
                                        "</script>" & _
                                        "<script type=""text/javascript"">" & _
                                        "try {" & _
                                        "var pageTracker = _gat._getTracker(""UA-272471-12"");" & _
                                        "pageTracker._trackPageview();" & _
                                        "} catch(err) {}</script>"

        Page.RegisterStartupScript("PopupScript", googleScript)


    End Sub

    'Functions to get spark plug images
    Protected Function getSparkImage(ByVal SparkItem As String) As String
        Dim imageSrc = "<img class='sparkimage' src='/images/{0}' />"
        Dim sparkDigits As String

        sparkDigits = SparkItem.Substring(0, 2)

        Select Case sparkDigits
            Case "1I"
                Return (String.Format(imageSrc, "MSD1_plug.jpg"))
            Case "2I"
                Return (String.Format(imageSrc, "MSD2_plug.jpg"))
        End Select
    End Function




    Protected Function getURL(ByVal partName As String) As String
        If partName.Length = 0 Then
            Return String.Empty
        Else
            Dim sql As String = "select contentid from content_lookup where partname = @partname"
            'Dim conn As OleDbConnection = New OleDbConnection(ConfigurationManager.ConnectionStrings("ACC_ApplicationGuideConnectionString").ToString())
            'Dim cmd As New OleDbCommand(sql, conn)
            Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ACC_ApplicationGuideConnectionString").ToString())
            Dim cmd As New SqlCommand(sql, conn)
            Dim contentid As Integer = 0

            cmd.Parameters.Add("@partname", Data.SqlDbType.VarChar)
            cmd.Parameters("@partname").Value = partName
            conn.Open()
            Int32.TryParse(cmd.ExecuteScalar(), contentid)
            conn.Close()

            If contentid = 0 Then
                Return ("")
            Else
                Return "http://www.msdperformance.com/product.aspx?id=" & contentid.ToString()
            End If

        End If

    End Function
    Protected Function getURLLink(ByVal partName As Object) As String
        Dim productURL As String

        If partName.ToString.Length = 0 Then
            Return "(None Available)"
        Else
            productURL = getURL(partName.ToString())
            Return ("<a href='" & productURL & "'>" & partName.ToString() & "</a>")
        End If

    End Function
    Protected Function getURLCollection(ByVal PartType As String, ByVal MakeID As String, ByVal Model As String, ByVal Year As String, ByVal Engine As String) As String

        Dim sql As String = "SELECT ignition, coil, distributor, wireset, harness, adapter, isusercontributed " & _
                            "FROM Data WHERE (MakeID = @makeid) AND (year = @year) AND (model = @model) AND (engine = engine) order by isusercontributed"

        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ACC_ApplicationGuideConnectionString").ToString())
        Dim cmd As New SqlCommand(sql, conn)
        Dim sqlReader As SqlDataReader
        Dim sb As New StringBuilder

        cmd.Parameters.Add("@makeid", Data.SqlDbType.VarChar)
        cmd.Parameters("@makeid").Value = MakeID
        cmd.Parameters.Add("@model", Data.SqlDbType.VarChar)
        cmd.Parameters("@model").Value = Model
        cmd.Parameters.Add("@year", Data.SqlDbType.VarChar)
        cmd.Parameters("@year").Value = Year
        cmd.Parameters.Add("@engine", Data.SqlDbType.VarChar)
        cmd.Parameters("@engine").Value = Engine

        conn.Open()
        sqlReader = cmd.ExecuteReader()
        While sqlReader.Read
            Select Case PartType
                Case "ignition"
                    If (sqlReader("ignition").ToString.Length) > 0 Then
                        sb.Append("<a href='" & getURL(sqlReader("ignition")) & "'>" & sqlReader("ignition") & "</a>")
                        If (sqlReader("isusercontributed")) Then
                            sb.Append("*")
                        End If
                        sb.Append("<br/>")
                    Else
                        sb.Append("None available")
                    End If
                Case "coil"
                    sb.Append("<a href='" & getURL(sqlReader("coil")) & "'>" & sqlReader("coil") & "</a>")
                    If (sqlReader("isusercontributed") And sqlReader("coil").ToString.Length > 0) Then
                        sb.Append("*")
                    End If
                    sb.Append("<br/>")
                Case "distributor"
                    sb.Append("<a href='" & getURL(sqlReader("distributor")) & "'>" & sqlReader("distributor") & "</a>")
                    If (sqlReader("isusercontributed") And sqlReader("distributor").ToString.Length > 0) Then
                        sb.Append("*")
                    End If
                    sb.Append("<br/>")
                Case "wireset"
                    sb.Append("<a href='" & getURL(sqlReader("wireset")) & "'>" & sqlReader("wireset") & "</a>")
                    If (sqlReader("isusercontributed") And sqlReader("wireset").ToString.Length > 0) Then
                        sb.Append("*")
                    End If
                    sb.Append("<br/>")
                Case "harness"
                    sb.Append("<a href='" & getURL(sqlReader("harness")) & "'>" & sqlReader("harness") & "</a>")
                    If (sqlReader("isusercontributed") And sqlReader("harness").ToString.Length > 0) Then
                        sb.Append("*")
                    End If
                    sb.Append("<br/>")
                Case "adapter"
                    sb.Append("<a href='" & getURL(sqlReader("adapter")) & "'>" & sqlReader("adapter") & "</a>")
                    If (sqlReader("isusercontributed") And sqlReader("adapter").ToString.Length > 0) Then
                        sb.Append("*")
                    End If
                    sb.Append("<br/>")
            End Select


        End While

        conn.Close()

        Return sb.ToString()


    End Function

    Private Function parseLegend(ByVal legendChar As String) As String
        Dim imageSrc = "<img class='icons' src='/icons/{0}' />"

        If Not legendItems.ContainsKey(legendChar) Then
            legendItems.Add(legendChar, legendChar)
        End If

        Select Case legendChar
            Case "w"
                Return (String.Format(imageSrc, "a_icon.gif"))
            Case "e"
                Return (String.Format(imageSrc, "b_icon.gif"))
            Case "y"
                Return (String.Format(imageSrc, "c_icon.gif"))
            Case "µ"
                Return (String.Format(imageSrc, "d_icon.gif"))
            Case "r"
                Return (String.Format(imageSrc, "e_icon.gif"))
            Case "d"
                Return (String.Format(imageSrc, "f_icon.gif"))
            Case "l"
                Return (String.Format(imageSrc, "g_icon.gif"))
            Case "h"
                Return (String.Format(imageSrc, "h_icon.gif"))
            Case "i"
                Return (String.Format(imageSrc, "h_icon.gif"))
            Case "þ"
                Return (String.Format(imageSrc, "j_icon.gif"))
            Case "z"
                Return (String.Format(imageSrc, "k_icon.gif"))
            Case "b"
                Return (String.Format(imageSrc, "l_icon.gif"))
            Case "ò"
                Return (String.Format(imageSrc, "m_icon.gif"))
            Case "a"
                Return (String.Format(imageSrc, "n_icon.gif"))
            Case "x"
                Return (String.Format(imageSrc, "o_icon.gif"))
            Case "ï"
                Return (String.Format(imageSrc, "p_icon.gif"))
            Case "n"
                Return (String.Format(imageSrc, "q_icon.gif"))
            Case "m"
                Return (String.Format(imageSrc, "r_icon.gif"))
            Case "v"
                Return (String.Format(imageSrc, "s_icon.gif"))
            Case ""
                Return (String.Format(imageSrc, "t_icon.gif"))
            Case "ü"
                Return (String.Format(imageSrc, "u_icon.gif"))
            Case "`"
                Return (String.Format(imageSrc, "v_icon.gif"))
            Case "â"
                Return (String.Format(imageSrc, "w_icon.gif"))
            Case "["
                Return (String.Format(imageSrc, "x_icon.gif"))
            Case "ð"
                Return (String.Format(imageSrc, "y_icon.gif"))
            Case "ñ"
                Return (String.Format(imageSrc, "z_icon.gif"))
            Case Else
                Return (String.Empty)
        End Select
    End Function
    Private Function parseLegendDescription(ByVal legendChar As String) As String
        Dim imageSrc As String = "* {0}<BR />"

        Select Case legendChar
            Case "w"
                Return (String.Format(imageSrc, "Two Required"))
            Case "e"
                Return (String.Format(imageSrc, "Three Required"))
            Case "y"
                Return (String.Format(imageSrc, "Four Required"))
            Case "µ"
                Return (String.Format(imageSrc, "Five Required"))
            Case "r"
                Return (String.Format(imageSrc, "<a href='http://www.msdperformance.com/page.aspx?id=15358'>Visual Inspection Required</a>"))
            Case "d"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=5913'>31199</a>"))
            Case "l"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=5951'>31309</a>"))
            Case "h"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=5957'>31319</a>"))
            Case "i"
                Return (String.Format(imageSrc, "Wires Under Manf use <a href='http://www.msdperformance.com/product.aspx?id=6031'>31599</a>"))
            Case "þ"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=6083'>31859</a>"))
            Case "z"
                Return (String.Format(imageSrc, "If DIS use 62112"))
            Case "b"
                Return (String.Format(imageSrc, "Must use <a href='http://www.msdperformance.com/product.aspx?id=6329'>32939</a> Wire Set w/Coil"))
            Case "ò"
                Return (String.Format(imageSrc, "Supercharged only"))
            Case "a"
                Return (String.Format(imageSrc, "Non Computer Controlled only"))
            Case "x"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=5997'>31389</a>"))
            Case "ï"
                Return (String.Format(imageSrc, "Stock Dist use <a href='http://www.msdperformance.com/product.aspx?id=6003'>31399</a>"))
            Case "n"
                Return (String.Format(imageSrc, "If using MSD Dist use <a href='http://www.msdperformance.com/product.aspx?id=6465'>35379</a>"))
            Case "m"
                Return (String.Format(imageSrc, "If using MSD Dist use <a href='http://www.msdperformance.com/product.aspx?id=6497'>35659</a>"))
            Case "v"
                Return (String.Format(imageSrc, "Supplied w/Steel Gear for Hyd Roller Cam"))
            Case ""
                Return (String.Format(imageSrc, "If Equip w/Internal Coil use <a href='http://www.msdperformance.com/product.aspx?id=5061'>8225</a>"))
            Case "ü"
                Return (String.Format(imageSrc, "If Equip w/Ext Coil only"))
            Case "`"
                Return (String.Format(imageSrc, "Requires 62112"))
            Case "â"
                Return (String.Format(imageSrc, "Terminal Insp Req, Rd use <a href='http://www.msdperformance.com/product.aspx?id=5069'>8229</a>, Flat use <a href='http://www.msdperformance.com/product.aspx?id=5079'>8239</a>"))
            Case "["
                Return (String.Format(imageSrc, "Will work with <a href='http://www.msdperformance.com/product.aspx?id=5079'>8239</a> only"))
            Case "ð"
                Return (String.Format(imageSrc, "Distributorless Apps use <a href='http://www.msdperformance.com/product.aspx?id=5803'>62152</a> & Two <a href='http://www.msdperformance.com/product.aspx?id=4713'>8912</a> Adapters"))
            Case "ñ"
                Return (String.Format(imageSrc, "SOHC only"))
            Case Else
                Return (String.Empty)
        End Select
    End Function

    Protected Function getLegend(ByVal legendString As Object) As String
        Dim imgTags As String = String.Empty
        Dim i As Integer
        Dim c As String

        If legendString Is DBNull.Value Then
            Return (String.Empty)
        End If
        For i = 0 To legendString.Length - 1
            c = legendString.Substring(i, 1)
            imgTags += parseLegend(c)
        Next

        Return (imgTags)
    End Function

    Protected Function getLegendDescription(ByVal legendString As Object) As String
        Dim imgTags As String = String.Empty
        Dim i As Integer
        Dim c As String

        If legendString Is DBNull.Value Then
            Return (String.Empty)
        End If
        For i = 0 To legendString.Length - 1
            c = legendString.Substring(i, 1)
            imgTags += parseLegendDescription(c)
        Next

        Return (imgTags)
    End Function

    Protected Function getLegendGuide() As String
        Dim token As KeyValuePair(Of String, String)
        Dim sb As New StringBuilder

        If legendItems.Count <= 0 Then
            Return (String.Empty)
        End If

        sb.Append("<div class='appGuideLegend'>Legend:</div><BR />")
        For Each token In legendItems
            sb.Append(parseLegend(token.Key))
            sb.Append(parseLegendDescription(token.Key))
            sb.Append("<BR />")
        Next

        Return (sb.ToString())
    End Function

    Protected Function getBuyNowLink(ByVal partName As Object, ByVal linkedImage As String) As String
        If partName Is DBNull.Value Then
            Return String.Empty
        Else
            Dim sql As String = "SELECT cmt.meta_value FROM content_meta_tbl AS cmt INNER JOIN " & _
                                "msd_content_lookup AS mcl ON cmt.content_id = mcl.ContentID " & _
                                "WHERE (mcl.PartNumber = @partname) AND (cmt.meta_type_id = 130)"
            Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("Ektron.DbConnection").ToString())
            Dim cmd As New SqlCommand(sql, conn)
            Dim BuyLink As String = String.Empty
            Dim URLSource As String = String.Empty

            cmd.Parameters.Add("@partname", Data.SqlDbType.VarChar)
            cmd.Parameters("@partname").Value = partName

            Try
                conn.Open()
                URLSource = cmd.ExecuteScalar()

                If URLSource = String.Empty Then
                    conn.Close()
                Else

                    BuyLink = "<a alt='" + partName + "' href='" + URLSource + "'>" + linkedImage + "</a>"

                End If
            Catch ex As Exception

            Finally
                conn.Close()
            End Try

            Return (BuyLink)


        End If
    End Function
    Protected Function getThumbNail(ByVal partName As Object) As String
        If partName Is DBNull.Value Then
            Return String.Empty
        Else
            Dim sql As String = "select part_typeid from parts where name = @partname"
            Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("ACC_ApplicationGuideConnectionString").ToString())
            Dim cmd As New SqlCommand(sql, conn)
            'Dim conn As OleDbConnection = New OleDbConnection(ConfigurationManager.ConnectionStrings("ACC_ApplicationGuideConnectionString").ToString())
            'Dim cmd As New OleDbCommand(sql, conn)

            Dim partTypeID As Integer
            Dim imageSrc As String = String.Empty

            cmd.Parameters.Add("@partname", Data.SqlDbType.VarChar)
            cmd.Parameters("@partname").Value = partName
            Try
                conn.Open()
                If Not Int32.TryParse(cmd.ExecuteScalar(), partTypeID) Then
                    conn.Close()
                Else

                    Select Case partTypeID
                        Case 1
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Ignitions/thumb_" + partName + "_full.jpg' /></a>"
                        Case 2
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Coils/thumb_" + partName + "_full.jpg' /></a>"
                        Case 3
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Distributors/thumb_" + partName + "_full.jpg' /></a>"
                        Case 4
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Spark_Plug_Wires/thumb_" + partName + "_full.jpg' /></a>"
                        Case 5
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Accessories/thumb_" + partName + "_full.jpg' /></a>"
                        Case 6
                            imageSrc = "<a href='" + getURL(partName) + "'>" + "<img border='0' src='/uploadedimages/MSDIgnitioncom/Products/Accessories/thumb_" + partName + "_full.jpg' /></a>"
                    End Select

                End If
            Catch ex As Exception

            Finally
                conn.Close()
            End Try

            Return (imageSrc)
        End If

    End Function
    'Protected Function returnLegend() As String
    '    'Dim token As KeyValuePair(Of String, String)
    '    'Dim sb As New StringBuilder
    '    'sb.Append("count" + legendItems.Count.ToString)
    '    'For Each token In legendItems
    '    '    sb.Append(token.Key)
    '    '    sb.Append("<BR>")
    '    'Next

    '    'Return sb.ToString()
    '    Return ("Count" + legendItems.Count.ToString())
    'End Function

    Protected Sub ddMake_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMake.SelectedIndexChanged
        If ddMake.SelectedIndex = 0 Then
            ddModel.Visible = False
            ddEngine.Visible = False

            lblModel.Visible = False
            lblEngine.Visible = False
        Else
            ddModel.Visible = True
            ddEngine.Visible = False

            lblModel.Visible = True
            lblEngine.Visible = False
        End If
    End Sub

    Protected Sub ddModel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddModel.SelectedIndexChanged
        If ddModel.SelectedIndex = 0 Then
            ddEngine.Visible = False

            lblEngine.Visible = False
        Else
            ddEngine.Visible = True

            lblEngine.Visible = True
        End If
    End Sub

    Protected Sub ddYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddYear.SelectedIndexChanged
        If ddYear.SelectedIndex = 0 Then
            ddMake.Visible = False
            ddModel.Visible = False
            ddEngine.Visible = False

            lblMake.Visible = False
            lblModel.Visible = False
            lblEngine.Visible = False
        Else

            ddMake.Visible = True
            ddModel.Visible = False
            ddEngine.Visible = False

            lblMake.Visible = True
            lblModel.Visible = False
            lblEngine.Visible = False

        End If
    End Sub
End Class
