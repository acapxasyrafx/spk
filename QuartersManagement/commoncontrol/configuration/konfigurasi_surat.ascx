﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="konfigurasi_surat.ascx.vb" Inherits="QuartersManagement.konfigurasi_surat" %>

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
    <tr>
         <td>Isi Surat </td>
         <td>:</td>
         <td colspan="4"><asp:TextBox ID="txtIsiSurat" TextMode ="MultiLine" runat="server" Width="300px" ></asp:TextBox></td>
    </tr>
    <tr>
         <td>Jenis </td>
         <td>:</td>
         <td colspan="4"><asp:TextBox ID="txtJenissurat" runat="server" Width="300px" ></asp:TextBox></td>
    </tr>
       
</table>

<br />

<%-- List --%>
<table class="fbform">
    <tr class="fbform_header">
        <td> 
            Senarai Format Surat
            <asp:Label ID="lblConfig" runat="server" Visible ="false"></asp:Label>
            <asp:Label ID="lblQ" runat="server" Visible ="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="Panel" runat="server" ScrollBars="vertical" Height="350">
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="false"  
             CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="suratTawaranConfig_id"
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

                    <asp:TemplateField HeaderText="Isi" >
                        <ItemTemplate>
                            <asp:Label ID="pangkalan_negara" runat="server" Text='<%# Bind("suratTawaranConfig_parameter")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="40%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Jenis" >
                        <ItemTemplate>
                            <asp:Label ID="pangkalan_negeri" runat="server" Text='<%# Bind("suratTawaranConfig_type")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"  Width ="10%" /><ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                           <span runat="server" style="float:right">
                           <a href ="Konfigurasi.Surat.Tawaran.aspx?edit=<%#Eval("suratTawaranConfig_id")%>&p=<%# lblConfig.Text  %>"><img title="Kemaskini"  src="icons/edit.png" width="13" height="13" alt="::"/></a>
                           | <asp:ImageButton Width ="12" Height ="12" ID="btnDelete" CommandName ="Delete" CommandArgument ='<%#Eval("suratTawaranConfig_id")%>' OnClientClick="javascript:return confirm('Adakah anda pasti mahu memadamkan item ini secara kekal? ')" runat="server" ImageUrl="~/icons/delete.png" ToolTip="Delete"/>
                        </span> 
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="right" VerticalAlign="Top"  /><ItemStyle VerticalAlign="Middle" />
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
                </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="3"></td>
    </tr>
</table>
<table class ="fbform">
    <tr>
        <td><span id ="MsgBottom" runat ="server" ><asp:Label ID ="strlbl_bottom" runat ="server" ></asp:Label></span></td>
    </tr>

</table>


