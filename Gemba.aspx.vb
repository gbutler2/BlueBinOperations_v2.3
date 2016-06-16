﻿Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Configuration

Partial Class Gemba
    Inherits Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GembaAuditNodeGridView.DataBind()

        If Me.Page.User.Identity.IsAuthenticated Then
            Dim UserLogin As String = Page.User.Identity.Name.ToString().ToLower()
            Dim OPGembaDashboardB As String

            Dim constr As String = ConfigurationManager.ConnectionStrings("Site_ConnectionString").ConnectionString

            Using conmenu As New SqlConnection(constr)
                Using cmdadmin As New SqlCommand("sp_ValidateMenus")

                    cmdadmin.CommandType = CommandType.StoredProcedure
                    cmdadmin.Connection = conmenu
                    conmenu.Open()

                    'QCN Dashboard Button
                    cmdadmin.Parameters.AddWithValue("@ConfigName", "OP-Gemba Dashboard")
                    OPGembaDashboardB = Convert.ToString(cmdadmin.ExecuteScalar())

                    conmenu.Close()
                End Using
            End Using



            If OPGembaDashboardB = "Yes" Then
                GembaDashboardB.Visible = True
            Else
                GembaDashboardB.Visible = False
            End If

        End If

    End Sub



    Protected Sub GembaAuditNodeFormButton_Click(sender As Object, e As EventArgs) Handles GembaAuditNodeFormButton.Click
        Response.Redirect("~/GembaAuditNodeForm")
    End Sub

    Protected Sub GembaDashboardB_Click(sender As Object, e As EventArgs) Handles GembaDashboardB.Click
        'Response.Redirect("~/QCNReportViewer")
        Dim url As String = "GembaReportViewer.aspx"
        Response.Write("<script type='text/javascript'>window.open('" + url + "');</script>")
    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GembaAuditNodeGridView.PageIndex = e.NewPageIndex
        GembaAuditNodeGridView.DataBind()

    End Sub
    Protected Sub OnRowDataBoundNode(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim cellAdditionalCommentsText As TableCell = e.Row.Cells(13)
            cellAdditionalCommentsText.ToolTip = TryCast(e.Row.DataItem, DataRowView)("AdditionalCommentsText").ToString()

        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim stagecell As TableCell = e.Row.Cells(9)
            Dim stagescore As Integer = Integer.Parse(stagecell.Text)
            If stagescore = 0 Then
                stagecell.BackColor = Color.Red
            End If
        End If

    End Sub



    Protected Sub GembaAuditNodeExportToExcel(sender As Object, e As EventArgs)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=GembaAuditNodeExport.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            'To Export all pages
            GembaAuditNodeGridView.AllowPaging = False
            GembaAuditNodeGridView.DataBind()
            GembaAuditNodeGridView.HeaderRow.BackColor = Color.White
            GembaAuditNodeGridView.HeaderRow.Cells(0).Visible = False
            GembaAuditNodeGridView.HeaderRow.Cells(1).Visible = False
            For Each cell As TableCell In GembaAuditNodeGridView.HeaderRow.Cells
                cell.BackColor = GembaAuditNodeGridView.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In GembaAuditNodeGridView.Rows
                row.BackColor = Color.White
                row.Cells(0).Visible = False
                row.Cells(1).Visible = False
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = GembaAuditNodeGridView.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = GembaAuditNodeGridView.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"


                Next
            Next

            GembaAuditNodeGridView.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(Regex.Replace(sw.ToString(), "(<a[^>]*>)|(</a>)", " ", RegexOptions.IgnoreCase))

            Response.[End]()
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class