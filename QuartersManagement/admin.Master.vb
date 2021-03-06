﻿Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Resources

Public Class admin
    Inherits System.Web.UI.MasterPage
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim currUserType As String = ""
    Dim totalNotification As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("user_type") IsNot Nothing Then
                currUserType = Session("user_type")
                load_page()
            Else
                Response.Redirect("default.aspx")
            End If
        End If
    End Sub

    Protected Sub load_page()
        load_notifikasi()
        jumplahNotifikasi.InnerText = totalNotification
    End Sub

    Protected Function getSQL(ByVal jenisNotifikasi As String) As String
        Dim tempSQL As String = "SELECT 
	        A.permohonan_id
	        , A.pengguna_id
	        , A.notifikasi_id
	        , A.notifikasi_untuk
	        , A.notifikasi_tarikh
	        , A.notifikasi_checked
	        , B.config_parameter 'notifkasi_kumpulan'
	        , REPLACE(B.config_value,'{NO_PERMOHONAN}',C.permohonan_no_permohonan) 'notifikasi_log'
        FROM 
	        spk_notifikasi A
	        JOIN general_config B ON B.config_id = A.notifikasi_kumpulan
	        JOIN spk_permohonan C ON C.permohonan_id = A.permohonan_id"
        Dim whereSQL As String = " WHERE 
	        A.notifikasi_untuk = '" & currUserType.ToUpper & "'
            AND A.notifikasi_checked = 0
	        AND B.config_parameter LIKE '%" & jenisNotifikasi & "%'"
        Dim orderBy As String = " ORDER BY notifikasi_tarikh DESC;"
        getSQL = tempSQL & whereSQL & orderBy
        Return getSQL
    End Function

    Protected Function countRows(ByVal dt As DataTable) As Integer
        totalNotification += dt.Rows.Count
        Return dt.Rows.Count
    End Function

    Protected Sub load_notifikasi()
        If notifikasi_baru() Or notifikasi_menunggu() Or notifikasi_ditolak() Or notifikasi_diterima() Then
            adaNotifikasi.Visible = True
            tiadaNotifikasi.Visible = False
        Else
            adaNotifikasi.Visible = False
            tiadaNotifikasi.Visible = True
        End If
    End Sub

    Protected Function notifikasi_baru() As Boolean
        Dim query = getSQL("BARU")
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand(query, conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable
                    sda.Fill(dt)
                    If countRows(dt) > 0 Then
                        rptBaru.DataSource = dt
                        rptBaru.DataBind()
                        Return True
                    Else
                        rptBaru.Visible = False
                        Return False
                    End If
                End Using
            End Using
        End Using
    End Function

    Protected Function notifikasi_menunggu()
        Dim query = getSQL("MENUNGGU")
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand(query, conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable
                    sda.Fill(dt)
                    If countRows(dt) > 0 Then
                        rptMenunggu.DataSource = dt
                        rptMenunggu.DataBind()
                        Return True
                    Else
                        rptMenunggu.Visible = False
                        Return False
                    End If
                End Using
            End Using
        End Using
    End Function

    Protected Function notifikasi_diterima()
        Dim query = getSQL("DITERIMA")
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand(query, conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable
                    sda.Fill(dt)
                    If countRows(dt) > 0 Then
                        rptDiterima.DataSource = dt
                        rptDiterima.DataBind()
                        Return True
                    Else
                        rptDiterima.Visible = False
                        Return False
                    End If
                End Using
            End Using
        End Using
    End Function

    Protected Function notifikasi_ditolak()
        Dim query = getSQL("DITOLAK")
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand(query, conn)
                Using sda As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable
                    sda.Fill(dt)
                    If countRows(dt) > 0 Then
                        rptDitolak.DataSource = dt
                        rptDitolak.DataBind()
                        Return True
                    Else
                        rptDitolak.Visible = False
                        Return False
                    End If
                End Using
            End Using
        End Using
    End Function

    Private Sub logKeluar_ServerClick(sender As Object, e As EventArgs) Handles logKeluar.ServerClick
        Session("user_type") = Nothing
        Session("user_id") = Nothing
        Session("pangkat_id") = Nothing
        Response.Redirect("default.aspx")
    End Sub
End Class