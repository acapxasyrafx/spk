﻿Imports System.Data.SqlClient

Public Class maklumat_pemohon_menunggu
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


    Dim dataAnak As New DataSet
    Dim countAnak As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Try
            loadUser()
            ddlunit_load()
            readMaklumatAnak()

        Catch ex As Exception

        End Try
    End Sub
    Private Function readMaklumatAnak() As Boolean
        Dim penggunaID = pengguna_id.Value
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim table As DataTable = New DataTable
            Dim da As New SqlDataAdapter(
                    "SELECT 
                        anak_id,
                        pengguna_id,
                        anak_nama,
                        anak_ic,
                        anak_umur
                        FROM spk_anak
                        WHERE pengguna_id = " & penggunaID & ";",
                    conn)
            Try
                conn.Open()
                da.Fill(dataAnak, "AnyTable")
                Dim nRows As Integer = 0
                Dim nCount As Integer = 1
                countAnak = dataAnak.Tables(0).Rows.Count
                If dataAnak.Tables(0).Rows.Count > 0 Then
                    datRespondent.DataSource = dataAnak
                    datRespondent.DataBind()
                End If
                Return True
            Catch ex As Exception
                Debug.WriteLine("ERROR(readMaklumatAnak): " & ex.Message)
                Return False
            Finally
                conn.Close()
            End Try
        End Using
    End Function

    Private Sub loadUser()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim cmd As New SqlCommand("SELECT TOP 1
	            A.pengguna_id,
	            A.pengguna_nama,
	            A.pengguna_mykad,
	            A.pengguna_jantina,
	            A.pengguna_tarikh_lahir,
                A.pengguna_mula_perkhidmatan,
                A.pengguna_tamat_perkhidmatan,
                A.pengguna_no_tentera,
	            B.pangkat_id,
	            B.pangkat_nama,
                C.pangkalan_nama,
                E.kuarters_nama,
				G.keluarga_tempat_tinggal ,
				G.keluarga_tarikh_mula,
				G.keluarga_anak,
				D.permohonan_mata
            FROM 
	            admin.spk_pengguna A
	            JOIN admin.spk_pangkat B ON A.pangkat_id = B.pangkat_id
	            JOIN dbo.spk_pangkalan C ON A.pangkalan_id = C.pangkalan_id
				JOIN spk_permohonan D on A.pengguna_id = D.pengguna_id 
				JOIN spk_kuarters E on D.kuarters_id = E.kuarters_id
				JOIN spk_keluarga G on A.pengguna_id = G.pengguna_id
				JOIN spk_anak F on A.pengguna_id = F.pengguna_id	
            WHERE D.permohonan_id = '" & Request.QueryString("uid") & "' ",
            conn)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    If reader.Read() Then
                        pengguna_id.Value = reader("pengguna_id")
                        lblNama.InnerText = reader("pengguna_nama")
                        lblTarikhLahir.InnerText = reader("pengguna_tarikh_lahir")
                        lblJantina.InnerText = reader("pengguna_jantina")
                        lblJawatan.InnerText = reader("pangkat_nama")
                        lblNoTentera.InnerText = reader("pengguna_no_tentera")
                        lblTarikhMulaBerkhidmat.InnerText = reader("pengguna_mula_perkhidmatan")
                        lbl_senaraiPangkalan.InnerText = reader("pangkalan_nama")
                        lbl_senaraiKuarters.InnerText = reader("kuarters_nama")

                        '-------------------
                        'If reader.IsDBNull("pengguna_tamat_perkhidmatan") Then
                        '    lblTarikhAkhirBerkhidmat.InnerText = "Masih Berkhidmat"
                        'Else
                        '    lblTarikhAkhirBerkhidmat.InnerText = reader("pengguna_tamat_perkhidmatan")
                        'End If
                        '-------------------
                    Else
                        Debug.Write("CANNOT READ")
                    End If
                Else
                    Debug.Write("NO ROWS")
                End If
            Catch ex As Exception
                Debug.WriteLine("ERROR(loadUser): " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub ddlunit_load()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim cmd As New SqlCommand("select a.kuarters_id as kuarters_id ,a.kuarters_nama as kuarters_nama from spk_kuarters a 
                                            ", conn)
            Dim ds As New DataSet
            Dim dt As New DataSet
            Dim dr As New DataSet

            Try
                conn.Open()

                Dim da As New SqlDataAdapter(cmd)
                da.Fill(ds)
                ddlcadanganUnit1.DataSource = ds
                ddlcadanganUnit1.DataTextField = "kuarters_nama"
                ddlcadanganUnit1.DataValueField = "kuarters_id"
                ddlcadanganUnit1.DataBind()

                Dim db As New SqlDataAdapter(cmd)
                db.Fill(dt)
                ddlcadanganUnit2.DataSource = dt
                ddlcadanganUnit2.DataTextField = "kuarters_nama"
                ddlcadanganUnit2.DataValueField = "kuarters_id"
                ddlcadanganUnit2.DataBind()

                Dim dc As New SqlDataAdapter(cmd)
                dc.Fill(dr)
                ddlcadanganUnit3.DataSource = dr
                ddlcadanganUnit3.DataTextField = "kuarters_nama"
                ddlcadanganUnit3.DataValueField = "kuarters_id"
                ddlcadanganUnit3.DataBind()
            Catch ex As Exception
                Debug.Write("ERROR: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Protected Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try

            If (e.CommandName = "Cadangan") Then
                Dim strCID = e.CommandArgument.ToString

                Try
                    Dim Getid = oCommon.ExecuteSQL("select permohonan_id from spk_permohonan where pengguna_id = '" & Request.QueryString("uid") & "'")
                    strSQL = "INSERT INTO spk_cadanganUnit (permohonan_id,unit_id1,unit_id2,unit_id3) VALUES ('" & Getid & "','" & ddlcadanganUnit1.SelectedValue.ToString & "','" & ddlcadanganUnit2.SelectedValue.ToString & "','" & ddlcadanganUnit3.SelectedIndex.ToString & "')"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        strlbl_bottom.Text = "Cadangan Kuarters Sudah Dimasukkan"

                    End If
                Catch ex As Exception
                    MsgTop.Attributes("class") = "errorMsg"
                    strlbl_top.Text = strSysErrorAlert
                    MsgBottom.Attributes("class") = "errorMsg"
                    strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message

                End Try

            End If

        Catch ex As Exception
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = strSysErrorAlert & "<br>" & ex.Message
        End Try
    End Sub

    Private Sub TerimaPermohonanKuarters_Click(sender As Object, e As EventArgs) Handles TerimaPermohonanKuarters.Click
        Dim Getid = oCommon.ExecuteSQL("select permohonan_id from spk_permohonan where pengguna_id = '" & Request.QueryString("uid") & "'")
        strSQL = "UPDATE spk_permohonan SET permohonan_sub_status = 'PERMOHONAN KUARTERS DITERIMA, MENANTI PEMBERIAN UNIT' WHERE permohonan_id = '" & Getid & "' "
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            strlbl_bottom.Text = "Cadangan Kuarters Sudah Dimasukkan"

        End If
    End Sub
End Class