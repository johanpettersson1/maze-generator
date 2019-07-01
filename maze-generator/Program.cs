using System;
using System.Collections.Generic;
using System.Drawing;

/**
    Project: A recursive backtracking implementation of the maze generation algorithm
    Author: Johan Pettersson
    Date: 2019-07-01
**/

namespace maze_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            // setup and config image
            Image image = new Bitmap(1002, 1002);
            Graphics graph = Graphics.FromImage(image);
            graph.Clear(Color.WhiteSmoke);
            Pen pen = new Pen(Brushes.Black);

            // initiate 100 cells
            Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    cells.Add(y.ToString() + x.ToString(), new Cell(y, x));
                }
            }

            // maze generation algorithm
            Stack<Cell> cellStack = new Stack<Cell>();
            Cell currentCell = cells["00"]; // initiate any cell as the starting point, 00 means top left
            while (true)
            {
                currentCell.visited = true;
                Cell neighbor = Cell.GetRandomNeighbor(currentCell, cells);
                if (neighbor != null)
                {
                    cellStack.Push(currentCell);
                    Cell.Dig(currentCell, neighbor);
                    currentCell = neighbor;
                }
                else if (cellStack.Count > 0)
                {
                    currentCell = cellStack.Pop();
                }
                else
                {
                    break;
                }
            }

            // generate image
            foreach (var cell in cells)
            {
                cell.Value.Draw(graph, pen);
            }

            image.Save("maze.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public class Cell
        {
            public int y;
            public int x;
            public bool visited = false;

            public bool north = true;
            public bool east = true;
            public bool south = true;
            public bool west = true;

            public Cell(int y, int x)
            {
                this.y = y;
                this.x = x;
            }

            public void Draw(Graphics graph, Pen pen)
            {
                if (north)
                    graph.DrawLine(pen, new Point(this.x * 100, this.y * 100), new Point(this.x * 100 + 100, this.y * 100));
                if (south)
                    graph.DrawLine(pen, new Point(this.x * 100, this.y * 100 + 100), new Point(this.x * 100 + 100, this.y * 100 + 100));
                if (east)
                    graph.DrawLine(pen, new Point(this.x * 100 + 100, this.y * 100), new Point(this.x * 100 + 100, this.y * 100 + 100));
                if (west)
                    graph.DrawLine(pen, new Point(this.x * 100, this.y * 100), new Point(this.x * 100, this.y * 100 + 100));
            }

            public static Cell GetRandomNeighbor(Cell currentCell, Dictionary<string, Cell> cells)
            {
                List<Cell> randomCellList = new List<Cell>(4);
                if (currentCell.y > 0)
                {
                    Cell c = cells[(currentCell.y - 1).ToString() + currentCell.x];
                    if (c.visited == false)
                        randomCellList.Add(c);
                }
                if (currentCell.y < 9)
                {
                    Cell c = cells[(currentCell.y + 1).ToString() + currentCell.x];
                    if (c.visited == false)
                        randomCellList.Add(c);
                }
                if (currentCell.x > 0)
                {
                    Cell c = cells[(currentCell.y).ToString() + (currentCell.x - 1)];
                    if (c.visited == false)
                        randomCellList.Add(c);
                }
                if (currentCell.x < 9)
                {
                    Cell c = cells[(currentCell.y).ToString() + (currentCell.x + 1)];
                    if (c.visited == false)
                        randomCellList.Add(c);
                }

                if (randomCellList.Count == 0) return null;

                Random random = new Random();
                int randomNeighbor = random.Next(0, randomCellList.Count);
                return randomCellList[randomNeighbor];
            }

            public static void Dig(Cell current, Cell neighbor)
            {
                if (current.y > neighbor.y)
                {
                    current.north = false;
                    neighbor.south = false;
                }
                else if (current.y < neighbor.y)
                {
                    current.south = false;
                    neighbor.north = false;
                }
                else if (current.x < neighbor.x)
                {
                    current.east = false;
                    neighbor.west = false;
                }
                else
                {
                    current.west = false;
                    neighbor.east = false;
                }
            }
        }
    }
}
