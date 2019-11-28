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

        If Not IsPostBack Then
            loadUser()
            readMaklumatAnak()
            loadCadanganKuarters()
        End If

    End Sub
    Private Function readMaklumatAnak() As Boolean
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Dim table As DataTable = New DataTable
            Dim da As New SqlDataAdapter(
                    "SELECT 
	                    * 
                    FROM 
	                    spk_historyAnak
                    WHERE
	                    permohonan_id = " & Request.QueryString("uid") & ";",
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
            Dim cmd As New SqlCommand("
            SELECT
                A.permohonan_id
                ,   D.pengguna_id
                ,   D.pengguna_nama
                ,   D.pengguna_jantina
	            ,   D.pengguna_tarikh_lahir
                ,   F.pangkat_nama
                ,   D.pengguna_no_tentera
                ,   D.pengguna_mula_perkhidmatan
                ,   D.pengguna_tamat_perkhidmatan
                ,   A.permohonan_no_permohonan
                ,   A.kuarters_id
                ,   B.kuarters_nama
                ,   B.pangkalan_id
                ,   C.pangkalan_nama
                ,   A.permohonan_tarikh
                ,   A.permohonan_status
                ,   A.permohonan_sub_status
                ,   A.permohonan_mata
	            ,	E.historyKeluarga_tempat_tinggal
	            ,	E.historyKeluarga_tarikh_mula
				,	(G.unit_blok + '-' + g.unit_tingkat + '-' + g.unit_nombor) as unit_nama
            FROM 
                spk_permohonan A
                JOIN spk_kuarters B ON B.kuarters_id = A.kuarters_id
                JOIN spk_pangkalan C ON C.pangkalan_id = B.pangkalan_id
                JOIN spk_pengguna D ON D.pengguna_id = A.pengguna_id
                JOIN spk_historyKeluarga E ON E.permohonan_id = A.permohonan_id
                JOIN spk_pangkat F ON F.pangkat_id = D.pangkat_id 
                LEFT JOIN spk_unit G ON G.unit_id = A.unit_id
            WHERE A.permohonan_id = '" & Request.QueryString("uid") & "';",
            conn)

            Try
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                If reader.HasRows Then
                    If reader.Read() Then
                        pID.Value = reader("pengguna_id")
                        lblNama.InnerText = reader("pengguna_nama")
                        lblTarikhLahir.InnerText = reader("pengguna_tarikh_lahir")
                        lblJantina.InnerText = reader("pengguna_jantina")
                        lblJawatan.InnerText = reader("pangkat_nama")
                        lblNoTentera.InnerText = reader("pengguna_no_tentera")
                        lblTarikhMulaBerkhidmat.InnerText = reader("pengguna_mula_perkhidmatan")
                        lbl_senaraiPangkalan.InnerText = reader("pangkalan_nama")
                        lbl_senaraiKuarters.InnerText = reader("kuarters_nama")
                        lblKuartersDipohon.Text = reader("kuarters_nama")
                        lblTarikhAkhirBerkhidmat.InnerText = reader("pengguna_tamat_perkhidmatan")
                        lblJenisPenempatan.InnerText = reader("historyKeluarga_tempat_tinggal")
                        lbltarikhPenempatan.InnerText = reader("historyKeluarga_tarikh_mula")

                        If reader("permohonan_sub_status").Equals("TAWARAN UNIT") Then
                            lblUnitDitawarkan.Text = reader("unit_nama")
                            trUnitDitawarkan.Visible = True
                            trStatusKuarters.Visible = False
                        Else
                            If checkKekosongan(Integer.Parse(reader("kuarters_id"))) Then
                                lblStatusKuarter.Text = "ADA KEKOSONGAN"
                                loadUnitAvailable(Integer.Parse(reader("kuarters_id")))
                                pnlPemilihanUnit.Visible = True
                            Else
                                lblStatusKuarter.Text = "TIADA KEKOSONGAN"
                                loadCadanganKuarters(Integer.Parse(reader("pangkalan_id")))
                                If reader("permohonan_sub_status").Equals("CADANGKAN KUARTERS LAIN") Then
                                    ddlCadanganKuarters.Visible = False
                                    btnSimpanCadanganKuarters.Visible = False
                                End If
                                pnlCadanganKuarters.Visible = True
                            End If
                        End If
                    Else
                        Debug.Write("Error(loadUser): CANNOT READ")
                    End If
                Else
                    Debug.Write("Error(loadUser): NO ROWS")
                End If
            Catch ex As Exception
                Debug.WriteLine("ERROR(loadUser): " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub TerimaPermohonanKuarters_Click(sender As Object, e As EventArgs) Handles TerimaPermohonanKuarters.Click
        Dim Getid = oCommon.ExecuteSQL("select permohonan_id from spk_permohonan where pengguna_id = '" & Request.QueryString("uid") & "'")
        strSQL = "UPDATE spk_permohonan SET permohonan_sub_status = 'PERMOHONAN KUARTERS DITERIMA, MENANTI PEMBERIAN UNIT' WHERE permohonan_id = '" & Getid & "' "
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            strlbl_bottom.Text = "Cadangan Kuarters Sudah Dimasukkan"

        End If
    End Sub

    Private Function checkKekosongan(ByVal kuartersID As Integer) As Boolean
        Dim jumlahKekosongan As Integer = 0
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand("SELECT COUNT(*) jumlah_kekosongan FROM spk_unit WHERE unit_status = 'Available' AND kuarters_id = @kuartersID;")
                cmd.Connection = conn
                cmd.Parameters.Add("@kuartersID", SqlDbType.Int).Value = kuartersID
                Try
                    conn.Open()
                    jumlahKekosongan = cmd.ExecuteScalar
                Catch ex As Exception
                    Debug.WriteLine("Error(checkKekosongan): " & ex.Message)
                    Return False
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
        If jumlahKekosongan > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub loadUnitAvailable(ByVal kuartersID As Integer)
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand("
                SELECT 
                    unit_id, 
                    (unit_blok + '-' + unit_tingkat + '-' + unit_nombor) AS nama_unit 
                FROM 
                    spk_unit 
                WHERE 
                    kuarters_id = @kuartersID AND unit_status = 'Available';"
                )
                Dim ds As New DataSet
                cmd.Connection = conn
                cmd.Parameters.Add("@kuartersID", SqlDbType.Int).Value = kuartersID
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds, "AnyTable")
                    ddlUnitKuarters.DataSource = ds
                    ddlUnitKuarters.DataValueField = "unit_id"
                    ddlUnitKuarters.DataTextField = "nama_unit"
                    ddlUnitKuarters.DataBind()
                    ddlUnitKuarters.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                Catch ex As Exception
                    Debug.WriteLine("Error(loadUnitAvailable): " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub

    Protected Function icToAge(ByVal ic As String) As Integer
        Dim year = ic.Substring(0, 2)
        Dim month = ic.Substring(2, 2)
        Dim day = ic.Substring(4, 2)
        Dim dob_string = day & "/" & month & "/" & year
        Dim dob_date = Convert.ToDateTime(dob_string)
        Dim age = Date.Now().Year - dob_date.Year
        Return age
    End Function

    Private Sub btnSimpanTawaranUnit_Click(sender As Object, e As EventArgs) Handles btnSimpanTawaranUnit.Click

        If Not ddlUnitKuarters.SelectedValue = "" Then
            Dim query As String = "
            UPDATE spk_permohonan
            SET 
                permohonan_sub_status = 'TAWARAN UNIT',
                unit_id = " & ddlUnitKuarters.SelectedValue & ",
                permohonan_tarikh = '" & Date.Now().ToString("dd/MM/yyyy") & "'
            WHERE permohonan_id = " & Request.QueryString("uid") & ";"
            strRet = oCommon.ExecuteSQL(query)
            If strRet = "0" Then
                Response.Redirect("Senarai.Permohonan.Menunggu.aspx?P=Pengurusan%20Pentadbiran%20>%20Senarai%20Permohonan%20>%20Senarai%20Permohonan%20Menunggu")
            End If
        Else
            Debug.WriteLine("Error(btnSimpanTawaranUnit): UNIT TAK DIPILIH")
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = "Unit kuarters perlu dipilih"
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = "Unit kuarters perlu dipilih"
        End If

    End Sub

    Private Sub loadCadanganKuarters(ByVal pangkalanID As Integer)
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand("SELECT DISTINCT
	                A.kuarters_id 
	                , A.kuarters_nama
                FROM 
	                spk_kuarters A
	                LEFT JOIN spk_unit B ON B.kuarters_id = A.kuarters_id
                WHERE 
	                A.pangkalan_id = @pangkalanID 
	                AND B.unit_status = 'Available'
                ORDER BY kuarters_nama ASC;
                ")
                Dim ds As New DataSet
                cmd.Connection = conn
                cmd.Parameters.Add("@pangkalanID", SqlDbType.Int).Value = pangkalanID
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds, "AnyTable")
                    ddlCadanganKuarters.DataSource = ds
                    ddlCadanganKuarters.DataValueField = "kuarters_id"
                    ddlCadanganKuarters.DataTextField = "kuarters_nama"
                    ddlCadanganKuarters.DataBind()
                    ddlCadanganKuarters.Items.Insert(0, New ListItem("-- SILA PILIH --", String.Empty))
                Catch ex As Exception
                    Debug.WriteLine("Error(loadCadanganKuarters): " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub


    Private Sub btnTambahCadangan_Click(sender As Object, e As EventArgs) Handles btnTambahCadangan.Click
        If ddlCadanganKuarters.SelectedIndex > 0 Then
            If gvCadanganKuarters.Rows.Count() < 3 Then
                Dim query As String = "INSERT INTO 
                spk_cadanganKuarters(
                    permohonan_id
                    , pengguna_id
                    , kuarters_dicadang
                    , status_dicadang
                    , tarikh_dicadang
                )
                VALUES (
                    " & Request.QueryString("uid") & "
                    ," & pID.Value & "
                    ," & ddlCadanganKuarters.SelectedValue & "
                    ,'DICADANG'
                    ,'" & Date.Now() & "');"
                Try

                    strRet = oCommon.ExecuteSQL(query)
                    If strRet = "0" Then
                        Debug.WriteLine("MSG(btnTambah): SIMPAN BERJAYA")
                        MsgTop.Attributes("class") = "successMsg"
                        strlbl_top.Text = "Disimpan."
                        MsgBottom.Attributes("class") = "successMsg"
                        strlbl_bottom.Text = "Disimpan."
                        loadCadanganKuarters()
                    Else
                        Debug.WriteLine("MSG(btnTambah): SIMPAN BERJAYA")
                        MsgTop.Attributes("class") = "errorMsg"
                        strlbl_top.Text = "Gagal Disimpan.<br/>" & strRet.ToString
                        MsgBottom.Attributes("class") = "errorMsg"
                        strlbl_bottom.Text = "Gagal Disimpan.<br/>" & strRet.ToString
                    End If
                Catch ex As Exception
                    Debug.WriteLine("Error(btnTambahCadangan): " & ex.Message)
                End Try

            Else
                Debug.WriteLine("Error(btnTambahCadangan): LIMIT REACH")
                MsgTop.Attributes("class") = "errorMsg"
                strlbl_top.Text = "Hanya 3 kuarters boleh dicadangkan."
                MsgBottom.Attributes("class") = "errorMsg"
                strlbl_bottom.Text = "Hanya 3 kuarters boleh dicadangkan."
            End If
        Else
            Debug.WriteLine("Error(btnTambahCadangan): KUARTERS TAK DIPILIH")
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = "Kuarters perlu dipilih."
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = "Kuarters perlu dipilih."
        End If
    End Sub

    Private Sub loadCadanganKuarters()
        Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
            Using cmd As New SqlCommand("SELECT
	            A.cadanganKuarters_id
	            , A.kuarters_dicadang
	            , C.pangkalan_nama
	            , B.kuarters_nama
            FROM 
	            spk_cadanganKuarters A
	            JOIN spk_kuarters B ON B.kuarters_id = A.kuarters_dicadang
	            JOIN spk_pangkalan C ON C.pangkalan_id = B.pangkalan_id
            WHERE
	            A.permohonan_id = @permohonanID
	            AND (status_dicadang <> 'DIRUJUK' OR status_dicadang <> 'DIPILIH')
            ORDER BY A.tarikh_dicadang DESC;")
                Dim ds As New DataSet
                cmd.Connection = conn
                cmd.Parameters.Add("permohonanID", SqlDbType.Int).Value = Request.QueryString("uid")
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(ds, "AnyTable")
                    If ds.Tables(0).Rows.Count = 3 Then
                        btnTambahCadangan.Visible = False
                    End If
                    gvCadanganKuarters.DataSource = ds
                    gvCadanganKuarters.DataBind()
                Catch ex As Exception
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub

    Private Sub gvCadanganKuarters_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvCadanganKuarters.RowDeleting
        Dim strCID = gvCadanganKuarters.DataKeys(e.RowIndex).Values("cadanganKuarters_id").ToString
        If Not strCID = "" Then
            strSQL = "DELETE FROM spk_cadanganKuarters WHERE cadanganKuarters_id = " & oCommon.FixSingleQuotes(strCID)
            strRet = oCommon.ExecuteSQL(strSQL)
            If strRet = "0" Then
                loadCadanganKuarters()
            Else
                Debug.WriteLine("Error(gvCadanganKuarters_rowDeleting): strRet != 0")
            End If
        Else
            Debug.WriteLine("Error(gvCadanganKuarters_rowDeleting): strCID = 0")
        End If
    End Sub

    Private Sub btnSimpanCadanganKuarters_Click(sender As Object, e As EventArgs) Handles btnSimpanCadanganKuarters.Click
        If gvCadanganKuarters.Rows.Count > 0 And gvCadanganKuarters.Rows.Count < 4 Then
            Using conn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
                Using cmd As New SqlCommand("UPDATE 
                spk_permohonan 
                SET 
                    permohonan_sub_status = @subStatus
                    ,   permohonan_tarikh = @tarikh
                WHERE permohonan_id = @permohonanID")
                    cmd.Connection = conn
                    cmd.Parameters.Add("@subStatus", SqlDbType.NVarChar, 50).Value = "CADANGKAN KUARTERS LAIN"
                    cmd.Parameters.Add("@tarikh", SqlDbType.NVarChar, 50).Value = Date.Now.ToString("dd/MM/yyyy")
                    cmd.Parameters.Add("@permohonanID", SqlDbType.Int).Value = Request.QueryString("uid")
                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        Debug.WriteLine("MSG(btnTambah): SIMPAN BERJAYA")
                        MsgTop.Attributes("class") = "successMsg"
                        strlbl_top.Text = "Berjaya Disimpan."
                        MsgBottom.Attributes("class") = "successMsg"
                        strlbl_bottom.Text = "Berjaya Disimpan."
                        loadCadanganKuarters()

                    Catch ex As Exception
                        Debug.WriteLine("Error(btnSimpanCadanganKuarters): " & ex.Message)
                    Finally
                        conn.Close()
                    End Try
                End Using
            End Using
        Else
            Debug.WriteLine("Error(btnSimpanCadanganKuarters): TIADA SEBARANG KUARTERS DICADANG")
            MsgTop.Attributes("class") = "errorMsg"
            strlbl_top.Text = "Sila pilih kuarters untuk dicadang."
            MsgBottom.Attributes("class") = "errorMsg"
            strlbl_bottom.Text = "Sila pilih kuarters untuk dicadang."
        End If
    End Sub


End Class