<%@ Page Language="C#" MasterPageFile="~/master/Default.Master" AutoEventWireup="true" CodeBehind="titlepage.aspx.cs" Inherits="ExamineSystem.titlepage" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
<frameset framespacing="0" border="0" frameborder="0" cols="159,*">
  <frame name="left" scrolling="no" noresize src="pageleft.aspx">
  <frame name="right" scrolling="auto" noresize src="pagestart.aspx">
  <noframes></noframes>
</frameset>
</asp:Content>