﻿Imports System.Data.SqlClient

Public Class konfigurasi_pangkat
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strlblMsgBottom As String = ConfigurationManager.AppSettings("lblMessage_bottom")
    Dim strlblMsgTop As String = ConfigurationManager.AppSettings("lblMessage_top")
    Dim strSucDelAlert As String = ConfigurationManager.AppSettings("deleteSuccessAlert")
    Dim strFailDelAlert As String = ConfigurationManager.AppSettings("deleteFailAlert")
    Dim strSaveSuccessAlert As String = ConfigurationManager.AppSettings("saveSuccessAlert")
    Dim strSaveFailAlert As String = ConfigurationManager.AppSettings("saveFailAlert")
    Dim strDataBindAlert As String = ConfigurationManager.AppSettings("dataBindAlert")
    Dim strRecordBindAlert As String = ConfigurationManager.AppSettings("recordBindAlert")
    Dim strSysErrorAlert As String = ConfigurationManager.AppSettings("systemErrorAlert")
    Dim strDataValAlert As String = ConfigurationManager.AppSettings("dataValidationAlert")

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then

                If strlblMsgBottom = 0 Then
                    strlbl_bottom.Visible = True
                Else
                    strlbl_bottom.Visible = False
                End If
                If strlblMsgTop = 0 Then
                    strlbl_top.Visible = True
                Else
                    strlbl_top.Visible = False
                End If

                requestJenisPangkat()

                If Not Request.QueryString("edit") = "" Then
                    lblConfig.Text = Request.QueryString("p")
                    Load_page()
                Else
                    requestPage()
                    strRet = BindData(datRespondent)
                End If

            End If

        Catch ex As Exception

            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strSysErrorAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message

        Finally

        End Try

    End Sub

    Private Sub requestJenisPangkat()

        ddlJenisPangkat.Items.Insert(0, New ListItem("- PILIH -", String.Empty))
        ddlJenisPangkat.Items.Insert(1, New ListItem("PEGAWAI KANAN", "PEGAWAI KANAN"))
        ddlJenisPangkat.Items.Insert(2, New ListItem("PEGAWAI MUDA", "PEGAWAI MUDA"))
        ddlJenisPangkat.Items.Insert(3, New ListItem("LAIN-LAIN PANGKAT RENDAH", "LAIN-LAIN PANGKAT RENDAH"))

    End Sub

    Private Sub Load_page()

        strSQL = " SELECT * FROM spk_pangkat"
        strSQL += " WHERE pangkat_id = '" & Request.QueryString("edit") & "'"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try

            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nRows As Integer = 0
            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            If MyTable.Rows.Count > 0 Then

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("pangkat_jenis")) Then
                    ddlJenisPangkat.SelectedValue = ds.Tables(0).Rows(0).Item("pangkat_jenis")
                Else
                    ddlJenisPangkat.SelectedValue = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("pangkat_nama")) Then
                    txtNamaPangkat.Text = ds.Tables(0).Rows(0).Item("pangkat_nama")
                Else
                    txtNamaPangkat.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("pangkat_singkatan")) Then
                    txtSingkatan.Text = ds.Tables(0).Rows(0).Item("pangkat_singkatan")
                Else
                    txtSingkatan.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("pangkat_mata")) Then
                    txtMata.Text = ds.Tables(0).Rows(0).Item("pangkat_mata")
                Else
                    txtMata.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("pangkat_idx")) Then
                    txtIdx.Text = ds.Tables(0).Rows(0).Item("pangkat_idx")
                Else
                    txtIdx.Text = ""
                End If

            End If
            strRet = BindData(datRespondent)
        Catch ex As Exception
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strSysErrorAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & ex.Message
        Finally
            objConn.Dispose()
        End Try

    End Sub

    '-- BIND DATA --'
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""

        Dim strOrder As String = " ORDER BY pangkat_idx, pangkat_nama ASC"

        tmpSQL = "SELECT * FROM spk_pangkat"

        strWhere += " WHERE pangkat_id IS NOT NULL"

        If Not ddlJenisPangkat.SelectedValue = "" Then
            strWhere += " AND pangkat_jenis = '" & ddlJenisPangkat.SelectedValue & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder

        Return getSQL

    End Function

    Private Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As New DataTable()
        Dim strConnString As [String] = ConfigurationManager.AppSettings("ConnectionString")
        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter()
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        Try
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            sda.Dispose()
            con.Dispose()
        End Try
    End Function

    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then

                MsgBottom.Attributes("class") = "errorMsg"
                strlbl_bottom.Text = strDataBindAlert
            Else

                MsgBottom.Attributes("class") = "successMsg"
                strlbl_bottom.Text = strRecordBindAlert & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception

            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message
            Return False
        End Try

        Return True

    End Function

    '--SAVE FUNCTION--'
    Private Function Save() As Boolean

        If Not Request.QueryString("edit") = "" Then

            strSQL = "UPDATE spk_pangkat SET "

            strSQL += " pangkat_jenis = UPPER('" & ddlJenisPangkat.SelectedValue & "'),"
            strSQL += " pangkat_nama = UPPER('" & txtNamaPangkat.Text & "'),"
            strSQL += " pangkat_singkatan = UPPER('" & txtSingkatan.Text & "'),"
            strSQL += " pangkat_mata = UPPER('" & txtMata.Text & "'),"
            strSQL += " pangkat_idx = UPPER('" & txtIdx.Text & "')"

            strSQL += " WHERE pangkat_id = '" & Request.QueryString("edit") & "'"

        Else
            strSQL = "INSERT INTO spk_pangkat (pangkat_jenis, pangkat_nama, pangkat_singkatan, pangkat_mata, pangkat_idx)"

            strSQL += " VALUES ("
            strSQL += " UPPER('" & ddlJenisPangkat.SelectedValue & "'),"
            strSQL += " UPPER('" & txtNamaPangkat.Text & "'),"
            strSQL += " UPPER('" & txtSingkatan.Text & "'),"
            strSQL += " UPPER('" & txtMata.Text & "'),"
            strSQL += " UPPER('" & txtIdx.Text & "'))"

        End If

        strRet = oCommon.ExecuteSQL(strSQL)

        If strRet = "0" Then
            Return True
        Else
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strSysErrorAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & strRet
            Return False
        End If

    End Function

    '--DATA VALIDATION--'
    Private Function ValidateData() As Boolean
        'If Not oCommon.isNumeric(txtidx.Text) Then
        '    txtidx.Focus()
        '    Return False

        'End If
        Return True
    End Function

    Private Sub SaveFunction_ServerClick(sender As Object, e As EventArgs) Handles SaveFunction.ServerClick

        strlbl_bottom.Text = ""
        strlbl_top.Text = ""
        '--validate--'
        If ValidateData() = False Then
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strDataValAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strDataValAlert
            Exit Sub
        End If
        Try
            '--execute--'
            If Save() = True Then
                MsgTop.Attributes("class") = "successMsg"
                strlbl_top.Text = strSaveSuccessAlert
                MsgBottom.Attributes("class") = "successMsg"
                strlbl_bottom.Text = strSaveSuccessAlert
            Else
                MsgTop.Attributes("class") = "errorMsg"
                strlbl_top.Text = strSaveFailAlert
                MsgBottom.Attributes("class") = "errorMsg"
                strlbl_bottom.Text = strSaveFailAlert
            End If
        Catch ex As Exception
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strSysErrorAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message
        End Try

        If Not Request.QueryString("edit") = "" Then
            Dim Pagelabel As String = lblConfig.Text & "&q=" & lblQ.Text & "&lblTop=" & strlbl_top.Text & "&lblBottom=" & strlbl_top.Text
            Response.Redirect("Konfigurasi.Pangkat.aspx?p=" & Pagelabel)
        Else
            strRet = BindData(datRespondent)
        End If



    End Sub
    '--REFRESH BUTTON--'
    Private Sub Refresh_ServerClick(sender As Object, e As EventArgs) Handles Refresh.ServerClick
        Dim Pagelabel As String = lblConfig.Text & "&q=" & lblQ.Text
        Response.Redirect("Konfigurasi.Pangkat.aspx?p=" & Pagelabel)
    End Sub

    Private Sub requestPage()
        lblConfig.Text = Request.QueryString("p")
        lblQ.Text = Request.QueryString("q")
        If Not Request.QueryString("lblBottom") = "" Then
            strlbl_top.Text = Request.QueryString("lblTop")
            strlbl_bottom.Text = Request.QueryString("lblBottom")
        End If
    End Sub

    Private Sub clear()
        'ddlNegara.SelectedValue = ""
        ddlJenisPangkat.SelectedValue = ""
        txtNamaPangkat.Text = ""
        txtSingkatan.Text = ""
        txtMata.Text = ""
        txtIdx.Text = ""
    End Sub

    '--DELETE FUNCTION--'
    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting

        Dim strCID = datRespondent.DataKeys(e.RowIndex).Values("pangkat_id").ToString

        'chk session to prevent postback
        If Not strCID = Session("strCID") Then
            strSQL = "DELETE FROM spk_pangkat WHERE pangkat_id = '" & oCommon.FixSingleQuotes(strCID) & "'"
            strRet = oCommon.ExecuteSQL(strSQL)

            Session("strCID") = ""
        End If
        strRet = BindData(datRespondent)

    End Sub

    Private Sub ddlJenisPangkat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlJenisPangkat.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub

    '--INDEX CHANGE--'

End Class