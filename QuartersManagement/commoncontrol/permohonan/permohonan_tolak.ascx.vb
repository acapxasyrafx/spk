﻿Imports System.Data.SqlClient

Public Class permohonan_tolak
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
        If Not IsPostBack Then
            load_page()
        End If
    End Sub

    Protected Sub load_page()
        Try
            loadJenisPangkat()
            loadPangkalan()
            loadPangkat()
            Label2.Text = "SEMUA"
            BindData(datRespondent)
        Catch ex As Exception
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = strSysErrorAlert
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message
        Finally

        End Try
    End Sub

    Private Sub loadPangkalan()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim cmd As New SqlCommand("SELECT * FROM spk_pangkalan;", conn)
            Dim ds As New DataSet
            Try
                conn.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
                ddlfilterPangkalan.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                ddlfilterPangkalan.SelectedIndex = 0
                ddlfilterPangkalan.DataSource = ds
                ddlfilterPangkalan.DataTextField = "pangkalan_nama"
                ddlfilterPangkalan.DataValueField = "pangkalan_id"
                ddlfilterPangkalan.DataBind()
                ddlfilterPangkalan.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                ddlfilterPangkalan.SelectedIndex = 0
            Catch ex As Exception
                Debug.WriteLine("ERROR(loadPangkalan-permohonan_tolak:62): " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub loadKuarters()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim cmd As New SqlCommand("SELECT kuarters_id, kuarters_nama FROM spk_kuarters WHERE pangkat_id = " & ddlfilterPangkalan.SelectedValue & " ORDER BY kuarters_nama ASC; ", conn)
            Dim ds As New DataSet

            Try
                conn.Open()
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
                ddlfilterKuarters.DataSource = ds
                ddlfilterKuarters.DataTextField = "kuarters_nama"
                ddlfilterKuarters.DataValueField = "kuarters_id"
                ddlfilterKuarters.DataBind()
                ddlfilterKuarters.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                ddlfilterKuarters.SelectedIndex = 0
            Catch ex As Exception
                Debug.Write("ERROR(loadKuarters): " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Protected Sub loadPangkat()
        Dim query As String = ""
        Dim tempQuery = "SELECT pangkat_id, pangkat_nama FROM spk_pangkat"
        Dim whereQuery = "  WHERE pangkat_jenis = " & ddlJenisPangkat.SelectedItem.Value & ""
        Dim orderBy = " ORDER BY pangkat_idx ASC;"

        If ddlJenisPangkat.SelectedIndex > 0 Then
            query = tempQuery & whereQuery & orderBy
        Else
            query = tempQuery & orderBy
        End If
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim cmd As New SqlCommand(query, conn)
            Dim ds As New DataSet

            Try
                conn.Open()
                Dim sda As New SqlDataAdapter(cmd)
                sda.Fill(ds)
                ddlfilterPangkat.DataSource = ds
                ddlfilterPangkat.DataTextField = "pangkat_nama"
                ddlfilterPangkat.DataValueField = "pangkat_id"
                ddlfilterPangkat.DataBind()
                ddlfilterPangkat.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                ddlfilterPangkat.SelectedIndex = 0
            Catch ex As Exception
                Debug.Write("ERROR(loadJawatan): " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    '-- BIND DATA --'
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""

        Dim strOrder As String = ""

        tmpSQL = "SELECT 
                    A.pengguna_id as pengguna_id 
                    ,   A.pengguna_no_tentera as no_tentera 
                    ,   A.pengguna_nama as nama 
                    ,   B.permohonan_no_permohonan   
                    ,   C.pangkalan_nama as pangkalan 
                    ,   D.pangkat_singkatan as pangkat 
                    ,   B.pengguna_id as pengguna_idx
                    ,   E.kuarters_nama as unit
                    ,   substring (B.permohonan_tarikh,1,10) as tarikhMohon
                    ,   B.permohonan_status as status
                    ,   B.permohonan_id as permohonan_id 
                    ,   B.permohonan_mata as total_poin
                    ,   B.permohonan_nota as nota
                    ,   substring (B.permohonan_tarikh,1,10) as tarikhMohon
                    ,   substring (G.log_tarikh,1,10) as tarikhUpdate
                FROM spk_permohonan as B
                    LEFT JOIN spk_pengguna A on B.pengguna_id = A.pengguna_id
					LEFT JOIN spk_pangkalan C on A.pangkalan_id = C.pangkalan_id 
					LEFT JOIN spk_pangkat D on A.pangkat_id = D.pangkat_id
                    LEFT JOIN spk_kuarters E on B.kuarters_id = E.kuarters_id
                    LEFT JOIN spk_unit F on B.unit_id = F.unit_id
                    LEFT JOIN (
                        SELECT
                            A.permohonan_id
                            ,	A.log_tarikh 
				        FROM 
                            spk_logPermohonan A 
                        WHERE
                            A.log_status = 'PERMOHONAN DITOLAK'
                    )   G ON G.permohonan_id = B.permohonan_id
					"
        strWhere += " WHERE B.permohonan_status = 'PERMOHONAN ANDA DITOLAK' or B.permohonan_status = 'PERMOHONAN DITOLAK'"

        Try
            If ddlfilterKuarters.SelectedIndex > 0 Then
                strWhere += " AND B.kuarters_id = '" & ddlfilterKuarters.SelectedValue & "'"
            End If

            If ddlJenisPangkat.SelectedIndex > 0 Then
                strWhere += " AND D.pangkat_jenis = " & ddlJenisPangkat.SelectedValue & ""
            End If

            If ddlfilterPangkalan.SelectedIndex > 0 Then
                strWhere += " AND A.pangkalan_id = '" & ddlfilterPangkalan.SelectedValue & "'"
            End If

            If ddlfilterPangkat.SelectedIndex > 0 Then
                strWhere += " AND A.pangkat_id = '" & ddlfilterPangkat.SelectedValue & "'"
            End If

        Catch ex As Exception
            MsgBottom.InnerText = ex.ToString
        End Try

        If Not txt_nama.Text = "" Then
            strWhere += " AND (A.pengguna_nama LIKE '%" & txt_nama.Text & "%' OR A.pengguna_no_tentera LIKE '%" & txt_nama.Text & "%')"
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

    '--DELETE FUNCTION--'
    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting

        Dim strCID = datRespondent.DataKeys(e.RowIndex).Values("pangkalan_id").ToString

        'chk session to prevent postback
        If Not strCID = Session("strCID") Then
            ''strSQL = "DELETE FROM spk_pangkalan WHERE pangkalan_id = '" & oCommon.FixSingleQuotes(strCID) & "'"
            strRet = oCommon.ExecuteSQL(strSQL)

            Session("strCID") = ""
        End If
        ''strRet = BindData(datRespondent)

    End Sub

    Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            If (e.CommandName = "ViewApllicant") Then
                Dim strCID = e.CommandArgument.ToString

                Response.Redirect("Senarai.Permohonan.Ditolak.aspx?uid=" + strCID)
            ElseIf (e.CommandName = "Process") Then
                Dim strCID = e.CommandArgument.ToString

                'chk session to prevent postback
                strSQL = "UPDATE spk_permohonan SET permohonan_status = 'PERMOHONAN MENUNGGU' WHERE permohonan_id = '" & oCommon.FixSingleQuotes(strCID) & "'"

                strRet = oCommon.ExecuteSQL(strSQL)
                If (strRet = 0) Then
                    strlbl_bottom.Text = "Data Diproses"

                    BindData(datRespondent)
                Else
                    strlbl_bottom.Text = "Pemprosesan Gagal"

                    BindData(datRespondent)
                End If

                BindData(datRespondent)
            ElseIf (e.CommandName = "Batal") Then
                Dim strCID = e.CommandArgument.ToString

                'chk session to prevent postback
                strSQL = "UPDATE spk_permohonan SET permohonan_status = 'PERMOHONAN ANDA DITOLAK' WHERE permohonan_id = '" & oCommon.FixSingleQuotes(strCID) & "'"
                oCommon.ExecuteSQL(strSQL)

                BindData(datRespondent)
            End If
            BindData(datRespondent)

        Catch ex As Exception
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message
        End Try
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        strRet = BindData(datRespondent)
    End Sub

    Private Sub ddlfilterPangkat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlfilterPangkat.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub

    Private Sub ddlfilterPangkalan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlfilterPangkalan.SelectedIndexChanged
        If ddlfilterPangkalan.SelectedIndex > 0 Then
            loadKuarters()
            ddlfilterKuarters.Enabled = True
            strRet = BindData(datRespondent)
        Else
            ddlfilterKuarters.Enabled = False
        End If
    End Sub

    Private Sub ddlfilterKuarters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlfilterKuarters.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub

    Protected Sub loadJenisPangkat()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand("select config_id,config_parameter from general_config WHERE config_type = 'KUMPULAN PEGAWAI' ORDER BY config_idx DESC;", conn)
                cmd.Connection = conn
                Dim sda As New SqlDataAdapter(cmd)
                Try
                    Dim ds As New DataSet
                    conn.Open()
                    sda.Fill(ds)
                    ddlJenisPangkat.DataSource = ds
                    ddlJenisPangkat.DataTextField = "config_parameter"
                    ddlJenisPangkat.DataValueField = "config_id"
                    ddlJenisPangkat.DataBind()
                    ddlJenisPangkat.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                Catch ex As Exception
                    Debug.Write("ERROR(loadJenisPangkat): " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub

    Private Sub ddlJenisPangkat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlJenisPangkat.SelectedIndexChanged
        loadPangkat()
        BindData(datRespondent)
        If ddlJenisPangkat.SelectedIndex > 0 Then
            Label2.Text = ddlJenisPangkat.SelectedItem.Text
        Else
            Label2.Text = "SEMUA"
        End If
    End Sub
End Class