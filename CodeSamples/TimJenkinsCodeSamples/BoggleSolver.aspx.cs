using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BoggleSolver;

namespace TimJenkinsCodeSamples
{
    public partial class BoggleSolver : System.Web.UI.Page
    {
        private int _BoardWidth;
        private int _BoardHeight;
        const string BOGGLEBOARDID = "BoggleBoard";
        const string BOGGLEDICTID = "BoggleDictionary";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ReadDimensions();
                GenerateBoardTable(_BoardWidth, _BoardHeight);
            }
        }

        protected void SetDimensions_Click(object sender, EventArgs e)
        {
            //show the "Go" button if we have a valid board
            if (_BoardHeight > 0 && _BoardWidth > 0)
            {
                SolveButton.Visible = true;
            }
            else
            {
                SolveButton.Visible = false;
            }
        }

        protected void ReadDimensions()
        {
            if (!int.TryParse(WidthTextBox.Text, out _BoardWidth) || _BoardWidth < 0)
            {
                _BoardWidth = 0;
            }

            if (!int.TryParse(HeightTextBox.Text, out _BoardHeight) || _BoardHeight < 0)
            {
                _BoardHeight = 0;
            }
        }

        private void GenerateBoardTable(int _BoardWidth, int _BoardHeight)
        {
            TextBox[,] newBoardBoxes = new TextBox[_BoardHeight, _BoardWidth];

            Table newTable = new Table() { ID = BOGGLEBOARDID };
            for (int i = 0; i < _BoardHeight; i++ )
            {
                TableRow newRow = new TableRow();
                for (int j = 0; j < _BoardWidth;j++ )
                {
                    TableCell newCell = new TableCell();
                    TextBox newTextBox = new TextBox()
                    {
                        //set the ID so ASP.NET can repopulate the edit value after postback
                        ID = String.Format("r{0}c{1}", i, j),
                        Width = Unit.Pixel(30),
                        MaxLength = 1,
                    };
                    newBoardBoxes[i, j] = newTextBox;
                    FilteredTextBoxExtender filter = new FilteredTextBoxExtender() { TargetControlID = newTextBox.ID, FilterType = FilterTypes.LowercaseLetters };
                    newCell.Controls.Add(newTextBox);
                    newCell.Controls.Add(filter);
                    newRow.Controls.Add(newCell);
                }
                newTable.Rows.Add(newRow);
            }

            Session[BOGGLEBOARDID] = newBoardBoxes;

            PlaceHolder1.Controls.Clear();
            PlaceHolder1.Controls.Add(newTable);
        }

        protected void SolveButton_Click(object sender, EventArgs e)
        {
            if(_BoardWidth < 1 || _BoardHeight < 1 || Session[BOGGLEBOARDID] == null)
            {
                return;
            }

            TextBox[,] boardTextBoxes = Session[BOGGLEBOARDID] as TextBox[,];

            ErrorLabel.Text = String.Empty;

            char[,] board = new char[_BoardHeight, _BoardWidth];
            for (int i = 0; i < _BoardHeight; i++)
            {
                for (int j = 0; j < _BoardWidth; j++)
                {
                    if( string.IsNullOrEmpty(boardTextBoxes[i,j].Text) )
                    {
                        ErrorLabel.Text = "All positions in the table must contain a letter.";
                        return;
                    }
                    board[i, j] = boardTextBoxes[i, j].Text[0];
                }
            }

            try
            {
                //check for the dictionary in the asp cache
                BoggleDictionary boggleDictionary = Cache[BOGGLEDICTID] as BoggleDictionary;
                if (boggleDictionary == null)
                {
                    //it wasn't in the cache, so load it
                    //this file came from the 12dicts word list: http://wordlist.sourceforge.net/12dicts-readme-r5.html
                    string wordListPath = Server.MapPath("~/App_Data/2of12.txt");
                    boggleDictionary = new BoggleDictionary();
                    boggleDictionary.Load(wordListPath, true);

                    //add it to the cache
                    Cache[BOGGLEDICTID] = boggleDictionary;
                }

                BoggleBoard boggleBoard = new BoggleBoard(board, _BoardWidth, _BoardHeight, boggleDictionary);

                List<string> foundWords = boggleBoard.Solve();
                
                //alphabetical order is easier to read
                foundWords.Sort();

                GridView1.DataSource = foundWords;
                GridView1.DataBind();
            }
            catch(Exception ex)
            {
                ErrorLabel.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }


    }
}