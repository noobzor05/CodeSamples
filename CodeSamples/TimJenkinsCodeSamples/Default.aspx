<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TimJenkinsCodeSamples._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>&nbsp;</h2>
            </hgroup>
            <p>
                This is a simple ASP.NET web application to demonstrate a few code samples.&nbsp; Below is a short overview of the problems, and in the upper-right there are links to a page for the solution to each problem.&nbsp;
            </p>
            <p>
                In addition to instructions for running the demonstration, each page also contains a brief description of the problem as well as details on where to find the implementation in the code.</p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Overview:</h3>
    <ol class="round">
        <li class="one">
            <h5>Boggle Solver</h5>
            Take the dimensions for an arbitrary-sized Boggle board and the letters on the board, then find all scorable words on the board.</li>
        <li class="two">
            <h5>ADO.NET Data Layer Abstraction</h5>
            An abstraction of ADO.NET data access.</li>
        <li class="three">
            <h5>Data Processor-Loader Queue</h5>
            A multi-threaded approach to a data processing queue.</li>
    </ol>
</asp:Content>
