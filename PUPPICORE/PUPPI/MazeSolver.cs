using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections;


namespace Mehroz
{
    // <summary>
    // Summary description for MazeSolver.
    // Class name: MazeSolver
    // Developed by: Syed Mehroz Alam
    // Email: smehrozalam@yahoo.com
    // URL: Programming Home "http://www.geocities.com/smehrozalam/"
    // 
    // Constructors:
    // 	( int[,] ):	takes 2D integer array	
    // 	( int Rows, int Cols )	initializes the dimensions, indexers may be used 
    // 							to set individual elements' values
    // 
    // Properties:
    // 	Rows: returns the no. of rows in the current maze
    // 	Cols: returns the no. of columns in the current maze
    // 	Maze: returns the current maze as a 2D array
    // 	PathCharacter: to get/set the value of path tracing character
    // 	AllowDiagonal: whether diagonal paths are allowed
    // 
    // Indexers:
    // 	[i,j] = used to set/get elements of maze
    // 
    // internal Methods (Description is given with respective methods' definitions)
    //		int[,] FindPath(int iFromY, int iFromX, int iToY, int iToX)
    // 
    // Private Methods
    //		void GetNodeContents(int[,] iMaze, int iNodeNo)
    //		void ChangeNodeContents(int[,] iMaze, int iNodeNo, int iNewValue)
    //		int[,] Search(int iBeginningNode, int iEndingNode)
    // 
    // Modified by Mihai Pruna 2014 to output stack of x and y coords
    // </summary>
    delegate void MazeChangedHandler(int iChanged, int jChanged);
    class MazeSolver
    {

        // <summary>
        // Class attributes/members
        // </summary>
        int[,] m_iMaze;
        //horizontal maze
        int[,] h_iMaze;
        //vertical maze
        int[,] v_iMaze;
        int m_iRows;
        int m_iCols;
        int iPath = 100;
        bool diagonal = false;
        internal ArrayList xindex;
        internal ArrayList yindex;
        internal event MazeChangedHandler OnMazeChangedEvent;

        // <summary>
        // Constructor 1: takes a 2D integer array
        // </summary>
        internal MazeSolver(int[,] iMaze, int[,] hMaze, int[,] vMaze)
        {
            //all arrays need to have same size
            m_iMaze = (int[,])iMaze.Clone();
            h_iMaze = hMaze;
            //  h_iMaze = vMaze;

            v_iMaze = vMaze;
            //v_iMaze = hMaze;


            m_iRows = iMaze.GetLength(0);
            m_iCols = iMaze.GetLength(1);
            //reverse order arraylist
            xindex = new ArrayList();
            yindex = new ArrayList();
        }

        // <summary>
        // Constructor 2: initializes the dimensions of maze, 
        // later, indexers may be used to set individual elements' values
        // </summary>
        internal MazeSolver(int iRows, int iCols)
        {
            m_iMaze = new int[iRows, iCols];
            m_iRows = iRows;
            m_iCols = iCols;
        }

        // <summary>
        // Properites:
        // </summary>
        internal int Rows
        {
            get { return m_iRows; }
        }

        internal int Cols
        {
            get { return m_iCols; }
        }
        internal int[,] GetMaze
        {
            get { return m_iMaze; }
        }
        internal int PathCharacter
        {
            get { return iPath; }
            set
            {
                if (value == 0)
                    throw new Exception("Invalid path character specified");
                else
                    iPath = value;
            }
        }
        internal bool AllowDiagonals
        {
            get { return diagonal; }
            set { diagonal = value; }
        }


        // <summary>
        // Indexer
        // </summary>
        internal int this[int iRow, int iCol]
        {
            get { return m_iMaze[iRow, iCol]; }
            set
            {
                m_iMaze[iRow, iCol] = value;
                if (this.OnMazeChangedEvent != null)	// trigger event
                    this.OnMazeChangedEvent(iRow, iCol);
            }
        }

        // <summary>
        // The function is used to get the contents of a given node in a given maze,
        //  specified by its node no.
        // </summary>
        private int GetNodeContents(int[,] iMaze, int iNodeNo)
        {
            int iCols = iMaze.GetLength(1);
            return iMaze[iNodeNo / iCols, iNodeNo - iNodeNo / iCols * iCols];
        }

