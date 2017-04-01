<%@ Page Language="C#" %>
<%@ Import Namespace="OnBarcode.Barcode.ASPNET" %>
<%
	LinearWebStream.drawBarcode(Request, Response);
%>
