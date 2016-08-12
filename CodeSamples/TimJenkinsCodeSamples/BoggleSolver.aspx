<%@ Page Title="Boggle Solver" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BoggleSolver.aspx.cs" Inherits="TimJenkinsCodeSamples.BoggleSolver" %>

<asp:Content ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>&nbsp;</h2>
            </hgroup>
            <p>
                This is a simple application that finds all valid words in an NxM Boggle board.</p>
            <p>
                For loading the Boggle dictionary, I went with a tree approach where each node represents the beginning of a word (or a complete word), and its children represent all of combinations of adding one additional letter.&nbsp; The root node represents an empty string; the 1st level of children represent a, b, c, etc.; the nth level nodes represent a string of n characters that could be a word or the beginning of a word.&nbsp; Each node has a flag indicating whether that node represents an actual word.&nbsp; And, all leaf nodes (those without children) must always be a valid, complete word (e.g. there are no dead ends).&nbsp; Additionally, the dictionary is stored in the ASP cache so that once loaded, a single instance can be shared by all users.</p>
            <p>
                The search for valid words becomes fairly easy, then.&nbsp; Essentially, the board is treated as a graph, with each letter representing a node with connecting paths to each of the (up to) 8 letters surrounding it.&nbsp; Then, it just does a depth-first search on the graph, with a matrix of &quot;visited&quot; flags to prevent using the same node twice.&nbsp; Each successive letter in the sequence, we check the corresponding dictionary node to see if we have a valid word and whether there are any children deeper in the dictionary.&nbsp; If there are child nodes in the dictionary, then we search one node deeper in the board and check again.&nbsp; If there are no child nodes, then there is no need to search any deeper, since we know that adding any further letters will not result in a valid word.</p>
            <h4>Instructions:</h4>
            <p>
                First, enter the desired dimensions of the board and click &quot;Set Dimensions&quot;.&nbsp; You should then see a number of text frames for entering the values into the Boggle board.&nbsp; After entering all of the letters, click &quot;Go&quot;.&nbsp; You should then see a list of all scoreable words at the bottom of the page.</p>
            <p>
                If there are any errors, you should see a message in red indicating what the errors are.</p>
            <h4>Code Files:</h4>
            <ul>
                <li><b>BoggleSolver.aspx:</b> The web page to run the test.</li>
                <li><b>App_Data\2of12.txt:</b> A text list of common English words to load into the dictionary for testing.</li>
                <li><b>Core\BoggleBoard.cs:</b> Representation of the board and the methods for solving it.</li>
                <li><b>Core\BoggleDictionary.cs:</b> The entry point for loading the dictionary and the object stored in the ASP cache to be shared by all users.</li>
                <li><b>Core\BoggleDictionaryNode.cs:</b> Class for each node in the dictionary tree.  Contains the bulk of the logic for adding words to the dictioanry.</li>
                <li><b>Core\Exceptions\InvalidBoardException.cs:</b> Custom exception thrown when the BoggleBoard fails validation.</li>
                <li><b>Core\Exceptions\InvalidDictionaryException.cs:</b> Custom Exception thrown when the BoggleDictionary fails validation.</li>
            </ul>
        </div>
    </section>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="ErrorLabel" runat="server" style="color:red; font-weight:bold;"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="WidthLabel" runat="server" Text="Width:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="WidthTextBox" runat="server" Width="100px"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="WidthTextBox" FilterType="Numbers" />
            </td>
            <td>
                <asp:Label ID="HeightLabel" runat="server" Text="Height:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="HeightTextBox" runat="server" Width="100px"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="HeightTextBox" FilterType="Numbers" />
            </td>
            <td>
                <asp:Button ID="SetDimensions" runat="server" Text="Set Dimensions" OnClick="SetDimensions_Click"/>
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <asp:Button ID="SolveButton" runat="server" Text="Go" Visible="False" OnClick="SolveButton_Click" />
    <asp:GridView ID="GridView1" runat="server" Caption="Words Found" CaptionAlign="Left">
    </asp:GridView>
</asp:Content>
