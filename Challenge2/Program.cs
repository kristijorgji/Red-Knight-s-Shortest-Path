// <author>Kristi Jorgji</author>
// <date>9/13/2016</date>
// <summary>Task 2 solution</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Challenge2
{

    class Node
    {
        public Location Location { get; set; }
        public Node Parent { get; set; }

        public Node(int x, int y)
        {
            this.Location = new Location(x, y);
        }

        public override int GetHashCode()
        {
            return Int32.Parse("1" + Location.X.ToString() + "2" + Location.Y.ToString()) % Int32.MaxValue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Node objAsNode = obj as Node;
            if (objAsNode == null)
                return false;
            else return Equals(objAsNode);
        }

        public bool Equals(Node other)
        {
            if (other == null)
                return false;

            return (this.Location.X == other.Location.X && this.Location.Y == other.Location.Y);
        }
    }

    class Location
    {
        // x is the coordinate from left to right which increases as we go right
        // y is the coordinate from up to down which increases as we go down
        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {
            X = 0;
            Y = 0;
        }

        public Location(int x, int y)
        {
            if (Board.CheckBorders(x, y))
            {
                X = x;
                Y = y;
            }
            //if coordinates outside board, put default 0, 0 by calling default const
        }
    }

    static class Board
    {
        public static int boardSize = 7; //default
        public static bool CheckBorders(int x, int y)
        {
            return !(x > boardSize - 1 || x < 0 || y > boardSize - 1 || y < 0);
        }
    }

    class RedKnight
    {
        public Location Location { get; set; }

        public RedKnight()
        {
            this.Location = new Location();
        }

        public RedKnight(int x, int y)
        {
            this.Location = new Location(x, y);
        }

        public static string GetMoveName(int x, int y, int xDest, int yDest)
        {
            if (xDest - x == 2 && yDest - y == 0)
                return "R";

            if (xDest - x == -2 && yDest - y == 0)
                return "L";

            if (xDest - x == 1 && yDest - y == -2)
                return "UR";

            if (xDest - x == 1 && yDest - y == 2)
                return "LR";

            if (xDest - x == -1 && yDest - y == -2)
                return "UL";

            if (xDest - x == -1 && yDest - y == 2)
                return "LL";

            return "Invalid move";
        }

        public static List<Location> AvailableMoves(Location currentLoc)
        {
            List<Location> result = new List<Location>();
            int xDest = 0;
            int yDest = 0;

            //UL
            xDest = currentLoc.X - 1;
            yDest = currentLoc.Y - 2;

            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            //UR
            xDest = currentLoc.X + 1;
            yDest = currentLoc.Y - 2;

            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            // R
            xDest = currentLoc.X + 2;
            yDest = currentLoc.Y;
            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            //LR
            xDest = currentLoc.X + 1;
            yDest = currentLoc.Y + 2;

            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            //LL
            xDest = currentLoc.X - 1;
            yDest = currentLoc.Y + 2;

            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            //L
            xDest = currentLoc.X - 2;
            yDest = currentLoc.Y;

            if (Board.CheckBorders(xDest, yDest))
            {
                result.Add(new Location(xDest, yDest));
            }

            return result;
        }
    }

    class Program
    {
        public static HashSet<Node> visitedNodes = new HashSet<Node>();
        
        static void Main(string[] args)
        {
            //Console.WriteLine("Input sample: 7 6 6 0 1");
            int bSize, y, x, yDest, xDest;
            string input = Console.ReadLine();
            //string input;
            //input = "7 6 6 0 1";
            //input = "5 4 1 0 3";
            try
            {
                var parts = input.Split(' ');
                bSize = Convert.ToInt32(parts[0]);
                y = Convert.ToInt32(parts[1]);
                x = Convert.ToInt32(parts[2]);
                yDest = Convert.ToInt32(parts[3]);
                xDest = Convert.ToInt32(parts[4]);
            }
            catch(Exception e)
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            Board.boardSize = bSize;
          
            // x is the coordinate from left to right which increases as we go right
            // y is the coordinate from up to down which increases as we go down

            Node start = new Node(x, y);
            Node destination = new Node(xDest, yDest);
            List<Node> shortestPath = FindShortestPath(start, start, destination.Location);
            PrintResult(shortestPath);
        }

        public static void PrintResult(List<Node> path)
        {
            if (path != null && path.Count > 1) //valid path
            {
                Console.WriteLine(path.Count - 1);
                for (int c = 0; c < path.Count - 1; c++)
                {
                    Console.Write(RedKnight.GetMoveName(path[c].Location.X, path[c].Location.Y, path[c + 1].Location.X, path[c + 1].Location.Y));
                    if (c < path.Count - 2)
                        Console.Write(" ");
                    //Console.WriteLine("{1} {0} to {3} {2}, move Type: {4}",
                    //    path[c].Location.X, path[c].Location.Y,
                    //    path[c + 1].Location.X, path[c + 1].Location.Y,
                    //    RedKnight.GetMoveName(path[c].Location.X,
                    //    path[c].Location.Y, path[c + 1].Location.X, path[c + 1].Location.Y));
                }
            }
            else
            {
                Console.WriteLine("Impossible");
            }
        }

        public static List<Node> GetAvailableNodes(Node parent, Location destination)
        {
            List<Node> result = new List<Node>();
            List<Location> availableMoves = RedKnight.AvailableMoves(parent.Location);

            foreach (Location l in availableMoves)
            {
                //Console.WriteLine("{0} {1}", l.X, l.Y);
                Node n = new Node(l.X, l.Y);
                n.Parent = parent;

                if (!visitedNodes.Contains(n))
                {
                    result.Add(n);
                    visitedNodes.Add(n);
                }
            }

            return result;
        }

        public static List<Node> FindShortestPath(Node start, Node current, Location destination)
        {
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(current);
            while (q.Count > 0)
            {
                current = q.Dequeue();
                if (current == null)
                    continue;
                List<Node> neighboors = GetAvailableNodes(current, destination);
                foreach (Node n in neighboors)
                {
                    n.Parent = current;
                    q.Enqueue(n);
                }

                if (current.Location.X == destination.X && current.Location.Y == destination.Y)
                {
                    return FormPath(current);
                }
            }
         
            return null;
        }

        // backtrack to find path from end to beggining, r is end node
        public static List<Node> FormPath(Node r)
        {
            List<Node> path = new List<Node>();
            path.Add(r);
            path.Add(r.Parent);
            Node n = r.Parent;
            while (n != null)
            {
                //Console.WriteLine("{0} {1}", n.Location.X, n.Location.Y);
                n = n.Parent;
                path.Add(n);
            }

            path.RemoveAll(no => no == null);
            path.Reverse();
            return path;
        }
    }
}
