﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Proses.Penempatan.Tawaran.aspx.vb" Inherits="QuartersManagement.Proses_Penempatan_Tawaran" %>

<%@ Register Src="~/commoncontrol/penempatan_kuarters/status_tawaran.ascx" TagPrefix="uc1" TagName="status_tawaran" %>
<%@ Register Src="~/commoncontrol/menu/navigation_menu.ascx" TagPrefix="uc1" TagName="navigation_menu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:navigation_menu runat="server" id="navigation_menu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc1:status_tawaran runat="server" id="status_tawaran" />
</asp:Content>
