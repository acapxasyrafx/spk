﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Konfigurasi.Kuota.Kemaskini.aspx.vb" Inherits="QuartersManagement.Konfigurasi_Kuota_Kemaskini1" %>

<%@ Register Src="~/commoncontrol/configuration/konfigurasi_kuota_kemaskini.ascx" TagPrefix="uc1" TagName="konfigurasi_kuota_kemaskini" %>
<%@ Register Src="~/commoncontrol/menu/navigation_menu.ascx" TagPrefix="uc1" TagName="navigation_menu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:navigation_menu runat="server" id="navigation_menu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc1:konfigurasi_kuota_kemaskini runat="server" id="konfigurasi_kuota_kemaskini" />
</asp:Content>