        // <summary>
        // The function is used to change the contents of a given node in a given maze,
        //  specified by its node no.
        // </summary>
        private void ChangeNodeContents(int[,] iMaze, int iNodeNo, int iNewValue)
        {
            int iCols = iMaze.GetLength(1);
            iMaze[iNodeNo / iCols, iNodeNo - iNodeNo / iCols * iCols] = iNewValue;
        }

        // <summary>
        // This internal function finds the shortest path between two points
        // in the maze and return the solution as an array with the path traced 
        // by "iPath" (can be changed using property "PathCharacter")
        // if no path exists, the function returns null
        // </summary>
        internal int[,] FindPath(int iFromY, int iFromX, int iToY, int iToX)
        {
            int iBeginningNode = iFromY * this.Cols + iFromX;
            int iEndingNode = iToY * this.Cols + iToX;
            return (Search(iBeginningNode, iEndingNode));
        }


        // <summary>
        // Internal function for that finds the shortest path using a technique
        // similar to breadth-first search.
        // It assigns a node no. to each node(2D array element) and applies the algorithm
        // </summary>
        private enum Status
        { Ready, Waiting, Processed }
        private int[,] Search(int iStart, int iStop)
        {
            const int empty = 0;

            int iRows = m_iRows;
            int iCols = m_iCols;
            int iMax = iRows * iCols;
            int[] Queue = new int[iMax];
            int[] Origin = new int[iMax];
            int iFront = 0, iRear = 0;

            //check if starting and ending points are valid (open)
            if (GetNodeContents(m_iMaze, iStart) != empty || GetNodeContents(m_iMaze, iStop) != empty)
            {
                return null;
            }

            //create dummy array for storing status
            int[,] iMazeStatus = new int[iRows, iCols];
            //initially all nodes are ready

        //    Parallel.For(0, iRows, i =>
        //{
        //    for (int j = 0; j < iCols; j++)
        //        iMazeStatus[i, j] = (int)Status.Ready;
        //});

            for (int i = 0; i < iRows; i++)
                for (int j = 0; j < iCols; j++)
                    iMazeStatus[i, j] = (int)Status.Ready;

            //add starting node to Q
            Queue[iRear] = iStart;
            Origin[iRear] = -1;
            iRear++;
            int iCurrent, iLeft, iRight, iTop, iDown, iRightUp, iRightDown, iLeftUp, iLeftDown;
            double divsize = 1.0 / (double)iCols;
            int numberMoves = 0;
            Random R = new Random();
            //switch order around so we use empty spaces better
            int r;
            int rr = R.Next(3) + 1;
            while (iFront != iRear)	// while Q not empty	
            {
                if (Queue[iFront] == iStop)		// maze is solved
                    break;

                iCurrent = Queue[iFront];


                int d = Math.DivRem(numberMoves, 4, out r);
                //L/R dominant
                if (r < rr)
                {

                    iLeft = iCurrent - 1;
                    //if (iLeft >= 0 && iLeft /iCols == iCurrent / iCols) 	//if left node exists
                    if (iLeft >= 0 && iLeft / iCols == iCurrent / iCols)
                        if (GetNodeContents(h_iMaze, iLeft) == empty)
                            //if (GetNodeContents(m_iMaze, iLeft) == empty) 	//if left node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iLeft) == (int)Status.Ready)	//if left node is ready
                            {
                                Queue[iRear] = iLeft; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iLeft, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }

                    iRight = iCurrent + 1;
                    if (iRight < iMax && iRight / iCols == iCurrent / iCols) 	//if right node exists
                        if (GetNodeContents(h_iMaze, iRight) == empty)
                            //if (GetNodeContents(m_iMaze, iRight) == empty) 	//if right node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iRight) == (int)Status.Ready)	//if right node is ready
                            {
                                Queue[iRear] = iRight; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iRight, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }
                    //try more

                    iTop = iCurrent - iCols;
                    if (iTop >= 0) 	//if top node exists
                        if (GetNodeContents(v_iMaze, iTop) == empty)
                            //if (GetNodeContents(m_iMaze, iTop) == empty) 	//if top node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iTop) == (int)Status.Ready)	//if top node is ready
                            {
                                Queue[iRear] = iTop; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iTop, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }

                    iDown = iCurrent + iCols;
                    if (iDown < iMax) 	//if bottom node exists
                        if (GetNodeContents(v_iMaze, iDown) == empty)
                            // if (GetNodeContents(m_iMaze, iDown) == empty) 	//if bottom node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iDown) == (int)Status.Ready)	//if bottom node is ready
                            {
                                Queue[iRear] = iDown; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iDown, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }

                }
                //up down dominant
                else
                {

                    iTop = iCurrent - iCols;
                    if (iTop >= 0) 	//if top node exists
                        if (GetNodeContents(v_iMaze, iTop) == empty)
                            //if (GetNodeContents(m_iMaze, iTop) == empty) 	//if top node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iTop) == (int)Status.Ready)	//if top node is ready
                            {
                                Queue[iRear] = iTop; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iTop, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }

                    iDown = iCurrent + iCols;
                    if (iDown < iMax) 	//if bottom node exists
                        if (GetNodeContents(v_iMaze, iDown) == empty)
                            // if (GetNodeContents(m_iMaze, iDown) == empty) 	//if bottom node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iDown) == (int)Status.Ready)	//if bottom node is ready
                            {
                                Queue[iRear] = iDown; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iDown, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }
                    //try more

                    iLeft = iCurrent - 1;
                    //if (iLeft >= 0 && iLeft /iCols == iCurrent / iCols) 	//if left node exists
                    if (iLeft >= 0 && iLeft / iCols == iCurrent / iCols)
                        if (GetNodeContents(h_iMaze, iLeft) == empty)
                            //if (GetNodeContents(m_iMaze, iLeft) == empty) 	//if left node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iLeft) == (int)Status.Ready)	//if left node is ready
                            {
                                Queue[iRear] = iLeft; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iLeft, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }

                    iRight = iCurrent + 1;
                    if (iRight < iMax && iRight / iCols == iCurrent / iCols) 	//if right node exists
                        if (GetNodeContents(h_iMaze, iRight) == empty)
                            //if (GetNodeContents(m_iMaze, iRight) == empty) 	//if right node is open(a path exists)
                            if (GetNodeContents(iMazeStatus, iRight) == (int)Status.Ready)	//if right node is ready
                            {
                                Queue[iRear] = iRight; //add to Q
                                Origin[iRear] = iCurrent;
                                ChangeNodeContents(iMazeStatus, iRight, (int)Status.Waiting); //change status to waiting
                                iRear++;
                            }



                }
                //change status of current node to processed
                ChangeNodeContents(iMazeStatus, iCurrent, (int)Status.Processed);
                iFront++;
                numberMoves++;
            }

            //create an array(maze) for solution
            int[,] iMazeSolved = new int[iRows, iCols];
            iMazeSolved = m_iMaze;
            //for (int i = 0; i < iRows; i++)
            //    for (int j = 0; j < iCols; j++)
            //        iMazeSolved[i, j] = m_iMaze[i, j];

            //make a path in the Solved Maze
            iCurrent = iStop;
            ChangeNodeContents(iMazeSolved, iCurrent, iPath);
            xindex.Add(Convert.ToInt32(iCurrent / iCols));
            yindex.Add(Convert.ToInt32(iCurrent - Convert.ToInt32(iCurrent / iCols) * iCols));
            for (int i = iFront; i >= 0; i--)
            {
                if (Queue[i] == iCurrent)
                {
                    iCurrent = Origin[i];
                    if (iCurrent == -1)		// maze is solved
                        return (iMazeSolved);
                    ChangeNodeContents(iMazeSolved, iCurrent, iPath);
                    //insert at 0
                    xindex.Insert(0, Convert.ToInt32(iCurrent / iCols));
                    yindex.Insert(0, Convert.ToInt32(iCurrent - Convert.ToInt32(iCurrent / iCols) * iCols));

                }
            }

            //no path exists
            return null;
        }
    }

}

