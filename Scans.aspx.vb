﻿Imports System.Data
Imports System.Data.SqlClient

Partial Class Scans
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack() Then
        'Comment

        'Dim SqlDataSourceScan As New SqlDataSource()
        'SqlDataSourceScan.ID = "Site_ConnectionString"
        'Me.Page.Controls.Add(SqlDataSourceScan)
        'SqlDataSourceScan.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("Site_ConnectionString").ConnectionString
        'SqlDataSourceScan.SelectCommand =
        '"exec sp_SelectScanBatch"
        'GridViewScan.DataSource = SqlDataSourceScan

        GridViewScan.DataBind()

        'End If
    End Sub

    Protected Sub NewScanBatch_Click(sender As Object, e As EventArgs) Handles NewScanBatch.Click
        Response.Redirect("~/ScanOrder")
        GridViewScan.DataBind()
    End Sub

    Protected Sub NewScanBatchReceive_Click(sender As Object, e As EventArgs) Handles NewScanBatchReceive.Click
        Response.Redirect("~/ScanReceive")
        GridViewScan.DataBind()
    End Sub

    Protected Sub OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewScan.PageIndex = e.NewPageIndex
        GridViewScan.DataBind()
    End Sub



End Class