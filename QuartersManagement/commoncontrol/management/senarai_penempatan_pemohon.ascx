﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="senarai_penempatan_pemohon.ascx.vb" Inherits="QuartersManagement.senarai_penempatan_pemohon" %>
<style>
    select{
        width: 20em;
    }
</style>

<table class ="fbform" style ="width :100%">
    <tr class ="fbform_header">
         <td><span id="MsgTop" runat ="server"><asp:Label ID ="strlbl_top" runat ="server" ></asp:Label></span></td>
         <td>
            <span class="buttonMenu"><a href="#" runat ="server" id="SaveFunction"><img title="Save" style="vertical-align: middle;" src="icons/save.png" width="25" height="25" alt="::"/></a>
            | <a href ="#"  id ="Refresh" runat ="server"  ><img title="Refresh" style="vertical-align: middle;" src="icons/refresh.png" width="22" height="22" alt="::"/></a>
                | <a href ="#"  id ="Help" ><img title="Help" style="vertical-align: middle;" src="icons/help.png" width="22" height="22" alt="::"/></a>
            
            </span>
         </td>
    </tr>
</table>

<table class="fbform" style="width :100%">
    <tr class="fbform_mheader">
        <td colspan="4">Saringan</td>
    </tr>
    <tr>
        <td style="width:150px;">Pangkalan</td>
        <td>:</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarianPangkalan" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width:150px;">Kuarters</td>
        <td>:</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarianKuarters" AutoPostBack="true" Enabled="false">
                <asp:ListItem Text="-- Sila Pilih pangkalan Terlebih Dahulu --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width:150px;">Pangkat</td>
        <td>:</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarianPangkat" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr class="fbform_mheader">
        <td colspan="3">Carian</td>
    </tr>
    <tr>
        <td style="width:150px;">Nama/No.Tentera</td>
        <td>:</td>
        <td><asp:TextBox runat="server" ID="tbCarianNama" style="width: 20em;"/></td>
    </tr>
    <tr>
        <td><asp:Button Text="Cari" runat="server" ID="btnCari"/></td>
    </tr>
    <tr class="fbform_mheader">
        <td colspan="4">Senarai Pemohon dan Kuarters Penempatan</td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:GridView 
                ID="datRespondent" 
                runat="server" 
                AutoGenerateColumns="False" 
                AllowPaging="false"
                CellPadding="4" 
                ForeColor="#333333" 
                GridLines="None" 
                DataKeyNames="permohonan_id"
                Width="100%" PageSize="100" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>

                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Nama Pemohon" >
                        <ItemTemplate>
                            <asp:Label ID="pengguna_nama" runat="server" Text='<%# Bind("pengguna_nama")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="40%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Unit Diberi" >
                        <ItemTemplate>
                            <asp:Label ID="unit_nama" runat="server" Text='<%# If(Eval("unit_nama"), Eval("unit_nama_lain")) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="10%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   

                    <asp:TemplateField HeaderText="Kuarters">
                        <ItemTemplate>
                            <asp:Label ID="kuarter_nama" runat="server" Text='<%# Bind("kuarters_nama")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="20%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Pangkalan" >
                        <ItemTemplate>
                            <asp:Label ID="pangkalan_nam" runat="server" Text='<%# Bind("pangkalan_nama")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="10%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Tindakan" >
                        <ItemTemplate>
                            <asp:ImageButton
                                Width="25px"
                                Height="25px"
                                runat="server" 
                                ID="btnView" 
                                CommandName="View_Permohonan" 
                                CommandArgument='<%#Eval("permohonan_id") %>'
                                ImageUrl="~/icons/test.svg"
                                ToolTip="Buka?"
                            />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="40%" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Underline="true" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" CssClass="cssPager" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                    HorizontalAlign="Center" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </td>
    </tr>
</table>

<table class ="fbform">
    <tr>
        <td><span id ="MsgBottom" runat ="server" ><asp:Label ID ="strlbl_bottom" runat ="server" ></asp:Label></span></td>
    </tr>

</table>
