<%@ Page Language="C#" %>
<%@ Import Namespace="OnBarcode.Barcode.ASPNET" %>
<%
    DataMatrixWebStream.drawBarcode(Request, Response);
%>
