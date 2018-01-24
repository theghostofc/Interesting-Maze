using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterestingMaze
{
    // Which way in the maze we are headed to
    public enum GoingTo
    {
        Start,
        Top,
        Bottom,
        Left,
        Right
    }

    public class Cell
    {
        public int Row;
        public int Col;
        public bool IsWall;
        public int VisitCount;
        public string RowCol { get { return Row + "," + Col; } }
    }

    class Maze
    {
        int size;
        int startRow;
        int startCol;

        // Maximum number of times a block can be parsed.
        // This is to avoid Stackoverflow.
        const int MaxVisitCount = 15;

        Cell[,] cell;
        List<Stack<string>> cellStackList;

        public Maze(int n, int r, int c)
        {
            GenerateMaze(n, r, c);
        }

        public void GenerateMaze(int n, int r, int c)
        {
            size = n;
            startRow = r;
            startCol = c;

            cell = new Cell[size, size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    cell[i, j] = new Cell();
                    cell[i, j].Row = i;
                    cell[i, j].Col = j;

                    // Randomly place blocks.
                    cell[i, j].IsWall = (random.Next(100) % 3 == 0);
                    cell[i, j].VisitCount = 0;
                }
            }

            cellStackList = new List<Stack<string>>();
            cell[startRow, startCol].IsWall = false;
        }

        private List<Stack<string>> Solve(int r, int c, Stack<string> cellStack, GoingTo goingTo)
        {
            if (r < 0 || c < 0)
                return null;
            if (r > size - 1 || c > size - 1)
                return null;
            if (cell[r, c].IsWall)
                return null;
            if (cell[r, c].VisitCount > MaxVisitCount)
                return null;

            List<Stack<string>> listStack = new List<Stack<string>>();
            List<Stack<string>> tempStack = new List<Stack<string>>();

            // If the current cell is already in the stack,
            // we may have a longer path. Optimize it by removing extras.
            while (cellStack.Contains(cell[r, c].RowCol))
            {
                cellStack.Pop();
            }

            // Push current cell back.
            cellStack.Push(cell[r, c].RowCol);
            cell[r, c].VisitCount++;

            if ((r <= 0 || r >= size - 1) && (c <= 0 || c >= size - 1))
            {
                listStack.Add(cellStack);
            }
            else
            {
                // If not going to bottom, go to top.
                if (!goingTo.Equals(GoingTo.Bottom))
                {
                    // Get all the elements from other chains.
                    tempStack = Solve(r - 1, c, cellStack.Clone<string>(), GoingTo.Top);
                    if (tempStack != null && tempStack.Any())
                        listStack.AddRange(tempStack);
                }

                // If not going to right, go to left.
                if (!goingTo.Equals(GoingTo.Right))
                { 
                    tempStack = Solve(r, c - 1, cellStack.Clone<string>(), GoingTo.Left);
                    if (tempStack != null && tempStack.Any())
                        listStack.AddRange(tempStack);
                }

                // If not going to left, go to right.
                if (!goingTo.Equals(GoingTo.Left))
                { 
                    tempStack = Solve(r, c + 1, cellStack.Clone<string>(), GoingTo.Right);
                    if (tempStack != null && tempStack.Any())
                        listStack.AddRange(tempStack);
                }

                // If not going to top, go to bottom.
                if (!goingTo.Equals(GoingTo.Top))
                {
                    tempStack = Solve(r + 1, c, cellStack.Clone<string>(), GoingTo.Bottom);
                    if (tempStack != null && tempStack.Any())
                        listStack.AddRange(tempStack);
                }
            }

            return listStack;
        }

        public List<string> Solve()
        {
            // Get all the possible ways out.
            // This may be optimized.
            cellStackList = Solve(startRow, startCol, new Stack<string>(), GoingTo.Start);
            List<string> allPaths = new List<string>();

            foreach (Stack<string> cellStack in cellStackList)
            {
                allPaths.Add(string.Join(" -> ", cellStack.Reverse()));
            }

            return allPaths.OrderBy(p => p.Length).Distinct().ToList();
        }

        // Create a string representation of the maze.
        public string Draw()
        {
            StringBuilder s = new StringBuilder("\n  ");
            for (int i = 0; i < size; i++)
            {
                s.Append(i.ToString().PadLeft(2));
            }

            s.Append("\n  ╔═");
            for (int i = 0; i < size - 1; i++)
            {
                s.Append("╤═");
            }
            s.Append("╗\n");

            for (int i = 0; i < size; i++)
            {
                s.Append(i.ToString().PadLeft(2));
                string padding = "║";
                for (int j = 0; j < size; j++)
                {
                    // The bigger block shows a blockage in the maze.
                    // The asterisk is the starting point.
                    s.Append(padding + (cell[i, j].IsWall ? "█" : ((i == startRow && j == startCol) ? "*" : " ")));
                    padding = "│";
                }

                s.Append("║\n");
                if (i < size - 1)
                {
                    s.Append("  ╟");
                    for (int j = 0; j < size - 1; j++)
                    {
                        s.Append("─┼");
                    }
                    s.Append("─╢\n");
                }
            }

            s.Append("  ╚");
            for (int i = 0; i < size - 1; i++)
            {
                s.Append("═╧");
            }
            s.Append("═╝\n");

            return s.ToString();
        }
    }
}