<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="ServicioBecario.Vistas.Reportes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
       <div class="col-md-12 text-center">
           <h1><label>Reportes</label></h1>    
           <hr />
       </div>
   </div>
    <div class="col-md-2">
        <asp:Button  ID="btnBecarios" CssClass="btn btn-primary" runat="server" Text="Becarios" OnClick="btnBecarios_Click" />
    </div>
    <div class="col-md-2">
        <asp:Button  runat="server" ID="btnSNovaluado" Text="SB No evaluados" CssClass="btn btn-primary" OnClick="btnSNovaluado_Click"/>
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnReasignacion" Text="Reasignacion" runat="server" CssClass="btn btn-primary" OnClick="btnReasignacion_Click" />
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnproyectos" Text="Proyectos" runat="server" CssClass="btn btn-primary" OnClick="btnproyectos_Click"/>
    </div>

</asp:Content>
