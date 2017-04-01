<%@ Page Language="C#" %>
<%@ Import Namespace="OnBarcode.Barcode.ASPNET" %>
<%
    QRCodeWebStream.drawBarcode(Request, Response);
%>
